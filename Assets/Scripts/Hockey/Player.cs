using System;
using System.Collections;
using UnityEngine;

namespace Hockey
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _minX;
        [SerializeField] private float _maxX;

        private IEnumerator _movingCoroutine;
        private Transform _transform;

        public event Action HitObsticle;

        private void Awake()
        {
            _transform = transform;
        }

        public void EnableMovement()
        {
            DisableMovement();

            _movingCoroutine = TiltMovement();
            StartCoroutine(_movingCoroutine);
        }

        public void DisableMovement()
        {
            if (_movingCoroutine != null)
            {
                StopCoroutine(_movingCoroutine);
                _movingCoroutine = null;
            }
        }
        
        private IEnumerator TiltMovement()
        {
            while (true)
            {
                float tilt = Input.acceleration.x;
                
#if UNITY_EDITOR
                if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
                    tilt = -1f;
                else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
                    tilt = 1f;
                else
                    tilt = 0f;
#endif
                
                Vector3 newPosition = _transform.position;
                newPosition.x += tilt * _speed * Time.deltaTime;
                
                newPosition.x = Mathf.Clamp(newPosition.x, _minX, _maxX);

                _transform.position = newPosition;
                
                
                yield return null;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Obstacle obstacle))
            {
                HitObsticle?.Invoke();
            }
        }
    }
}
