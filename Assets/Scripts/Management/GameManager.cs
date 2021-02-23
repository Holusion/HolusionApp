using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static Student currentStudentSet;
    //public static string currentStudent;
    public static int scoreParticipation;
    public static int numberOfTotalBlockInSession;
    private TrainingManager[] stages;
    public static bool canLoad;

    public string studentName;

    void Start()
    {
        currentStudentSet = new Student { name = "#None" };
        stages = GetComponentsInChildren<TrainingManager>();
        StartCoroutine(LateStart());

    }
    IEnumerator LateStart()
    {
        yield return new WaitForEndOfFrame();
        canLoad = true;
    }
    public void Quit()
    {
        Application.Quit();
    }
    private void Update()
    {
        studentName = currentStudentSet.name;
    }

}
