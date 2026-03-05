using MainStore;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class GlobalManager : MonoBehaviour
{
    public static GlobalManager Instance { get; private set; }

    public StoreMinigame CurrentMinigame { get; set; }
    public string CurrentMinigameSavePath => MinigamesSavePaths[Minigames.IndexOf(CurrentMinigame)];
    [SerializeField] StoreData storeData;

    public float Coins
    {
        get => storeData.Coins;
        set => storeData.Coins = value;
    }

    public UnityEvent OnMinigamesRefresh = new();
    public List<StoreMinigame> Minigames => storeData.Minigames;
    public List<string> MinigamesSavePaths { get; } = new();

    private string MinigamesPath => Path.Combine("General", "Minigames");
    private string StoreDataPath => Path.Combine("General", "StoreData.json");
    private Parser<StoreMinigame> _minigamesParser;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        _minigamesParser = new Parser<StoreMinigame>(MinigamesPath, Minigames);
        _minigamesParser.Load();
        OnMinigamesRefresh.Invoke();

        MinigamesSavePaths.AddRange(_minigamesParser.SavePaths);
    }

    public void AddMinigame(string minigamePath)
    {
        try
        {
            string json = File.ReadAllText(minigamePath);
            StoreMinigame minigame = JsonUtility.FromJson<StoreMinigame>(json);

            Minigames.Add(minigame);
            _minigamesParser.Save();

            OnMinigamesRefresh.Invoke();
        }
        catch (Exception e)
        {
            Debug.Log("Exception occured: " + e);
        }
    }

    private void OnApplicationQuit()
    {
        _minigamesParser.Save();
        Debug.Log(("minigames save", string.Join(", ", Minigames)));
        //storeData = storeDataParser.Value;
    }
}
