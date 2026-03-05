using MainStore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StoreData", menuName = "SO/Store Data")]
public class StoreData : ScriptableObject
{
    public float Coins;
    public List<StoreMinigame> Minigames;
}
