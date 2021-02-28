using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// OBSOLETE

public class RadialStatInfos : MonoBehaviour
{
    public Image bar;
    public Text title;
    public Text percentage;

    public float startDelay;

    public float endNumber;

    private void Start()
    {
        percentage.text = endNumber + " %";
        bar.fillAmount = 0;

    }
    void Update()
    {
        while (startDelay > 0)
            startDelay -= Time.deltaTime;
        bar.fillAmount = Mathf.MoveTowards(bar.fillAmount, endNumber*0.01f, 0.25f*Time.deltaTime);

    }
}
