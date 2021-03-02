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
    public List<string> emma = new List<string>();
    public List<string> choix = new List<string>();
    public List<int> score = new List<int>();
}


