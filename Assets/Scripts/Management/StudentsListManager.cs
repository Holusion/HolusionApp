using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class StudentsListManager : MonoBehaviour
{
    // the button that will be spawned for each student
    public GameObject buttonPrefab;
    // when you click on one button you will get to this panel
    public PanelSettings nextPanel;
    // the gameobject where buttons are spawned
    private GameObject scrollContent;
    // test tmp

    [SerializeField]
    static public List<Student> listOfStudents = new List<Student>();
    private List<GameObject> listOfButtons = new List<GameObject>();

    // test tmp
    private void Start()
    {
        // get the content gameobject where buttons for the student will be spawned
        scrollContent = GetComponentInChildren<ScrollRect>().content.gameObject;
        LoadSavedList();
        //listOfStudents.Sort();
    }

    private void OnEnable()
    {
        BuildAndDisplayList();
    }

    void BuildAndDisplayList()
    {
        // sort the list
        var sortedList = listOfStudents.OrderBy(foo => foo.name).ToList();



        // delete all displayed button just in case, to avoid duplicates
        foreach (GameObject button in listOfButtons)
            Destroy(button);

        // spawn the button back
        foreach (Student s in sortedList)
        {
            GameObject newStudent = Instantiate(buttonPrefab, scrollContent.transform);
            newStudent.name = s.name;
            listOfButtons.Add(newStudent);

            newStudent.GetComponentInChildren<Text>().text = s.name;

            //manually add a listener : when anybutton is click change the panel
            //TODO pass students info
            //newStudent.GetComponent<Button>().onClick.AddListener(delegate { UpdateStudentName(s.name); });
            newStudent.GetComponent<Button>().onClick.AddListener(delegate { GameManager.currentStudentSet = s; });
            newStudent.GetComponent<Button>().onClick.AddListener(delegate { MenuManager.Actual.ChangeToPanel(nextPanel); });
        }

    }

    void UpdateStudentName(string newName)
    {
        //GameManager.currentStudent = newName;
    }
    void UpdateStudentIdentification(Student ident)
    {
        GameManager.currentStudentSet = ident;
    }

    public void AddStudentToList(Student newIdentity)
    {
        listOfStudents.Add(newIdentity);
        print("new name is " + newIdentity.name);
        SaveSystem.SaveStudent(listOfStudents);
        BuildAndDisplayList();
    }
    public void RemoveStudentToList()
    {
        listOfStudents.Remove(GameManager.currentStudentSet);
        SaveSystem.SaveStudent(listOfStudents);
        BuildAndDisplayList();
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
