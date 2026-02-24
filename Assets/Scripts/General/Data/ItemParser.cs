using Blocks3D;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ItemParser<T>
{
    private string CurrentFile { get; set; }
    private string CurrentPath => Path.Combine(Application.streamingAssetsPath, CurrentFile);

    public T Value;

    public ItemParser(string currentFile, T value)
    {
        CurrentFile = currentFile;
        Value = value;
    }

    public void Load()
    {
        FileInfo file = new FileInfo(CurrentPath);

        if (!file.Exists)
        {
            Debug.Log("file " + CurrentPath + " does not exists!");
            return;
        }

        string json = File.ReadAllText(file.FullName);
        Value = JsonUtility.FromJson<T>(json);
    }


    public void Save()
    {
        FileInfo file = new FileInfo(CurrentPath);

        string json = JsonUtility.ToJson(file, prettyPrint: true);
        File.WriteAllText(CurrentPath, json);
    }
}
