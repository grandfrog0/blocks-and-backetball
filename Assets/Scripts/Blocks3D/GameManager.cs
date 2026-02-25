using MainStore;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities.UniversalDelegates;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using UnityEngine.WSA;
using static UnityEngine.UI.GridLayoutGroup;

namespace Blocks3D
{
    public class GameManager : MonoBehaviour
    {
        public UnityEvent OnBeforeGameStart;
        public UnityEvent<float> OnTimerPercentChanged;
        public UnityEvent OnBeforeGameEnd;

        public bool IsGameStarted => _gameRoutine != null;
        public float RoundTime = 20f;
        private Coroutine _gameRoutine;

        public BlocksPlacement PlacementConfig;
        public bool[,] CenterMap { get; private set; }

        [SerializeField] Transform gameFieldTransform;
        [SerializeField] Corner cornerPrefab;
        [SerializeField] GameObject holderPrefab;
        [SerializeField] MovingBlock blockPrefab;
        [SerializeField] Vector3 startPos;
        [SerializeField] float cellSize = 1f;
        private List<Transform> _blocks = new();
        private BlocksField _blocksField;

        /// <summary>
        /// In-game time (in seconds)
        /// </summary>
        public float IngameTime;
        
        public UnityEvent<float> OnScoreChanged;
        public UnityEvent<float> OnBestChanged;
        [SerializeField] DailyRewardManager dailyRewardManager;
        private float _score;
        private float BestScore
        {
            get => dailyRewardManager.DailyRewardConfig.UserConfig.BestScore;
            set
            {
                dailyRewardManager.DailyRewardConfig.UserConfig.BestScore = value;
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
            StartCoroutine(PrepareGameRoutine());
        }
        private IEnumerator PrepareGameRoutine()
        {
            PlacementConfig config = PlacementConfig.GetRandomConfig();

            CenterMap = new bool[config.Size, config.Size];

            List<Vector2Int> cornerPositions = new()
            {
                new Vector2Int(0, 0),
                new Vector2Int(-1, 0),
                new Vector2Int(0, 1),
                new Vector2Int(1, 0),
                new Vector2Int(0, -1)
            };

            List<BlocksField> placements = config.GetPlacements();

            List<Transform> corners = new();
            List<Vector3> cornersStart = new();
            List<Vector3> cornersEnd = new();
            for (int i = 0; i < cornerPositions.Count; i++)
            {
                Vector2Int v2 = cornerPositions[i];

                Vector3 spawnCornerPos = new Vector3(startPos.x + v2.x * config.Size * cellSize, startPos.y, startPos.z + v2.y * config.Size * cellSize);
                Vector3 endCornerPos = spawnCornerPos;
                spawnCornerPos -= Vector3.up * cellSize;

                Corner corner = Instantiate(
                    cornerPrefab,
                    spawnCornerPos,
                    Quaternion.identity,
                    gameFieldTransform
                    );

                List<Transform> holders = new();
                List<Vector3> holdersSpawnPositions = new();
                List<Vector3> holderEndPositions = new();
                for (int y = -config.Size + 1; y <= 0; y++)
                {
                    for (int x = 0; x <= config.Size - 1; x++)
                    {
                        Vector3 start = endCornerPos + new Vector3(x * cellSize, 0, y * cellSize);
                        Vector3 end = start;
                        start -= Vector3.up * cellSize;

                        holders.Add(Instantiate(holderPrefab, start, Quaternion.identity, gameFieldTransform).transform);
                        holdersSpawnPositions.Add(start);
                        holderEndPositions.Add(end);
                    }
                }

                corner.transform.localScale = new Vector3(cellSize * config.Size, 2, cellSize * config.Size);
                spawnCornerPos += new Vector3(cellSize / 2, 0, -cellSize / 2);
                endCornerPos += new Vector3(cellSize / 2, 0, -cellSize / 2);

                foreach (Vector2Int blockV2 in placements[i].placement)
                {
                    MovingBlock block = Instantiate(
                        blockPrefab,
                        corner.transform.position + new Vector3(blockV2.x * cellSize, 2, -blockV2.y * cellSize),
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

                if (placements[i].placement.Count != 0)
                {
                    corners.Add(corner.transform);
                    cornersStart.Add(spawnCornerPos);
                    cornersEnd.Add(endCornerPos);
                }
                else
                {
                    corner.transform.position = endCornerPos;
                }

                for (float t = 0; t <= 1; t += Time.deltaTime / 0.2f)
                {
                    for(int h = 0; h < holders.Count; h++)
                    {
                        holders[h].position = Vector3.Lerp(holdersSpawnPositions[h], holderEndPositions[h], t);
                    }
                    yield return null;
                }
                for (int h = 0; h < holders.Count; h++)
                {
                    holders[h].position = holderEndPositions[h];
                }
            }
            for (int c = 0; c < corners.Count; c++)
            {
                for (float t = 0; t <= 1; t += Time.deltaTime / 0.2f)
                {
                    corners[c].transform.position = Vector3.Lerp(cornersStart[c], cornersEnd[c], t);
                    yield return null;
                }
                corners[c].transform.position = cornersEnd[c];
            }
        }

        private IEnumerator GameRoutine()
        {
            PrepareGameField();

            for (float t = RoundTime; t >= 0; t -= Time.fixedDeltaTime)
            {
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
            OnScoreChanged.Invoke(_score);

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
            if (GlobalManager.Instance)
            {
                GlobalManager.Instance.CurrentMinigame.IngameTime += IngameTime;
            }
            else Debug.Log("GlobalManager Instance does not exists!");
        }
    }
}