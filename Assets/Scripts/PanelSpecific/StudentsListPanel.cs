using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class StudentsListPanel : PanelSettings
{
    // the button that will be spawned for each student
    public GameObject buttonPrefab;
    // when you click on one button you will get to this panel
    public PanelSettings nextPanel;
    // the gameobject where buttons are spawned
    private GameObject scrollContent;

    [SerializeField]
    static public List<Student> listOfStudents = new List<Student>();
    private List<GameObject> listOfButtons = new List<GameObject>();


    private void Start()
    {
        // get the content gameobject where buttons for the student will be spawned
        scrollContent = GetComponentInChildren<ScrollRect>().content.gameObject;
        LoadSavedList();
    }

    public override void Activate()
    {
        base.Activate();
        UpdateList();
    }

    void UpdateList()
    {
        // sort the list
        var sortedList = listOfStudents.OrderBy(foo => foo.name).ToList();

        // delete all displayed button just in case, to avoid duplicates
        foreach (GameObject button in listOfButtons)
            Destroy(button);

        // spawn the button
        foreach (Student student in sortedList)
        {
            GameObject newButton = Instantiate(buttonPrefab, scrollContent.transform);
            newButton.name = student.name;
            listOfButtons.Add(newButton);

            newButton.GetComponentInChildren<Text>().text = student.name;

            // add the listener for when the button is clicks
            newButton.GetComponent<Button>().onClick.AddListener(delegate { GameManager.currentStudentSet = student; });
            newButton.GetComponent<Button>().onClick.AddListener(delegate { ScreenManager.Actual.ChangeToPanel(nextPanel); });
        }

        

    }

    void UpdateStudentIdentification(Student ident)
    {
        GameManager.currentStudentSet = ident;
    }

    public void AddStudentToList(Student newIdentity)
    {
        listOfStudents.Add(newIdentity);
        SaveSystem.SaveStudent(listOfStudents);
        UpdateList();
    }
    public void RemoveStudentToList()
    {
        listOfStudents.Remove(GameManager.currentStudentSet);
        SaveSystem.SaveStudent(listOfStudents);
        UpdateList();
    }
    public void ModifyStudentInList()
    {
        SaveSystem.SaveStudent(listOfStudents);
    }
    void LoadSavedList()
    {
        listOfStudents = SaveSystem.LoadData();
    }

    // search function
    public void z_FilterButton(string input)
    {
        for (int i = 0; i < listOfButtons.Count; ++i)
        {
            if (listOfButtons[i] != null) 
                listOfButtons[i].SetActive(listOfButtons[i].name.ToLower().IndexOf(input.ToLower().Trim()) >= 0);
        }
    }



}
