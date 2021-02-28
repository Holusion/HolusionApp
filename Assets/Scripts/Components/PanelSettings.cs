using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class PanelSettings : MonoBehaviour
{
    public string title;
    public string subtitle;

    protected GameObject choicesPanel;



    public float duration = 0.5f;

    private CanvasGroup canvas;
    private float desiredAlpha;
    private float currentAlpha;

    void Awake()
    {


        //get the Canvasgroup and setalpha to 0
        canvas = GetComponent<CanvasGroup>();
        canvas.alpha = 0;
        currentAlpha = canvas.alpha;
        if (duration == 0)
            duration = 0.01f;
    }

    // When the panel is turned on
    public virtual void Activate()
    {
        TitleDisplay.Change.Title = title;
        TitleDisplay.Change.SubTitle = subtitle;

        desiredAlpha = 1;
        if (canvas != null)
        {
            canvas.interactable = true;
            canvas.blocksRaycasts = true;
        }
    }

    //when the panel is turned off
    public virtual void Deactivate()
    {
        desiredAlpha = 0;
        // when exiting playmode I get a null reference, reason unknown
        if (canvas != null)
        {
            canvas.interactable = false;
            canvas.blocksRaycasts = false;
        }
    }

    //make the fade happen
    private void Update()
    {
        if (currentAlpha != desiredAlpha)
        {
            currentAlpha = Mathf.MoveTowards(currentAlpha, desiredAlpha, Time.deltaTime / duration);
            canvas.alpha = currentAlpha;
        }
    }
}
