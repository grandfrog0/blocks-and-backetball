using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketballThrower : MonoBehaviour
{
    [SerializeField] Rigidbody ballPrefab;
    [SerializeField] Transform throwPoint;
    [SerializeField] float throwPower = 10;
    private Vector2 _mouseDelta;

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            _mouseDelta = Vector2.Lerp(_mouseDelta + new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")), Vector2.zero, Time.deltaTime);
        }
            
        if (Input.GetMouseButtonUp(0))
        {
            Vector2 axis = _mouseDelta;
            Vector3 force = Camera.main.transform.rotation * new Vector3(axis.x, axis.y, axis.y) * throwPower;

            Rigidbody ball = Instantiate(ballPrefab, throwPoint.position, throwPoint.rotation);
            ball.AddForce(force, ForceMode.Impulse);

            _mouseDelta = Vector2.zero;
        }
    }
}
