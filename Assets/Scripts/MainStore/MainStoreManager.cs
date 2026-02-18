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

        public void Start()
        {
            foreach (StoreMinigame minigame in GlobalManager.Instance.Minigames)
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