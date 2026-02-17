using Blocks3D;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Blocks3D
{
    public class MovingBlock : MonoBehaviour
    {
        [SerializeField] DisposableParticles crashParticles;
        public Corner Corner { get; set; }
        private void OnMouseDown()
        {
            Corner.MoveBlocks();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out MovingBlock _) && !gameObject.IsDestroyed())
            {
                Corner.Crash();
                Crash();
            }
        }

        public void Crash()
        {
            crashParticles.Play();
            //Destroy(gameObject);
            gameObject.SetActive(false);
        }
    }
}
