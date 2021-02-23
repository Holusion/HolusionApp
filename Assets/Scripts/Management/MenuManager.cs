using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.Collections;

[ExecuteAlways]
public class MenuManager : MonoBehaviour
{
    public static MenuManager Actual;
    private PanelSettings[] listOfPanels;

    /*public GameObject[] listOfPanels;
    [SerializeField] static public List<string> panelNamesStatic = new List<string>();
    [SerializeField] private List<string> panelNames = new List<string>();*/


    /*
#if UNITY_EDITOR
    void Update()
    {
        panelNames.Clear();
        foreach (GameObject ob in listOfPanels)
        {
            //remove whitespace and punctuation
            string newName = Regex.Replace(ob.name, @"\s+", "");
            newName = Regex.Replace(newName, @"[^0-9a-zA-Z]+", "");
            panelNames.Add(newName);
        }
        panelNamesStatic = panelNames;
    }
    static public string[] GetPanelsNames()
    {
        return panelNamesStatic.ToArray();
    }
#endif
    */
    void Awake()
    {
        listOfPanels = GetComponentsInChildren<PanelSettings>();
        Actual = this;
        HideEverything();
        // When using UnityRemote, a delay is necessary overwise the UI animation will start before the screen is changed to the device causing offsets
        StartCoroutine(Delay());
    }

    // change to the next panel in array
    public void ChangeToNext()
    {
        int i = 0;
        for (i = 0; i < listOfPanels.Length; i++)
        {
            if (listOfPanels[i].gameObject.activeSelf)
                break;
        }
        UpdatePanel(listOfPanels[i + 1].gameObject);
    }

    public void ChangeToPanel (PanelSettings newPanel)
    {
        UpdatePanel(newPanel.gameObject);
    }



    public void ChangeToIndex(int index)
    {
        UpdatePanel(listOfPanels[index].gameObject);
    }

    /*public void ChangeToEnum(EnumScreen enumscreen)
    {
        if (listOfPanels != null)
        {

            if (enumscreen == EnumScreen.Auto)
                ChangeToNext();
            else
                UpdatePanel(listOfPanels[(int)enumscreen-1].gameObject);
        }

    }*/

    private void UpdatePanel(GameObject newPanel)
    {
        HideEverything();
        newPanel.GetComponent<CanvasFade>().Show();
    }

    public void HideEverything()
    {
        foreach (PanelSettings panel in listOfPanels)
            panel.gameObject.GetComponent<CanvasFade>().Hide();
    }
    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.5f);
        ChangeToIndex(0);
    }
}
