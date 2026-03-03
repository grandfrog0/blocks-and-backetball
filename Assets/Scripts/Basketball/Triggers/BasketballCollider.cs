using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BasketballCollider : MonoBehaviour
{
    public UnityEvent OnCollided;
    private bool _isDestroying;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Chain"))
        {
            collision.gameObject.GetComponent<AudioSource>().Play();
        }
        else if (!_isDestroying && collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject, 4);
            _isDestroying = true;
        }

        OnCollided.Invoke();
    }
}
