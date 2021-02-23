using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Handle the alpha of a whole panel allowing FadeInFadeOut transitions


[RequireComponent(typeof(CanvasGroup))]
public class CanvasFade : MonoBehaviour
{
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
    public void Show()
    {
        gameObject.SetActive(true);
        //when canvas becomes active
        desiredAlpha = 1;
        if (canvas != null)
        {
            canvas.interactable = true;
            canvas.blocksRaycasts = true;
        }
    }
    private void Update()
    {
        currentAlpha = Mathf.MoveTowards(currentAlpha, desiredAlpha,Time.deltaTime/duration);
        canvas.alpha = currentAlpha;
        if (canvas.alpha <= 0)
            gameObject.SetActive(false);
    }

    public void Hide()
    {
        desiredAlpha = 0;
        // when exiting playmode I get a null reference, reason unknown
        if (canvas != null)
        {
            canvas.interactable = false;
            canvas.blocksRaycasts = false;
        }
    }

}
