using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Basketball
{
    public class BasketCollider : MonoBehaviour
    {
        [SerializeField] Animator animator;
        [SerializeField] string animationName;

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Basketball"))
            {
                animator.Play(animationName);
            }
        }
    }
}