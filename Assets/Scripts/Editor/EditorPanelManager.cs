using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditorPanelManager : EditorWindow
{
    PanelSettings selected;
    PanelSettings[] panels;


    [MenuItem("Window/ScreenSelection")]
    public static void ShowWindow()
    {
       // GetWindow<EditorPanelManager>("Screen Selection");
    }


    private void OnGUI()
    {
        //panels = FindObjectsOfType<PanelSettings>();
       // selected = EditorGUILayout.Popup();
    }

    private EditorPanelManager()
    {
        foreach (PanelSettings panel in panels)
            panel.gameObject.SetActive(false);

        selected.gameObject.SetActive(true);
    }
}
