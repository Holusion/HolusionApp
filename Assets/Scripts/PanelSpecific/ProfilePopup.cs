using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class ProfilePopup : MonoBehaviour
{
    public StudentsListPanel builder;
    private string name;
    private string age;
    private Gender gender;
    private string comment;

    public InputField fieldName;
    public InputField fieldAge;
    public InputField fieldComment;
    public Dropdown fieldGender;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void SetName(string s)
    {
        name = s;
    }
    public void SetAge(string a)
    {
        age = a;
    }
    public void SetGender(Dropdown d)
    {
        gender = (Gender)d.value;
    }
    public void SetComment (string c)
    {
        comment = c;
    }

    public void AddStudent()
    {
        Score newscore = new Score();
        Student newIdentification = new Student
        {
            name = StringManager.ToTitleCase(name),
            age = this.age,
            gender = this.gender,
            comments = comment,
            progress = 0,
           // scores.Add(newscore)
        };



        builder.AddStudentToList(newIdentification);
    }

    // when Modify popup appear, write the inputfield with the current informations
    public void ModifyStudent()
    {
        Student studentToModify = GameManager.currentStudentSet;
        fieldName.text = studentToModify.name;
        fieldAge.text = studentToModify.age;
        fieldGender.value = (int)studentToModify.gender;
        fieldComment.text = studentToModify.comments;
    }

    public void UpdateModifiedStudent()
    {
        Student studentToModify = GameManager.currentStudentSet;
        studentToModify.name = StringManager.ToTitleCase(name);
        studentToModify.age = age;
        studentToModify.comments = comment;
        studentToModify.gender = gender;

        // save the list
        builder.ModifyStudentInList();
    }


}
