using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Blocks3D
{
    public class Corner : MonoBehaviour
    {
        public GameManager GameManager { get; set; }
        public List<Vector2Int> PlacedBlocks { get; } = new();
        public List<Vector2Int> MovingBlocks { get; } = new();
        public Vector3 Direction { get; set; }
        public float CellSize { get; set; }
        [SerializeField] float speed = 4f;
        public bool IsMoving => _moveRoutine != null;

        private bool _isClickable = true;
        private Coroutine _moveRoutine;

        public void Crash()
        {
            if (_moveRoutine != null)
            {
                StopCoroutine(_moveRoutine);
                _moveRoutine = null;
            }
            GameManager.LoseGame();
        }

        private void OnMouseDown()
        {
            MoveBlocks();
        }

        public void MoveBlocks()
        {
            if (!_isClickable || Direction == Vector3.zero || !GameManager.IsGameStarted)
                return;

            _isClickable = false;
            _moveRoutine = StartCoroutine(MoveRoutine());
        }

        private IEnumerator MoveRoutine()
        {
            Vector2Int arrayDirection = new Vector2Int((int)Direction.x, -(int)Direction.z);
            int size = GameManager.CenterMap.GetLength(0);
            int moveCellsCount = size;

            foreach (Vector2Int pos in PlacedBlocks)
            {
                GameManager.CenterMap[pos.x, pos.y] = true;
            }

            Vector3 start = transform.position;
            Vector3 target = start + Direction * CellSize * moveCellsCount;

            for (float t = 0; t <= 1; t += Time.deltaTime * speed / moveCellsCount)
            {
                transform.position = Vector3.Lerp(start, target, t);
                yield return null;
            }
            transform.position = target;

            if (IsCenterFilled)
            {
                GameManager.Next();
            }

            _moveRoutine = null;
        }
        /*private IEnumerator MoveRoutine()
        {
            Vector2Int arrayDirection = new Vector2Int((int)Direction.x, -(int)Direction.z);
            int size = GameManager.CenterMap.GetLength(0);
            int moveCellsCount = size;
            int moveCellsOffset = 0;

            bool[,] tempMap = (bool[,])GameManager.CenterMap.Clone();

            foreach (Vector2Int blockPos in PlacedBlocks)
            {
                Vector2Int startPos = new Vector2Int(
                    Mathf.Clamp(-arrayDirection.x, 0, size - 1),
                    Mathf.Clamp(-arrayDirection.y, 0, size - 1)
                    );

                int tempOffset = 0;
                if (arrayDirection.y == 0)
                {
                    startPos.y += -blockPos.y;
                    tempOffset += arrayDirection.x > 0 ? size - blockPos.x : blockPos.x + (size - 1);
                }
                else if (arrayDirection.x == 0)
                {
                    startPos.x += blockPos.x;
                    tempOffset += arrayDirection.y > 0 ? size + blockPos.y : -blockPos.y + (size - 1);
                }

                Vector2Int pos = startPos;

                int cells = 0;
                while ((pos + arrayDirection).x >= 0 && (pos + arrayDirection).x < size
                    && (pos + arrayDirection).y >= 0 && (pos + arrayDirection).y < size
                    && !tempMap[(pos + arrayDirection).x, (pos + arrayDirection).y])
                {
                    pos += arrayDirection;
                    cells++;
                }

                tempMap[pos.x, pos.y] = true;

                moveCellsCount = Mathf.Min(moveCellsCount, cells);
                moveCellsOffset = Mathf.Max(moveCellsOffset, tempOffset);
            }

            moveCellsCount += moveCellsOffset;

            foreach (Vector2Int blockPos in PlacedBlocks)
            {
                Vector2Int pos = new Vector2Int(blockPos.x, -blockPos.y);
                pos += arrayDirection * moveCellsCount;
                pos -= new Vector2Int(size * arrayDirection.x, size * arrayDirection.y);

                GameManager.CenterMap[pos.x, pos.y] = true;
            }

            Vector3 start = transform.position;
            Vector3 target = start + Direction * CellSize * moveCellsCount;

            for (float t = 0; t <= 1; t += Time.deltaTime * speed / moveCellsCount)
            {
                transform.position = Vector3.Lerp(start, target, t);
                yield return null;
            }
            transform.position = target;

            GameManager.ReadyCornersCount++;
        }*/

        private bool IsCenterFilled
        {
            get
            {
                foreach(var x in GameManager.CenterMap)
                {
                    if (!x)
                        return false;
                }
                return true;
            }
        }
    }
}
