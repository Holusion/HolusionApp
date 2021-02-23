using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIIntro : MonoBehaviour
{
    private float desiredPos;
    private float startPos;
    private float currentPos;
    void Start()
    {
        desiredPos = transform.position.x;
        print (transform.position.x);
    }

    void OnEnable()
    {
        RectTransform rt = (RectTransform)transform;
        float distance = rt.rect.width * 0.5f;
        startPos = transform.position.x + distance;
        transform.position = new Vector3(startPos, transform.position.y, transform.position.z);
        currentPos = startPos;
        //StartCoroutine(Move());
    }

    // Update is called once per frame
    void Update()
    {
        currentPos =  Mathf.MoveTowards(currentPos, desiredPos, Time.deltaTime*20);
        transform.position = new Vector3(currentPos, transform.position.y, transform.position.z);
    }
    IEnumerator Move()
    {
        while (transform.position.x < desiredPos)
        {
            transform.position = new Vector3(transform.position.x + 0.001f * Time.deltaTime, transform.position.y, transform.position.z);
            yield return null;
        }
    }
}
