using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class TrainingPanel1 : PanelSettings
{
    //panel to show when the session has ended
    public PanelSettings panelWhenEnd;

    public TypeOfSession session;
    public TypeOfStage[] sequence;

    private int sequenceIndex;
    private TypeOfStage currentStage;

    private string jsonFile;
    private Stage stage;

    private string emmaSpeech;

    //current block text
    private Block currentBlock;
    private int blockIndex = 0;
    private int emmaIndex = 0;
    private int step;
    // text to display
    public Text emmaField;

   // private Slider progressSlider;

    //Emotion list for displaying random emotion like in exercice 1
    private TypeOfEmotion currentEmotion;
    //if random //private HashSet<TypeOfEmotion> emotionList = new HashSet<TypeOfEmotion>();
    public List<TypeOfEmotion> emotionList = new List<TypeOfEmotion>();

    // when an exercice needs to be reload
    private int cycle;
    public int maxCycle;

    //time remaining with the autoskip
    private SkipDialogueCircle skipCircle;
    private float duration;

    // score for each exercice
    protected int score;

    // for DEBUG ONLY
    //public GameObject prefabButtonDebug;
    private int skip;
    public  GameObject buttonPREF;
    public GameObject debugButtonPREF;

    public ProfilePanel profilScreen;

    //orientation
    private DeviceOrientation currentOrientation;

    protected List<GameObject> listOfInstantiedButtons = new List<GameObject>();

    private void Start()
    {
        skipCircle = GetComponentInChildren<SkipDialogueCircle>();
        skipCircle.gameObject.SetActive(false);

        currentOrientation = DeviceOrientation.Portrait;

        choicesPanel = GetComponentInChildren<GridLayoutGroup>().gameObject;
    }

    public override void Activate()
    {
        base.Activate();
        LoadJSON();
    }

    void ResetIndexes()
    {
        blockIndex = 0;
        emmaIndex = 0;
        step = 0;
        cycle = 0;
        score = 0;
    }

    private void LoadJSON()
    {
        //check if there is an exercice to load, if not we've reached the end of the session, save the score and change to profil panel
        if (sequenceIndex != sequence.Length)
            currentStage = sequence[sequenceIndex];
        else
        {
            TitleDisplay.Change.Title = "";
            TitleDisplay.Change.SubTitle = "";
            //profilScreen.AddProgress();
            //save score to the student profil
            GameManager.currentStudentSet.scores.Add(new Score { session = this.session, score = this.score});
            GameManager.currentStudentSet.score1 = score;
            GameManager.currentStudentSet.progress++;
            SaveSystem.SaveStudent(StudentsListPanel.listOfStudents);

            ScreenManager.Actual.ChangeToPanel(panelWhenEnd);
            return;
        }

        // when loading a new exercice, reset the lines and score variables 
        ResetIndexes();

        TextAsset textfile = Resources.Load<TextAsset>("JSON/" + session + "/" + currentStage.ToString());
        jsonFile = textfile.text;


        if (currentStage == TypeOfStage.Exercice1)
            maxCycle = 0;
        else
            maxCycle = 5;
        if (currentStage == TypeOfStage.Exercice3)
            maxCycle = 99;

        stage = JsonUtility.FromJson<Stage>(jsonFile);

        //update the subtitle text
        TitleDisplay.Change.Title = "Session 1 - " + currentStage.ToString();
        TitleDisplay.Change.SubTitle = GameManager.currentStudentSet.name;

        Load();
    }

    private void Load()
    {

        DisplayLine();
    }

    public void NextLine()
    {
        if (blockIndex == stage.block.Length)
        {
            sequenceIndex++;
            LoadJSON();
        }
        else
            DisplayLine();
    }


    public void DisplayLine()
    {
        foreach (Button butt in GetComponentsInChildren<Button>())
            Destroy(butt);

        StopAllCoroutines(); // stop the autonext

        currentBlock = stage.block[blockIndex];

        // Display the text from Emma
        emmaSpeech = "#error";
        emmaSpeech = currentBlock.text;
        Debug.LogWarning("emma speech is : " + emmaSpeech);

        step++;

//--------------- STRING REPLACEMENTS--------------------------------------------       

        emmaField.text = emmaSpeech;

        // if there is any choices, display them, else display "Next"
        // the first line of emma always bring the questions, the next lines always bring next

        SpawnButtons();
    }


//--------------- SPAWN BUTTONS --------------------------------------------------------------------------------------------- 
    private void SpawnButtons()
    {
        foreach (GameObject button in listOfInstantiedButtons)
            Destroy(button);

        int choicesCount = currentBlock.choices.Count;
        // if first line of emma for each block and there is a choice key

        // set the vertical layout as a parent
        if (Input.deviceOrientation == DeviceOrientation.Portrait || Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown)
            //codes for portrait
            choicesPanel = GetComponentInChildren<VerticalLayoutGroup>().gameObject;
        else
            //code for landscape
            choicesPanel = GetComponentInChildren<GridLayoutGroup>().gameObject;

        if ( choicesCount > 0)
        {
            //deactivate the skip UI, there is no auto-skip
            skipCircle.gameObject.SetActive(false);
            //if there is a lot of button to display, set the grid layout as the parent
            if (choicesCount > 4)
                choicesPanel = GetComponentInChildren<GridLayoutGroup>().gameObject;
            // Spawn a button per choice F1 F2 F.....
            for (int i = 0; i < choicesCount; i++)
            {
                ChoiceButton newChoice = new ChoiceButton
                {
                    id = i,
                    text = currentBlock.choices[i].btn
                };
                //newChoice.id = i+1;
                //newChoice.text = "F" + (i+1) + " : " + currentBlock.choix[i];
                SpawnChoices(newChoice);
            }
        }
        // if it's not a choice, it's a sequence, emma is talking, no button to spawn expect the debug ones
        else
        {
            step=0;
            
            // spawn debug button to skip
            ChoiceButton debugSkip = new ChoiceButton
            {
                id = currentBlock.choices.Count + 1,
                text = "DEV : Ignorer"
            };
            SpawnDebugButton(debugSkip);

            if (emmaSpeech.Contains("On fait une toute petite pause."))
            {
                ChoiceButton endPause = new ChoiceButton
                {
                    text = "Finir la Pause",
                    id = currentBlock.choices.Count + step
                };
                SpawnChoices(endPause);
            }
            else
            {
                skipCircle.gameObject.SetActive(true); // activate the skip ui timer
                duration = WordCount.GetWordCount(emmaSpeech); // will be obsolete and calculate from the audio file
                skipCircle.GetComponent<SkipDialogueCircle>().time = duration; // 2f is to make it longer before changing to the audio file;
                StartCoroutine(SkipDialogue(duration));

            }
        }

        //spawn debug button to go back
        //if (currentStage == TypeOfStage.Introduction || currentStage == TypeOfStage.Pause)
        //{
            ChoiceButton debugBack = new ChoiceButton
            {
                id = -1,
                text = "DEV : retour"
            };
            SpawnDebugButton(debugBack);
        //}
    }

    void OnClick(int id)
    {
        skip++;
        skipCircle.gameObject.SetActive(false);

        //destroy all button to avoid spawning duplicates

        foreach (GameObject button in listOfInstantiedButtons)
            Destroy(button);

        // -1 as id means a debug button as been click to go back
        // if emmaindex is 1 or 2 emma is displaying an answer, I can make it to go back to the specific answer so we go back twice to the question
        // we can only go back to the common text

        if (id == -1)
        {
            id = 0;
            if (step > 0)
                step--;
            else if (blockIndex > 0)
            {
                blockIndex--;
                NextLine();
            } 
            else if (blockIndex == 0 && sequenceIndex > 0)
            {
                sequenceIndex--;
                LoadJSON();
            }
        }
        emmaSpeech = currentBlock.choices[id].res[0].text;
        Debug.LogWarning("emma speech is : " + emmaSpeech);

                
        // check for score
        //if there is a score in the block 
        if (stage.block[blockIndex].choices[id].score > 0)
        {
            score++;
        }

        step = 0;
        SpawnButtons();
        blockIndex++;
        NextLine();

        //ProgressionSlider.Current.UpdateValue();
    }

    IEnumerator SkipDialogue(float duration)
    {
        //when emma is speaking, automatically advance to the question without any button
        float normalizedTime = 0;
        while (normalizedTime <= 1f)
        {
            normalizedTime += Time.deltaTime / (duration);
            yield return null;
        }
        blockIndex++;
        NextLine();
    }

    public int GetNumberOfBlock()
    {
        return stage.block.Length;
    }

    public void SpawnChoices(ChoiceButton newChoice)
    {
        GameObject newButtonChoice = Instantiate(buttonPREF, choicesPanel.transform);
        listOfInstantiedButtons.Add(newButtonChoice);
        newButtonChoice.GetComponentInChildren<Text>().text = newChoice.text;
        newButtonChoice.GetComponent<Button>().onClick.AddListener(delegate { OnClick(newChoice.id); });
    }
    public void SpawnDebugButton(ChoiceButton newChoice)
    {
        GameObject newButtonDebug = Instantiate(debugButtonPREF, choicesPanel.transform);
        listOfInstantiedButtons.Add(newButtonDebug);
        newButtonDebug.GetComponentInChildren<Text>().text = newChoice.text;
        newButtonDebug.GetComponent<Button>().onClick.AddListener(delegate { OnClick(newChoice.id); });

    }
}

