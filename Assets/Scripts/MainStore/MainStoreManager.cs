using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MainStore
{
    public class MainStoreManager : MonoBehaviour
    {
        [SerializeField] MinigameView minigameViewPrefab;
        [SerializeField] Transform minigameViewParent;

        [SerializeField] List<StoreMinigame> minigames;
        public void Start()
        {
            foreach (StoreMinigame minigame in minigames)
            {
                MinigameView view = Instantiate(minigameViewPrefab, minigameViewParent);
                view.Subscribe(minigame, () => SceneManager.LoadScene(minigame.Scene.buildIndex));
            }
        }
    }
}