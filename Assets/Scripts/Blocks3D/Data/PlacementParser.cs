using Blocks3D;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlacementParser : MonoBehaviour
{
    private static string PlacementsPath => Path.Combine(Application.streamingAssetsPath, "Blocks3D", "Placements");

    [SerializeField] BlocksPlacement blocksPlacement;

    public void Load()
    {
        blocksPlacement.Configs.Clear();

        DirectoryInfo directory = new DirectoryInfo(PlacementsPath);

        if (!directory.Exists)
        {
            Debug.Log("directory " + PlacementsPath + " does not exists!");
            return;
        }

        foreach (FileInfo file in directory.GetFiles())
        {
            if (file.Extension == ".json")
            {
                string json = File.ReadAllText(file.FullName);
                blocksPlacement.Configs.Add(JsonUtility.FromJson<PlacementConfig>(json));
            }
        }
    }

    public void Save()
    {
        DirectoryInfo directory = new DirectoryInfo(PlacementsPath);

        if (!directory.Exists)
        {
            Debug.Log("directory " + PlacementsPath + " does not exists!");
            return;
        }

        foreach (PlacementConfig p in blocksPlacement.Configs)
        {
            string json = JsonUtility.ToJson(p, prettyPrint: true);
            File.WriteAllText(Path.Combine(PlacementsPath, p.ToString() + ".json"), json);
        }
    }

    private void Awake()
    {
        Load();
    }
}
