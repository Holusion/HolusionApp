using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ RequireComponent(typeof(Text)) ]
public class ChangeTextToStudentName : MonoBehaviour
{
    void OnEnable()
    {
        if (GameManager.canLoad)
        GetComponent<Text>().text = GameManager.currentStudentSet.name;
    }

}
