using System.Collections;
using UnityEngine;

namespace Tennis
{
    public class Opponent : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private Transform _ballTransform;

        private Vector2 _defaultPosition;
        private Transform _transform;
        private Coroutine _movingCoroutine;

        private void Awake()
        {
            _transform = transform;
            _defaultPosition = _transform.position;
        }

        public void StartMovement()
        {
            if (_movingCoroutine == null)
            {
                _movingCoroutine = StartCoroutine(Move());
            }
        }

        public void StopMovement()
        {
            if (_movingCoroutine != null)
            {
                StopCoroutine(_movingCoroutine);
                _movingCoroutine = null;
            }
        }

        public void ReturnToDefaultPosition()
        {
            _transform.position = _defaultPosition;
        }

        private IEnumerator Move()
        {
            while (true)
            {
                if (_ballTransform != null)
                {
                    Vector2 targetPosition = new Vector2(_ballTransform.position.x, _transform.position.y);
                    
                    _transform.position = Vector2.Lerp(targetPosition, _transform.position, _speed * Time.deltaTime);
                }

                yield return null;
            }
        }
    }
}
