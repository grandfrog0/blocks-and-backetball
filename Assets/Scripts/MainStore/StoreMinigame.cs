using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MainStore
{
    [CreateAssetMenu(fileName = "Minigame", menuName = "SO/Store Minigame")]
    public class StoreMinigame : ScriptableObject
    {
        public Scene Scene;

        public Sprite Icon;
        public float IngameTime;
    }
}
