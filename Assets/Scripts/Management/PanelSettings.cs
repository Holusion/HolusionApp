using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class PanelSettings : MonoBehaviour
{
    public string title;
    public string subtitle;

    // for manual choice, leave null if exercice
    public ChoiceButton[] choices;
   // public GameObject menuPanel;
    public bool displayRetryButton;

    protected GameObject choicesPanel;
    //protected GameObject choicePanelGrid;
    private GameObject buttonPREF;
    private GameObject retryButtonPREF;
    private GameObject debugButtonPREF;

    protected List<GameObject> listOfInstantiedButtons = new List<GameObject>();



    void Awake()
    {
        buttonPREF = Resources.Load<GameObject>("ButtonMenu");
        retryButtonPREF = Resources.Load<GameObject>("ButtonRetry");
        debugButtonPREF = Resources.Load<GameObject>("ButtonDebug");

        choicesPanel = GetComponentInChildren<VerticalLayoutGroup>().gameObject; 

        // spawn the manually inputed buttons
        foreach (ChoiceButton item in choices)
            SpawnChoices(item);

        if (displayRetryButton)
        {
            GameObject retryButton = Instantiate(buttonPREF, choicesPanel.transform);
            //newChoice.GetComponent<Button>().onClick.AddListener(delegate { MenuManager.Actual.ChangeToPanel(item.panel); });
        }
    }
    public void SpawnChoices(ChoiceButton newChoice)
    {
        GameObject newButtonChoice = Instantiate(buttonPREF, choicesPanel.transform);
        listOfInstantiedButtons.Add(newButtonChoice);
        newButtonChoice.GetComponentInChildren<Text>().text = newChoice.text;
        if (newChoice.id > 0)
            newButtonChoice.GetComponent<Button>().onClick.AddListener(delegate { OnClick(newChoice.id); });
        else
            newButtonChoice.GetComponent<Button>().onClick.AddListener(delegate { MenuManager.Actual.ChangeToPanel(newChoice.panel); });
    }

    public void SpawnDebugButton(ChoiceButton newChoice)
    {
        GameObject newButtonDebug = Instantiate(debugButtonPREF, choicesPanel.transform);
        listOfInstantiedButtons.Add(newButtonDebug);
        newButtonDebug.GetComponentInChildren<Text>().text = newChoice.text ;
        newButtonDebug.GetComponent<Button>().onClick.AddListener(delegate { OnClick(newChoice.id); });

    }
    public virtual void OnClick(int id)
    {

    }
    public void OnEnable()
    {
        TitleDisplay.Change.Title = title;
        TitleDisplay.Change.SubTitle = subtitle;
    }
}
