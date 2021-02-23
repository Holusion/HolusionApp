using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ChoiceButton
{
    public int id;

    // the name displayed in the button
    public string text;

    //the panel to show when clicked
    public PanelSettings panel;
}
