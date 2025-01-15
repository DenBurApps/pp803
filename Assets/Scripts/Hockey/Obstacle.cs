using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Hockey
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Obstacle : MonoBehaviour
    {
        [SerializeField] private float _speed;
        

        private Transform _transform;
        private IEnumerator _movingCoroutine;
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _transform = transform;
            _spriteRenderer = GetComponent<SpriteRenderer>();
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

        public void AssignSprite(Sprite sprite)
        {
            _spriteRenderer.sprite = sprite;
        }
        
        private IEnumerator StartMoving()
        {
            while (true)
            {
                _transform.position += Vector3.down * _speed * Time.deltaTime;

                yield return null;
            }
        }
    }
}