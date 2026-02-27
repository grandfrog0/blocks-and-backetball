using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketMover : MonoBehaviour
{
    private bool _isActive = false;

    public void SetActive(bool value)
        => _isActive = value;

    [SerializeField] Vector3 moveVector;
    [SerializeField] float moveSpeed;

    private void FixedUpdate()
    {
        if (!_isActive)
            return;

        transform.Translate(moveVector * Mathf.Sin(Time.time * moveSpeed) * Time.fixedDeltaTime);
    }
}
