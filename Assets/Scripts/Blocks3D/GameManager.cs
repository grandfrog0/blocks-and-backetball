using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Blocks3D
{
    public class GameManager : MonoBehaviour
    {
        public UnityEvent OnBeforeGameStart;
        public UnityEvent<float> OnTimerChanged;
        public UnityEvent<float> OnTimerPercentChanged;
        public UnityEvent OnBeforeGameEnd;
        public UnityEvent OnWin;
        public UnityEvent OnLose;

        public bool IsGameStarted { get; private set; }
        public float RoundTime = 20f;

        public void StartGame()
        {
            if (IsGameStarted)
                return;

            OnBeforeGameStart.Invoke();
            StartCoroutine(GameRoutine());
        }

        private IEnumerator GameRoutine()
        {
            for (float t = RoundTime; t >= 0; t -= Time.fixedDeltaTime)
            {
                OnTimerChanged.Invoke(t);
                OnTimerPercentChanged.Invoke(t / RoundTime);
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }

            PrepareResults();
        }

        private void PrepareResults()
        {
            OnBeforeGameEnd.Invoke();

            OnLose.Invoke();
        }
    }
}