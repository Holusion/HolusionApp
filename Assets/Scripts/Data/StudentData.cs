using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StudentData
{
    public List<Student> listOfStudent = new List<Student>();

    public StudentData(List<Student> list)
    {
        listOfStudent = list;
    }
}
