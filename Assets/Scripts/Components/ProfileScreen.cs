using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileScreen : MonoBehaviour
{

    private Student currentStudent;

    //fieldtext to updates
    public Text name;
    public Text age;
    public Text comment;
    public Text gender;

    public Slider progress;
    public Slider progress1;

    public Button session1Butt;
    public Button session2Butt;

    public Text debug;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        DisplayProfile();
    }
    public void DisplayProfile()
    {
        if (GameManager.currentStudentSet != null)
        {
            currentStudent = GameManager.currentStudentSet;
            name.text = currentStudent.name;
            age.text = "âge : " +currentStudent.age;
            comment.text = "commentaires : " + currentStudent.comments;
            gender.text = currentStudent.gender.ToString();

            foreach (Score s in currentStudent.scores)
            {
                debug.text += s.session.ToString() + " - " + s.score + " / ";
            }

            //print("score : " + currentStudent.scores[0].score.ToString());


            if (currentStudent.progress > 0)
            {
                progress.value = 1;
                session1Butt.interactable = false;
                session2Butt.interactable = true;
                session1Butt.GetComponentInChildren<Text>().text = "session 1 score: " + currentStudent.score1.ToString();
                progress1.value = currentStudent.score1;
                print("score " + currentStudent.scores.Count);

            }
            else
            {
                progress.value = 0;
                session1Butt.interactable = true;
                session2Butt.interactable = false;
            }


        }
    }


}
