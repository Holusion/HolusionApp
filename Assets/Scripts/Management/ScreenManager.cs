using UnityEngine;

[ExecuteAlways]
public class ScreenManager : MonoBehaviour
{
    public static ScreenManager Actual;
    private PanelSettings[] listOfPanels;
    public PanelSettings firstPanel;

    void Start()
    {
        Actual = this;

        // get all panels
        listOfPanels = GetComponentsInChildren<PanelSettings>();
        //turn off every panels
        HideEverything();
        ChangeToPanel(firstPanel);
    }

    // change to the next panel in array
    // probably obselete by now
    /*public void ChangeToNext()
    {
        int i = 0;
        for (i = 0; i < listOfPanels.Length; i++)
        {
            if (listOfPanels[i].gameObject.activeSelf)
                break;
        }
        UpdatePanel(listOfPanels[i + 1]);
    }*/

    public void ChangeToPanel (PanelSettings newPanel)
    {
        UpdatePanel(newPanel);
    }
    public void ChangeToIndex(int index)
    {
        UpdatePanel(listOfPanels[index]);
    }

    private void UpdatePanel(PanelSettings newPanel)
    {
        HideEverything();
        newPanel.Activate();
        //newPanel.GetComponent<CanvasFade>().Show();
    }

    private void HideEverything()
    {
        foreach (PanelSettings panel in listOfPanels)
            panel.Deactivate();
            //panel.gameObject.GetComponent<CanvasFade>().Hide();
    }
}
