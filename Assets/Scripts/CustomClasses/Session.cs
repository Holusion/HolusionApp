using System;
using System.Collections.Generic;
using UnityEngine;



public enum TypeOfEmotion { joie, tristesse, dégoût, surprise, colère, peur }

public enum TypeOfSession { Session1, Session2, Session3, Session4, Session5, Session6, Session7 };

public enum TypeOfStage { Introduction, Pause, Conclusion, Exercice1, Exercice2, Exercice3, Exercice4, Exercice5, Exercice6 };

[Serializable]
public struct Session {
  public Stage[] stages;
}

[Serializable]
public class Stage
{
    public string title;
    public Block[] blocks;
}

[Serializable]
public class Bubble
{
    public string text;
    public string audio;
    public override string ToString()
    {
        return base.ToString() + ": ["+audio+"]" + text;
    }
}

[Serializable]
public class Block : Bubble
{
    public Choice[] choices = new Choice[0];
}

[Serializable]
public class Choice
{
    public string btn;
    public int score;
    public Bubble[] res = new Bubble[0];
    public override string ToString()
    {
        return base.ToString() + btn ;
    }
}
