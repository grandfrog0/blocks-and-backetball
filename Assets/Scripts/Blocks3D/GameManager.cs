using MainStore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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

        public bool IsGameStarted => _gameRoutine != null;
        public float RoundTime = 20f;
        private Coroutine _gameRoutine;

        public PlacementConfig PlacementConfig;
        public bool[,] CenterMap { get; private set; }

        private int _readyCornersCount;
        public int ReadyCornersCount
        {
            get => _readyCornersCount;
            set
            {
                _readyCornersCount = value;

                if (_readyCornersCount == 4)
                {
                    Next();
                }
            }
        }

        [SerializeField] Transform gameFieldTransform;
        [SerializeField] Corner cornerPrefab;
        [SerializeField] GameObject holderPrefab;
        [SerializeField] MovingBlock blockPrefab;
        [SerializeField] Vector3 startPos;
        [SerializeField] float cellSize = 1f;
        private List<Transform> _blocks = new();
        private BlocksField _blocksField;

        /// <summary>
        /// In-game time (in milliseconds)
        /// </summary>
        public float IngameTime;
        [SerializeField] StoreMinigame storeMinigame;

        public UnityEvent<float> OnBestChanged;
        [SerializeField] UserConfig userConfig;
        private float _score;
        private float BestScore
        {
            get => userConfig.BestScore;
            set
            {
                userConfig.BestScore = value;
                OnBestChanged.Invoke(value);
            }
        }

        public void StartGame()
        {
            if (IsGameStarted)
                return;

            OnBeforeGameStart.Invoke();
            _gameRoutine = StartCoroutine(GameRoutine());
        }

        private void PrepareGameField()
        {
            CenterMap = new bool[PlacementConfig.Size, PlacementConfig.Size];
            ReadyCornersCount = 0;

            List<Vector2Int> cornerPositions = new()
            {
                new Vector2Int(0, 0),
                new Vector2Int(-1, 0),
                new Vector2Int(0, 1),
                new Vector2Int(1, 0),
                new Vector2Int(0, -1)
            };

            for (int i = 0; i < cornerPositions.Count; i++)
            {
                Vector2Int v2 = cornerPositions[i];

                Corner corner = Instantiate(
                    cornerPrefab,
                    new Vector3(startPos.x + v2.x * PlacementConfig.Size * cellSize, startPos.y, startPos.z + v2.y * PlacementConfig.Size * cellSize),
                    Quaternion.identity,
                    gameFieldTransform
                    );

                for (int y = -PlacementConfig.Size + 1; y <= 0; y++)
                {
                    for (int x = 0; x <= PlacementConfig.Size - 1; x++)
                    {
                        Instantiate(holderPrefab, corner.transform.position + new Vector3(x * cellSize, 0, y * cellSize), Quaternion.identity, gameFieldTransform);
                    }
                }

                corner.transform.localScale = new Vector3(cellSize * PlacementConfig.Size, 2, cellSize * PlacementConfig.Size);
                corner.transform.Translate(cellSize / 2, 0, -cellSize / 2);

                foreach (Vector2Int blockV2 in PlacementConfig.Fields[i].placement)
                {
                    MovingBlock block = Instantiate(
                        blockPrefab,
                        corner.transform.position + new Vector3(blockV2.x * cellSize, 2, blockV2.y * cellSize),
                        Quaternion.identity
                        );

                    block.transform.parent = corner.transform;
                    block.transform.Translate(-cellSize / 2, 0, cellSize / 2);

                    block.Corner = corner;
                    corner.PlacedBlocks.Add(blockV2);
                }

                corner.Direction = new Vector3(-v2.x, 0, -v2.y);
                corner.CellSize = cellSize;
                corner.GameManager = this;
            }
        }

        private IEnumerator GameRoutine()
        {
            PrepareGameField();

            for (float t = RoundTime; t >= 0; t -= Time.fixedDeltaTime)
            {
                OnTimerChanged.Invoke(t);
                OnTimerPercentChanged.Invoke(t / RoundTime);
                yield return new WaitForSeconds(Time.fixedDeltaTime);
                IngameTime += Time.fixedDeltaTime;
            }

            PrepareResults();
            _gameRoutine = null;
        }

        public void LoseGame()
        {
            if (IsGameStarted)
            {
                StopCoroutine(_gameRoutine);
                _gameRoutine = null;
                PrepareResults();
            }
        }

        public void Next()
        {
            _score++;
            if (BestScore < _score)
                BestScore = _score;

            // destroy everything;
            for (int i = 0; i < gameFieldTransform.childCount; i++)
            {
                Destroy(gameFieldTransform.GetChild(i).gameObject);
            }

            PrepareGameField();
        }

        private void PrepareResults()
        {
            OnBeforeGameEnd.Invoke();

            if (BestScore < _score)
                BestScore = _score;
        }

        private void Start()
        {
            OnBestChanged.Invoke(BestScore);
        }

        private void OnDestroy()
        {
            Debug.Log("Ingame-time: " + IngameTime);
            storeMinigame.IngameTime += IngameTime;
        }
    }
}