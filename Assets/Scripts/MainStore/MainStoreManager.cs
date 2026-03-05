using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MainStore
{
    public class MainStoreManager : MonoBehaviour
    {
        [SerializeField] MinigameView minigameViewPrefab;
        [SerializeField] Transform minigameViewParent;
        private List<MinigameView> _minigameViews = new();

        [SerializeField] AppLauncher launcher;

        public void Start()
        {
            Debug.Log(("minigames load", string.Join(", ", GlobalManager.Instance.Minigames)));
            RefreshMinigames();
        }

        public void RefreshMinigames()
        {
            foreach (MinigameView view in _minigameViews)
            {
                Destroy(view.gameObject);
            }
            _minigameViews.Clear();

            foreach (StoreMinigame minigame in GlobalManager.Instance.Minigames)
            {
                MinigameView view = Instantiate(minigameViewPrefab, minigameViewParent);
                view.Subscribe(minigame, () => launcher.LaunchExternalApp(minigame.ExePath, GlobalManager.Instance.MinigamesSavePaths[GlobalManager.Instance.Minigames.IndexOf(minigame)]));
                _minigameViews.Add(view);
            }
        }
    }
}