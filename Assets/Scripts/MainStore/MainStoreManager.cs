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

        [SerializeField] Sprite serializeSprite;

        public void Start()
        {
            Debug.Log(("minigames load", string.Join(", ", GlobalManager.Instance.Minigames)));

            //SpriteData data = new SpriteData()
            //{
            //    Width = serializeSprite.texture.width,
            //    Height = serializeSprite.texture.height,
            //    TextureBytes = ImageConversion.EncodeToPNG(serializeSprite.texture)
            //};
            //GlobalManager.Instance.Minigames[1].SpriteData = data;

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