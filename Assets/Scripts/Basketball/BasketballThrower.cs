using Basketball;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Basketball
{
    public class BasketballThrower : MonoBehaviour
    {
        [SerializeField] Rigidbody ballPrefab;
        [SerializeField] Transform throwPoint;
        [SerializeField] float throwPower = 10;
        [SerializeField] GameManager gameManager;
        private Vector2 _mouseDelta;

        [SerializeField] GameObject ballPreview;
        [SerializeField] Vector3 previewReadyPos, previewNotReadyPosition;

        private BasketballCollider _currentBall;
        private bool _canThrowNext = true;

        private void Update()
        {
            if (!gameManager.IsPlaying || !_canThrowNext)
            {
                ballPreview.transform.localPosition = Vector3.Lerp(ballPreview.transform.localPosition, previewNotReadyPosition, 5 * Time.deltaTime);
                return;
            }

            ballPreview.transform.localPosition = Vector3.Lerp(ballPreview.transform.localPosition, previewReadyPos, 5 * Time.deltaTime);

            if (Input.GetMouseButton(0))
            {
                _mouseDelta = Vector2.Lerp(_mouseDelta + new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")), Vector2.zero, Time.deltaTime);
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (_mouseDelta.magnitude > 1f)
                {
                    Vector2 axis = _mouseDelta;
                    Vector3 force = Camera.main.transform.rotation * new Vector3(axis.x, axis.y, axis.y) * throwPower;

                    Rigidbody ball = Instantiate(ballPrefab, throwPoint.position, Random.rotation);
                    ballPreview.transform.rotation = ball.transform.rotation;
                    ball.AddForce(force, ForceMode.Impulse);
                    ball.angularVelocity = force;

                    _canThrowNext = false;
                    _currentBall = ball.GetComponent<BasketballCollider>();
                    _currentBall.OnCollided.AddListener(OnCurrentBallCollided);
                }

                _mouseDelta = Vector2.zero;
            }
        }

        private void OnCurrentBallCollided()
        {
            _canThrowNext = true;
        }
    }
}
