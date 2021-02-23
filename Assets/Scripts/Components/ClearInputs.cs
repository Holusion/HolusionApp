using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ClearInputs : MonoBehaviour
{
    private InputField[] fields;

    void Start()
    {
        fields = GetComponentsInChildren<InputField>();
    }

    public void Clear()
    {
        foreach (InputField field in fields)
        {
            field.text = "";
        }
    }


}
