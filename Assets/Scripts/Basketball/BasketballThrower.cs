using Basketball;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Basketball
{
    public class BasketballThrower : MonoBehaviour
    {
        [SerializeField] Rigidbody ballPrefab, fireBallPrefab;
        [SerializeField] float throwPower = 10;
        [SerializeField] GameManager gameManager;
        private Vector2 _mouseDelta;

        [SerializeField] GameObject ballPreview, fireBallPreview;
        [SerializeField] Vector3 previewReadyPos, previewNotReadyPosition;

        private BasketballCollider _currentBall;
        private bool _canThrowNext = true;

        [SerializeField] AudioSource fireBallThrowSound;

        public Rigidbody BallPrefab => gameManager.IsLastThrow ? fireBallPrefab : ballPrefab;
        public GameObject BallPreview => gameManager.IsLastThrow ? fireBallPreview : ballPreview;
        public GameObject AnotherBallPreview => !gameManager.IsLastThrow ? fireBallPreview : ballPreview;


        private void Update()
        {
            if (AnotherBallPreview.activeSelf)
            {
                AnotherBallPreview.transform.position = previewNotReadyPosition;
                AnotherBallPreview.SetActive(false);
            }
            if (!BallPreview.activeSelf)
            {
                BallPreview.SetActive(true);
                BallPreview.transform.rotation = Random.rotation;
            }

            if (!gameManager.IsPlaying || !_canThrowNext)
            {
                BallPreview.transform.localPosition = Vector3.Lerp(BallPreview.transform.localPosition, previewNotReadyPosition, 5 * Time.deltaTime);
                return;
            }

            BallPreview.transform.localPosition = Vector3.Lerp(BallPreview.transform.localPosition, previewReadyPos + Vector3.up * Mathf.Sin(Time.time * 2) * 0.01f, 5 * Time.deltaTime);

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

                    Rigidbody ball = Instantiate(BallPrefab, BallPreview.transform.position, Random.rotation);
                    BallPreview.transform.rotation = ball.transform.rotation;
                    ball.AddForce(force, ForceMode.Impulse);
                    ball.angularVelocity = force;

                    _canThrowNext = false;
                    _currentBall = ball.GetComponent<BasketballCollider>();
                    _currentBall.OnCollided.AddListener(OnCurrentBallCollided);
                }

                _mouseDelta = Vector2.zero;

                if (gameManager.IsLastThrow)
                {
                    StartCoroutine(FireBallThrowingRoutine());
                }
            }

            void OnCurrentBallCollided()
            {
                _canThrowNext = true;
                _currentBall.OnCollided.RemoveListener(OnCurrentBallCollided);
            }
        }

        private IEnumerator FireBallThrowingRoutine()
        {
            fireBallThrowSound.Play();

            Time.timeScale = 0.25f; 
            bool isBallCollided = false;
            _currentBall.OnCollided.AddListener(OnFireBallCollided);

            yield return new WaitWhile(() => !isBallCollided);

            _currentBall.OnCollided.RemoveListener(OnFireBallCollided);
            Time.timeScale = 1f;

            gameManager.IsLastThrow = false;

            void OnFireBallCollided() => isBallCollided = true;
        }
    }
}
