using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MainStore
{
    [Serializable]
    public class StoreMinigame
    {
        public string SceneName;

        public Sprite Icon;
        public float IngameTime;

        public override string ToString() => SceneName;
    }
}
