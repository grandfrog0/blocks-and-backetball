using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Basketball
{
    public class GameManager : MonoBehaviour
    {
        public UnityEvent OnSceneStart;
        public UnityEvent OnLoadingEnd;

        public BasketballGameConfig BasketballGameConfig { get; private set; }
        private ItemParser<BasketballGameConfig> _gameConfigParser;
        
        private void Start()
        {
            Load();
            OnSceneStart.Invoke();
        }

        public void StartGame()
        {
            OnLoadingEnd.Invoke();
        }

        private void Load()
        {
            _gameConfigParser = new ItemParser<BasketballGameConfig>("Basketball/BasketballGameConfig.json", BasketballGameConfig);
            _gameConfigParser.Load();
            BasketballGameConfig = _gameConfigParser.Value;

            _gameConfigParser.Value = new BasketballGameConfig()
            {
                CurrentLevel = 1,
                NextLevelScoreCount = 1,
                LevelAvailableTime = 30
            };
            Debug.Log(_gameConfigParser.Value.CurrentLevel);
            _gameConfigParser.Save();
        }
    }
}