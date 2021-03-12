using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.IO;
using System.Linq;

public delegate void OnClickCallBack(int count);

class DropOutStack<T>
{
    private T[] items;
    private int top = 0;
    public DropOutStack(int capacity)
    { 
        items = new T[capacity];
    }
    public int Count {
      get { return this.top; }
    }
    public void Push(T item)
    {
        items[top] = item;
        top = (top + 1) % items.Length;
    }
    public T Pop()
    {
        top = (items.Length + top - 1) % items.Length;
        return items[top];
    }
}


public class TrainingPanel1 : PanelSettings
{
    //panel to show when the session has ended
    public PanelSettings panelWhenEnd;

    public int session=1;
    private int currentStage=0;

    private DropOutStack<Block[]> timeTravel = new DropOutStack<Block[]>(10 /*max undo count*/);
    // text to display
    public Text emmaField;

   // private Slider progressSlider;

    //Emotion list for displaying random emotion like in exercice 1
    private TypeOfEmotion currentEmotion;
    //if random //private HashSet<TypeOfEmotion> emotionList = new HashSet<TypeOfEmotion>();
    public List<TypeOfEmotion> emotionList = new List<TypeOfEmotion>();

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
        Session s = LoadJSON();        //check if there is an exercice to load, if not we've reached the end of the session, save the score and change to profil panel
        if (s.stages.Length <= currentStage)
        {
            TitleDisplay.Change.Title = "";
            TitleDisplay.Change.SubTitle = "";
            //profilScreen.AddProgress();
            //save score to the student profil
            //GameManager.currentStudentSet.scores.Add(new Score { session = this.session, score = this.score});
            GameManager.currentStudentSet.score1 = score;
            GameManager.currentStudentSet.progress++;
            SaveSystem.SaveStudent(StudentsListPanel.listOfStudents);

            ScreenManager.Actual.ChangeToPanel(panelWhenEnd);
            return;
        }
        Stage stage = s.stages[currentStage];
        TitleDisplay.Change.Title = "Session" + session.ToString()+" - "+stage.title;
        TitleDisplay.Change.SubTitle = GameManager.currentStudentSet.name;
        Play(stage.blocks);
    }

    void Play(Block[] blocks){
      if(blocks.Length == 0) return; //END
      Block b = blocks[0];
      ShowText(b.text);
      ShowButtons(b.choices, (int i)=>{
        IEnumerable<Block> next;
        timeTravel.Push(blocks);
        if(i ==-1) {
          timeTravel.Pop(); //un-push last blocks
          next = timeTravel.Pop();
        }else if(i < b.choices.Length){
          next = blocks.Skip(1);
          timeTravel.Push(blocks);
          IEnumerable<Bubble> responses = b.choices[i].res;
          if(responses != null){
            foreach( Bubble r in responses){
              next = next.Prepend(new Block{text=r.text, audio=r.audio});
            }
          }
        }else{
          Debug.LogWarning("SKip because"+i+">"+b.choices.Length);
          //Implement time travel for skip
          next = blocks.Skip(i-b.choices.Length);
        }
        Play(next.ToArray());
      });
      //Play audio
    }
    private void ShowButtons(Choice[] choices, OnClickCallBack cb){
        foreach (GameObject button in listOfInstantiedButtons)
            Destroy(button);
        if(choices != null){
          for (int i = 0; i < choices.Length; i++){
            int _i = i;
            CreateButton(choices[i].btn, delegate { cb(_i);});
          }
        }
        if(0 < timeTravel.Count){
          CreateButton("Back", delegate { cb(-1);});
        }
        CreateButton("Ignorer", delegate { cb(1+choices.Length);});
    }
    private void ShowText(string txt){
      emmaField.text = txt; 
    }
    public void CreateButton(string text, UnityAction cb, bool debug=false)
    {
        GameObject newButtonChoice = Instantiate((debug? debugButtonPREF : buttonPREF), choicesPanel.transform);
        listOfInstantiedButtons.Add(newButtonChoice);
        newButtonChoice.GetComponentInChildren<Text>().text = text;
        newButtonChoice.GetComponent<Button>().onClick.AddListener(cb);
    }

    private Session LoadJSON()
    {
        
        TextAsset textfile = Resources.Load<TextAsset>("JSON/Session" + session);
        if(textfile == null){
          return new Session{
            stages = new [] {new Stage{
              title="Not found", 
              blocks= (new[] {
                new Block{text= "Fichier non trouvé pour la session "+ session}
              })
            }}
          };
        }
        string jsonFile = textfile.text;

        //update the subtitle text
        
        return JsonUtility.FromJson<Session>(jsonFile);
    }

}

