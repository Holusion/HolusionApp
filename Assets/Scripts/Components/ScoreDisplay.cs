using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{

    private Text field;

    private void Start()
    {
        field = GetComponent<Text>();
    }
    // Update is called once per frame
    void Update()
    {
        field.text = "";
        if (GameManager.currentStudentSet == null)
            field.text = "null";
        else if (GameManager.currentStudentSet.scores == null)
            field.text = "score null";
        else if (GameManager.currentStudentSet.scores.Count == 0)
            field.text = "studentSet but no score in list";
        else
            for (int i = 0; i < GameManager.currentStudentSet.scores.Count; i++)
            {
                field.text += GameManager.currentStudentSet.scores[i] + " ";

            }
    }
}
