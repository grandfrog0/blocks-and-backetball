using MainStore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalManager : MonoBehaviour
{
    public static GlobalManager Instance { get; private set; }

    [SerializeField] StoreData storeData;

    public float Coins
    {
        get => storeData.Coins;
        set => storeData.Coins = value;
    }

    public List<StoreMinigame> Minigames
        => storeData.Minigames;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
