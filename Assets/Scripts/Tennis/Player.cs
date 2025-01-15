using System;
using System.Collections;
using UnityEngine;

namespace Tennis
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private float _fieldBoundaryY;
        [SerializeField] private LayerMask _ballMask;
        [SerializeField] private GameObject _rocket;
        [SerializeField] private AudioSource _ballHit;
        
        private IEnumerator _detectingCoroutine;

        public event Action CatchedBall;
        
        public void EnableDetection()
        {
            DisableDetection();

            _detectingCoroutine = StartDetecting();
            StartCoroutine(_detectingCoroutine);
        }

        public void DisableDetection()
        {
            if (_detectingCoroutine != null)
            {
                StopCoroutine(_detectingCoroutine);
                _detectingCoroutine = null;
            }
        }

        private IEnumerator StartDetecting()
        {
            while (enabled)
            {
                if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);

                    if (touch.phase == TouchPhase.Began)
                    {
                        Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                        RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero, Mathf.Infinity, _ballMask);

                        if (hit.collider != null)
                        {
                            TennisBall tennisBall = hit.collider.GetComponent<TennisBall>();
                            if (tennisBall != null  && IsBallOnPlayerField(tennisBall))
                            {
                                tennisBall.ThrowBack();
                                StartCoroutine(EnableRocket(hit.transform));
                                CatchedBall?.Invoke();
                            }
                        }
                    }
                }
                
                yield return null;
            }
        }

        private IEnumerator EnableRocket(Transform position)
        {
            _rocket.gameObject.SetActive(true);
            _rocket.transform.position = position.position;
            _ballHit.Play();
            yield return new WaitForSeconds(1);
            
            _rocket.gameObject.SetActive(false);
        }
        
        private bool IsBallOnPlayerField(TennisBall ball)
        {
            return ball.transform.position.y <= _fieldBoundaryY;
        }
    }
}
