using MainStore;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Launchable : MonoBehaviour
{
    public static StoreMinigame CurrentMinigame;
    private string _configPath;

    public void LoadConfig(string configPath)
    {
        _configPath = configPath;

        string json = File.ReadAllText(_configPath);
        CurrentMinigame = JsonUtility.FromJson<StoreMinigame>(json);

        Debug.Log(("Load config", _configPath, CurrentMinigame, CurrentMinigame.Best));
    }

    private void OnDestroy()
    {
        string json = JsonUtility.ToJson(CurrentMinigame, prettyPrint: true);
        Debug.Log(("Save", _configPath, CurrentMinigame.Best));
        File.WriteAllText(_configPath, json);
    }
}
