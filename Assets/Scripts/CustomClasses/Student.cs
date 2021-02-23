using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Gender { masculin, feminin}

[System.Serializable]
public class Student
{
    public string name;
    public string age;
    public Gender gender;
    public string comments;
    public int progress;
    public int score1;
    public List<Score> scores = new List<Score>();
    
}
[System.Serializable]
public class Score
{
    public TypeOfSession session;
    //public TypeOfStage stage;
    public int score;
}
