using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleDisplay : MonoBehaviour
{
    // Display the Title according to the PanelSettings

    public Text Titlefield;
    public Text SubTitlefield;
    static private string title;
    static private string subtitle;

    // to access the script everywhere
    static public TitleDisplay Change;

    TitleDisplay()
    {
        Change = this;
    }

    public string Title
    {
        set
        {
            Titlefield.text = value;
        }
    }
    public string SubTitle
    {
        set
        {
            SubTitlefield.text = value;
        }
    }
}
