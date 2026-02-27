using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Basketball
{
    public class LevelsManager : MonoBehaviour
    {
        public List<LevelData> Levels;
        public int CurrentLevel;
        private int _subLevel;

        [SerializeField] CameraCinemachine cameraCinemachine;
        [SerializeField] BasketMover basketMover;

        private bool _isActive = true;

        public int TargetScore => Levels[CurrentLevel].CameraPositions.Count - _subLevel;

        public void AddScore()
        {
            if (!_isActive)
                return;

            _subLevel++;
            if (Levels[CurrentLevel].CameraPositions.Count > _subLevel)
            {
                cameraCinemachine.SetState(Levels[CurrentLevel].CameraPositions[_subLevel]);
            }
            else
            {
                //CurrentLevel++;
                CurrentLevel = ++CurrentLevel % Levels.Count;
                _subLevel = 0;

                //if (CurrentLevel >= Levels.Count)
                //{
                //    _isActive = false;
                //    return;
                //}

                cameraCinemachine.SetState(Levels[CurrentLevel].CameraPositions[_subLevel]);
                basketMover.SetActive(Levels[CurrentLevel].IsBasketMoving);
            }
        }
    }
}