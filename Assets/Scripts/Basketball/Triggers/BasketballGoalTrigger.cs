using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BasketballGoalTrigger : MonoBehaviour
{
    public UnityEvent OnTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Basketball"))
        {
            OnTrigger.Invoke();
        }
    }
}
