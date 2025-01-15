using System.Collections;
using UnityEngine;

namespace Pole
{
    public class Wall : MonoBehaviour
    {
        [SerializeField] private float _speed;

        private Transform _transform;
        private IEnumerator _movingCoroutine;

        private void Awake()
        {
            _transform = transform;
        }

        public void EnableMovement()
        {
            if (_movingCoroutine == null)
                _movingCoroutine = StartMoving();

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

        private IEnumerator StartMoving()
        {
            while (true)
            {
                _transform.position += Vector3.left * _speed * Time.deltaTime;

                yield return null;
            }
        }
    }
}