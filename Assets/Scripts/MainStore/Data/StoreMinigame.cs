using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MainStore
{
    [Serializable]
    public class StoreMinigame
    {
        public string ExePath;

        public SpriteData SpriteData;
        public Sprite Icon
        {
            get
            {
                Texture2D texture = new Texture2D(SpriteData.Width, SpriteData.Height);
                texture.LoadImage(SpriteData.TextureBytes);
                return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
            }
        }
        public float IngameTime;
        public float Best;

        public override string ToString() => Path.GetFileName(ExePath);
    }
}
