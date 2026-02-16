using Blocks3D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blocks3D
{
    public class MovingBlock : MonoBehaviour
    {
        public GameManager GameManager { get; set; }
        public Corner Corner { get; set; }
        private void OnMouseDown()
        {
            Corner.MoveBlocks();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out MovingBlock _))
            {
                GameManager.LoseGame();
            }
        }
    }
}
