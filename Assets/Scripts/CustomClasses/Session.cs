using System;
using System.Collections.Generic;
using UnityEngine;



public enum TypeOfEmotion { joie, tristesse, dégoût, surprise, colère, peur }

public enum TypeOfSession { Session1, Session2, Session3, Session4, Session5, Session6, Session7 };

public enum TypeOfStage { Introduction, Pause, Conclusion, Exercice1, Exercice2, Exercice3, Exercice4, Exercice5, Exercice6 };

[Serializable]
public class Stage
{
    public Block[] block;
}
[Serializable]
public class Block
{
    public string text;
    public string audio;
    public Choices[] choices;
    //public List<string> emma = new List<string>();
    //public List<string> choix = new List<string>();
    //public List<int> score = new List<int>();
}
[Serializable]
public class Choices
{
    public string btn;
    public int score;
    public Emma[] res;
}
[Serializable]
public class Emma
{
    public string text;
    public string audio;
}
