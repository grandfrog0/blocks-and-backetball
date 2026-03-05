using MainStore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class GlobalManager : MonoBehaviour
{
    public static GlobalManager Instance { get; private set; }
    [SerializeField] StoreData storeData;

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

    public void AddMinigame(string exePath)
    {
        // if minigames already has config for this exe file
        if (Minigames.Any(x => x.ExePath == exePath))
        {
            Debug.Log("config already exists.");
            return;
        }

        StoreMinigame storeMinigame = new StoreMinigame()
        {
            ExePath = exePath,
            Best = 0,
            IngameTime = 0,
            SpriteData = new SpriteData(),
        };
        Minigames.Add(storeMinigame);
        MinigamesSavePaths.Add(Path.Combine(MinigamesPath, storeMinigame.ToString()));
        Debug.Log(Path.Combine(MinigamesPath, storeMinigame.ToString()));

        _minigamesParser.Save();
        OnMinigamesRefresh.Invoke();
    }
}
