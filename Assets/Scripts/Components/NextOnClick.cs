using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NextOnClick : MonoBehaviour
{

    /*public EnumScreen ToDoOnClick = EnumScreen.Auto;
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        MenuManager.Actual.ChangeToEnum(ToDoOnClick);
    }*/

    public PanelSettings nextPanel;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        ScreenManager.Actual.ChangeToPanel(nextPanel);
    }



}
