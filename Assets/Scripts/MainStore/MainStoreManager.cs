using System;
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

        [SerializeField] SavedMinigames minigames;
        public void Start()
        {
            foreach (StoreMinigame minigame in minigames.Saved)
            {
                MinigameView view = Instantiate(minigameViewPrefab, minigameViewParent);
                view.Subscribe(minigame, () => LoadScene(minigame.SceneName));
            }
        }

        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}