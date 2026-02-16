using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Blocks3D
{
    public class Corner : MonoBehaviour
    {
        public bool[,] CenterMap { get; set; }
        public List<Vector2Int> PlacedBlocks { get; } = new();
        public Vector3 Direction { get; set; }
        public float CellSize { get; set; }
        [SerializeField] float speed = 4f;

        private bool _isClickable = true;

        private void OnMouseDown()
        {
            MoveBlocks();
        }

        public void MoveBlocks()
        {
            if (!_isClickable || Direction == Vector3.zero)
                return;

            _isClickable = false;
            StartCoroutine(MoveRoutine());
        }

        private IEnumerator MoveRoutine()
        {
            Vector2Int arrayDirection = new Vector2Int((int)Direction.x, -(int)Direction.z);
            int moveCellsCount = CenterMap.GetLength(0);
            int moveCellsOffset = 0;

            foreach (Vector2Int blockPos in PlacedBlocks)
            {
                Vector2Int startPos = new Vector2Int(
                    Mathf.Clamp(-arrayDirection.x, 0, CenterMap.GetLength(0) - 1),
                    Mathf.Clamp(-arrayDirection.y, 0, CenterMap.GetLength(1) - 1)
                    );

                if (arrayDirection.y == 0)
                {
                    startPos.y += -blockPos.y;
                    moveCellsOffset += CenterMap.GetLength(0) - 1 - blockPos.x;
                }
                else if (arrayDirection.x == 0)
                {
                    startPos.x += blockPos.x;
                    moveCellsOffset += CenterMap.GetLength(0) - 1 + blockPos.y;
                }

                Vector2Int pos = startPos;

                int cells = 0;
                while (pos.x >= 0 && pos.x < CenterMap.GetLength(0)
                    && pos.y >= 0 && pos.y < CenterMap.GetLength(1))
                {
                    if (!CenterMap[pos.x, pos.y])
                    {
                        pos += arrayDirection;
                        cells++;
                    }
                }

                moveCellsCount = Mathf.Min(moveCellsCount, cells);
            }
            moveCellsCount += moveCellsOffset;
            Debug.Log(moveCellsCount);

            Vector3 start = transform.position;
            Vector3 target = start + Direction * CellSize * moveCellsCount;

            for (float t = 0; t <= 1; t += Time.deltaTime * speed)
            {
                transform.position = Vector3.Lerp(start, target, t);
                yield return null;
            }
            transform.position = target;
        }
    }
}
