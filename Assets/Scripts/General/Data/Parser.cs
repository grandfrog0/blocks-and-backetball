using Blocks3D;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Parser<T>
{
    private string CurrentFolder { get; set; }
    private string CurrentPath => Path.Combine(Application.streamingAssetsPath, CurrentFolder);

    public List<string> SavePaths = new();
    public List<T> Values;

    public Parser(string currentFolder, List<T> values)
    {
        CurrentFolder = currentFolder;
        Values = values;
    }

    public void Load()
    {
        Values.Clear();
        SavePaths.Clear();

        DirectoryInfo directory = new DirectoryInfo(CurrentPath);

        if (!directory.Exists)
        {
            Debug.Log("directory " + CurrentPath + " does not exists!");
            return;
        }

        foreach (FileInfo file in directory.GetFiles())
        {
            if (file.Extension == ".json")
            {
                string json = File.ReadAllText(file.FullName);
                Values.Add(JsonUtility.FromJson<T>(json));
                SavePaths.Add(file.FullName);
            }
        }
    }


    public void Save()
    {
        DirectoryInfo directory = new DirectoryInfo(CurrentPath);

        if (!directory.Exists)
        {
            Debug.Log("directory " + CurrentPath + " does not exists!");
            return;
        }

        foreach (T t in Values)
        {
            string json = JsonUtility.ToJson(t, prettyPrint: true);
            File.WriteAllText(Path.Combine(CurrentPath, t.ToString() + ".json"), json);
        }
    }
}
