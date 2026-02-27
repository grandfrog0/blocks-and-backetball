using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BasketballCollider : MonoBehaviour
{
    public UnityEvent OnCollided;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Chain"))
        {
            collision.gameObject.GetComponent<AudioSource>().Play();
        }
        OnCollided.Invoke();
    }
}
