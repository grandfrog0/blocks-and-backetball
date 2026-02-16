using MainStore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Minigames", menuName = "SO/Saved Minigames")]
public class SavedMinigames : ScriptableObject
{
    public List<StoreMinigame> Saved;
}
