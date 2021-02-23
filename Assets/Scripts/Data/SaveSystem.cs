using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem 
{
    public static void SaveStudent( List<Student> studentList )
    {
        //StudentData data = new StudentData(studentList);

        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/studentList.data";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, studentList);
        stream.Close();
    }

    public static List<Student> LoadData()
    {
        string path = Application.persistentDataPath + "/studentList.data";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            List<Student> loadedList = formatter.Deserialize(stream) as List<Student>;
            stream.Close();
            return loadedList;

        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return new List<Student>();
        }
    }
}
