using MainStore;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

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

    public List<StoreMinigame> Minigames => storeData.Minigames;
    public List<string> MinigamesSavePaths { get; } = new();
    
    private string MinigamesPath => Path.Combine("General", "Minigames");
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

        _minigamesParser = new Parser<StoreMinigame>(MinigamesPath, GlobalManager.Instance.Minigames);
        _minigamesParser.Load();

        foreach(StoreMinigame game in GlobalManager.Instance.Minigames)
        {
            //game.
        }

        MinigamesSavePaths.AddRange(_minigamesParser.SavePaths);
    }

    private void OnApplicationQuit()
    {
        _minigamesParser.Save();
        Debug.Log(("minigames save", string.Join(", ", GlobalManager.Instance.Minigames)));
    }
}
