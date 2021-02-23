using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressionSlider : MonoBehaviour
{
    static public ProgressionSlider Current;
    private int progress;
    private int addMaxValue;
    private Slider slider;

    ProgressionSlider()
    {
        Current = this;
    }


    void Start()
    {
        slider = GetComponent<Slider>();
        slider.maxValue = 0 ;
    }

    public void UpdateValue()
    {
        slider.value++;
    }
    public void UpdateMaxValue(int i)
    {
        slider.maxValue += i;
    }


}
