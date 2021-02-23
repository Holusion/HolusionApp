using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class TrainingManager : PanelSettings
{
    public PanelSettings panelWhenEnd;

    public TypeOfSession session;
    public TypeOfStage[] sequence;
    private int sequenceIndex;
    private TypeOfStage currentStage;

    private string jsonFile;
   // private JsonUtility data;
    private Stage stage;

    private string emmaSpeech;

    //current block text
    private Block currentBlock;
    private int blockIndex = 0;
    private int emmaIndex = 0;
    private int step;
    private int scoreId;
    // text to display
    public Text emmaField;

   // private Slider progressSlider;

    //Emotion list for displaying random emotion like in exercice 1
    private TypeOfEmotion currentEmotion;
    private HashSet<TypeOfEmotion> emotionList = new HashSet<TypeOfEmotion>();

    // when an exercice needs to be reload
    private int cycle;
    public int maxCycle;

    //time remaining with the autoskip
    private SkipDialogueCircle skipCircle;
    private float duration;

    // score for each exercice
    protected int score;

    // for DEBUG ONLY
    public GameObject prefabButtonDebug;
    private int skip;

    public ProfileScreen profilScreen;

    //orientation
    private DeviceOrientation currentOrientation;

    private void Start()
    {
        skipCircle = GetComponentInChildren<SkipDialogueCircle>();
        skipCircle.gameObject.SetActive(false);

        currentOrientation = DeviceOrientation.Portrait;
    }

    private void OnEnable()
    {
        base.OnEnable();
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
            print("saved score is " + score);
            SaveSystem.SaveStudent(StudentsListManager.listOfStudents);
            MenuManager.Actual.ChangeToPanel(panelWhenEnd);
            return;
        }

        ResetIndexes();
        // load the jsonfile as a long string
        //Debug.Log("Path is " + "JSON/" + currentStage.ToString());
        TextAsset textfile = Resources.Load<TextAsset>("JSON/" + session + "/" + currentStage.ToString());
        jsonFile = textfile.text;


        if (currentStage == TypeOfStage.Exercice1)
            maxCycle = 0;
        else
            maxCycle = 6;
        if (currentStage == TypeOfStage.Exercice3)
            maxCycle = 99;

        stage = JsonUtility.FromJson<Stage>(jsonFile);

        //update the subtitle text
        if (GameManager.canLoad)
        {
        TitleDisplay.Change.Title = "Session 1 - " + currentStage.ToString();
        TitleDisplay.Change.SubTitle = GameManager.currentStudentSet.name;

        }

        
        //GameManager.currentStudentSet.scores.Add(0);
        //Debug.Log("adding a new score entry, total : " + GameManager.currentStudentSet.scores.Count);


        if (GameManager.canLoad)
            Load();
    }

    private void Load()
    {
        // reset the emotion list 
        ResetEmotionList();

        // get the number of emma lines
        int emmaLines = 0;
        int choicesLines = 0;
        foreach (Block bl in stage.block)
        {
            foreach (string s in bl.emma)
            {
                emmaLines++;
            }
            foreach (string s in bl.choix)
            {
                choicesLines++;
            }
            choicesLines -= 1;
        }
        emmaLines -= 1 + choicesLines;
       // ProgressionSlider.Current.UpdateMaxValue(emmaLines);

        DisplayLine();
    }

    public void NextLine()
    {
        /*if (blockIndex == stage.block.Length)
        {
            sequenceIndex++;
            LoadJSON();
        }
        else*/
            DisplayLine();
    }


    public void DisplayLine()
    {
        //foreach (Button butt in GetComponentsInChildren<Button>())
            //Destroy(butt);




        StopAllCoroutines(); // stop the autonext

        currentBlock = stage.block[blockIndex];

        // Display the text from Emma
        emmaSpeech = "#error";
        emmaSpeech = currentBlock.emma[emmaIndex];
        Debug.LogWarning("emma speech is : " + emmaSpeech);
        // line checker
        switch (emmaSpeech)
        {
            case "END":
                emmaSpeech = "END you should not see this";
                sequenceIndex++;
                LoadJSON();
                return;
                //break;

            case "RELAUNCH":
                if (emotionList.Count == 0)
                {
                    ResetEmotionList();
                    cycle++;
                }
                if (currentStage != TypeOfStage.Exercice1)
                    cycle++;
                if (cycle > maxCycle)
                {
                    sequenceIndex++;
                    LoadJSON();
                    return;
                   // MenuManager.Actual.ChangeToNext();
                }
                step = 0;
                emmaIndex = 0;
                NextLine();
                return;

            case "SKIPIFDONE":
                if (emotionList.Count == 0 && cycle == maxCycle)
                {
                    sequenceIndex++;
                    LoadJSON();
                    //MenuManager.Actual.ChangeToNext();
                }
                else
                {
                    emmaIndex++;
                    step++;
                    DisplayLine();
                }
                return;
        }
/*
//--------------- IF END OF STAGE -----------------------------------------------------------------------
        if (emmaSpeech == "END")
        {
            emmaSpeech = "";
            // OBSOLETE MenuManager.Actual.ChangeToNext();
            sequenceIndex++;
            LoadJSON();

        }
//--------------- IF END OF STAGE FOR CYCLIC EXERCICES-----------------------------------------------------------------------
        if (emmaSpeech == "RELAUNCH")
        {
            if (emotionList.Count == 0)
            {
                ResetEmotionList();
                cycle++;
            }
            if (currentStage != TypeOfStage.Exercice1)
                cycle++;
            if (cycle > maxCycle)
                MenuManager.Actual.ChangeToNext();
            step = 0;
            emmaIndex = 0;
            
        }
        if (emmaSpeech == "SKIPIFDONE")
        {
            if (emotionList.Count == 0 && cycle == maxCycle)
                MenuManager.Actual.ChangeToNext();
            else
            {
                emmaIndex++;
                step++;
            }
        }
        emmaSpeech = currentBlock.emma[emmaIndex];*/

//--------------- STRING REPLACEMENTS--------------------------------------------       

        if (emmaSpeech.Contains("$randomEmotion"))
            emmaSpeech = emmaSpeech.Replace("$randomEmotion", NewEmotion());
        if (emmaSpeech.Contains("$currentEmotion"))
            emmaSpeech = emmaSpeech.Replace("$currentEmotion", currentEmotion.ToString());
        if (emmaSpeech.Contains("$studentname"))
            emmaSpeech = emmaSpeech.Replace("$studentname", GameManager.currentStudentSet.name);
        if (emmaSpeech.Contains("$numberEmotion"))
        {
            //score++;
            //emmaSpeech = emmaSpeech.Replace("$numberEmotion", GameManager.currentStudentSet.score1.ToString());
            emmaSpeech = emmaSpeech.Replace("$numberEmotion",score.ToString());
            //NextLine();
            //return;
        }
        if (emmaSpeech.Contains("FORCEEND"))
        {
            print("force end");
            currentBlock.emma[currentBlock.emma.Count-1] = "END";
            emmaSpeech = emmaSpeech.Replace("FORCEEND", "");
        }

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

        int choicesCount = currentBlock.choix.Count;
        // if first line of emma for each block and there is a choice key

        // set the vertical layout as a parent
        if (Input.deviceOrientation == DeviceOrientation.Portrait || Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown)
            //codes for portrait
            choicesPanel = GetComponentInChildren<VerticalLayoutGroup>().gameObject;
        else
            //code for landscape
            choicesPanel = GetComponentInChildren<GridLayoutGroup>().gameObject;

        if (emmaIndex < 1 && choicesCount > 0)
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
                    id = i + 1,
                    text = "F" + (i + 1) + " : " + currentBlock.choix[i]
                };
                //newChoice.id = i+1;
                //newChoice.text = "F" + (i+1) + " : " + currentBlock.choix[i];
                SpawnChoices(newChoice);
            }
        }
        // if it's not a choice, it's a sequence, emma is talking, no button to spawn expect the debug ones
        else
        {
            step++;

            // spawn debug button to skip
            ChoiceButton debugSkip = new ChoiceButton
            {
                id = currentBlock.choix.Count + step,
                text = "DEV : Ignorer"
            };
            SpawnDebugButton(debugSkip);

            if (emmaSpeech.Contains("On fait une toute petite pause."))
            {
                ChoiceButton endPause = new ChoiceButton
                {
                    text = "Finir la Pause",
                    id = currentBlock.choix.Count + step
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
        if (currentStage == TypeOfStage.Introduction || currentStage == TypeOfStage.Pause)
        {
            ChoiceButton debugBack = new ChoiceButton
            {
                id = -1,
                text = "DEV : retour"
            };
            SpawnDebugButton(debugBack);

        }
    }

    public override void OnClick(int id)
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
            if (emmaIndex > 0)
                emmaIndex = 0;
            else if (blockIndex > 0)
                blockIndex--;
            else if (blockIndex == 0 && sequenceIndex > 0)
            {
                sequenceIndex--;
                LoadJSON();
            }
        }


        emmaIndex = id;
        // check for score
        //if there is a score in the block 
        if (stage.block[blockIndex].score.Count > 0)
        {
            for (int i = 0; i < stage.block[blockIndex].score.Count; i++)
            {
                //if score id match answer id
                if (id == stage.block[blockIndex].score[i])
                {
                    //update the last entry
                    //GameManager.currentStudentSet.scores

                    //GameManager.currentStudentSet.score1 ++;
                    //if (currentStage == TypeOfStage.Exercice3)
                        score++;
                }
                    
            }
        }



        // if you get to the last emma line
        if (emmaIndex > currentBlock.emma.Count-1)
        {

            step = 0;
            emmaIndex = 0;
            blockIndex++;
        }
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
        OnClick(currentBlock.choix.Count + step);
    }


    // display a random new emotion
    string NewEmotion()
    {
        // get a random entry from the emotionList and remove it
        TypeOfEmotion[] EmotionListasArray = emotionList.ToArray();
        TypeOfEmotion randomEmotion = EmotionListasArray[Random.Range(0, EmotionListasArray.Length)];

        currentEmotion = randomEmotion;
        emotionList.Remove(randomEmotion);
        return currentEmotion.ToString();
    }
    void ResetEmotionList()
    {
        // add every emotions to a list
        emotionList.Clear();
        foreach (TypeOfEmotion emotion in (TypeOfEmotion[])System.Enum.GetValues(typeof(TypeOfEmotion)))
            emotionList.Add(emotion);
    }

    public int GetNumberOfBlock()
    {
        return stage.block.Length;
    }

    private void Update()
    {
        /*if (currentOrientation != Input.deviceOrientation)
        {
            SpawnButtons();
            currentOrientation = Input.deviceOrientation;
            

        }*/


    }
}

