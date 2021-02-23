using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkipDialogueCircle : MonoBehaviour
{
    public Image circle;
    public float startDelay;
    public float endNumber;
    public float time;

    void OnEnable()
    {
        StopAllCoroutines();
        circle.fillAmount = 0;
        endNumber = 1;
        StartCoroutine(Circle());
    }

    void Update()
    {
        //while (startDelay > 0)
            //startDelay -= Time.deltaTime;
        //circle.fillAmount = Mathf.MoveTowards(circle.fillAmount, endNumber, time/ Time.deltaTime);
        if (circle.fillAmount == 1)
        {
            gameObject.SetActive(false);
        }

    }
    IEnumerator Circle()
    {
        //float duration = emmaField.text.Split('/').Length *0.2f;
        //float duration = 2f;
        float normalizedTime = 0;
        while (normalizedTime <= 1f)
        {
            circle.fillAmount += Time.deltaTime / (time);
            yield return null;
        }
       // OnClick(currentBlock.choix.Count + step);
    }
}
