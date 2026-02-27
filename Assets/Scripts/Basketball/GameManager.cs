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

        public UnityEvent<int> OnScoreChanged;
        public UnityEvent<int> OnTargetChanged;
        public UnityEvent<int, int> OnTimerChanged;
        public UnityEvent<int> OnLevelChanged;
        public UnityEvent<int> OnBestChanged;

        public UnityEvent OnWin;
        public UnityEvent OnLose;

        public BasketballGameConfig BasketballGameConfig { get; private set; }
        private ItemParser<BasketballGameConfig> _gameConfigParser;

        private Coroutine _timerRoutine;

        public bool IsPlaying { get; private set; }
        private int _score;
        public int Score 
        {
            get => _score; 
            private set
            {
                _score = value;
                OnScoreChanged.Invoke(value);
            }
        }
        private int _best;
        public int Best
        {
            get => _best;
            set
            {
                _best = value;
                OnBestChanged.Invoke(value);
            }
        }
        private int _winScore;

        [SerializeField] LevelsManager levelManager;

        public void OnBallGoal()
        {
            Score++;
            levelManager.AddScore();

            BasketballGameConfig.CurrentLevel = levelManager.CurrentLevel + 1;
            OnLevelChanged.Invoke(BasketballGameConfig.CurrentLevel);

            BasketballGameConfig.NextLevelScoreCount = Score + levelManager.TargetScore;
            OnTargetChanged.Invoke(BasketballGameConfig.NextLevelScoreCount);

            if (Score > Best)
            {
                Best = Score;
            }
        }
        
        private void Start()
        {
            Load();
            OnSceneStart.Invoke();

            OnScoreChanged.Invoke(Score);
            OnTargetChanged.Invoke(BasketballGameConfig.NextLevelScoreCount);
            OnLevelChanged.Invoke(BasketballGameConfig.CurrentLevel);
            OnBestChanged.Invoke(Best);
        }

        public void StartGame()
        {
            if (_timerRoutine != null)
                return;

            Score = 0;
            _winScore = levelManager.TargetScore;

            _timerRoutine = StartCoroutine(TimerRoutine());
            OnLoadingEnd.Invoke();
        }

        private IEnumerator TimerRoutine()
        {
            IsPlaying = true;

            for (float i = BasketballGameConfig.LevelAvailableTime; i >= 0 ; i--)
            {
                OnTimerChanged.Invoke((int)i / 60, (int)i % 60);
                yield return new WaitForSeconds(1);
            }

            _timerRoutine = null;
            EndGame();
        }

        public void EndGame()
        {
            if (_timerRoutine != null)
                StopCoroutine(_timerRoutine);

            IsPlaying = false;

            if (Score >= _winScore)
            {
                OnWin.Invoke();
            }
            else
            {
                OnLose.Invoke();
            }
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
            _gameConfigParser.Save();
        }
    }
}