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

        public Texture2D Texture { get; set; }
        public Sprite Icon => Sprite.Create(Texture, new Rect(0, 0, Texture.width, Texture.height), Vector2.one * 0.5f);

        public float IngameTime;
        public float Best;

        public override string ToString() => Path.GetFileName(ExePath);
    }
}
