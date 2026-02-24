using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            Debug.Log(("minigames load", string.Join(", ", GlobalManager.Instance.Minigames)));

            foreach (StoreMinigame minigame in GlobalManager.Instance.Minigames)
            {
                MinigameView view = Instantiate(minigameViewPrefab, minigameViewParent);
                view.Subscribe(minigame, () => LoadScene(minigame));
            }
        }

        public void LoadScene(StoreMinigame minigame)
        {
            GlobalManager.Instance.CurrentMinigame = minigame;
            SceneManager.LoadScene(minigame.SceneName);
        }
    }
}