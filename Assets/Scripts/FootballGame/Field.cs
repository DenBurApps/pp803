using System;
using UnityEngine;

namespace FootballGame
{
    [RequireComponent(typeof(LinearMovementComponent))]
    [RequireComponent(typeof(Collider2D))]
    public class Field : MonoBehaviour
    {
        private BoxCollider2D _collider;
        private LinearMovementComponent _linearMovementComponent;
        private Transform _transform;
        private Vector2 _defaultPosition;

        public event Action Hit;

        private void Awake()
        {
            _transform = transform;
            _defaultPosition = _transform.position;
            _collider = GetComponent<BoxCollider2D>();
            _linearMovementComponent = GetComponent<LinearMovementComponent>();
        }

        private void Start()
        {
            _linearMovementComponent.SetMovingDirection(Vector2.right);
        }

        public void SetDefault()
        {
            _transform.position = _defaultPosition;
        }

        public void EnableMovement()
        {
            _linearMovementComponent.EnableMovement();
            _collider.enabled = true;
        }

        public void DisableMovement()
        {
            _linearMovementComponent.DisableMovement();
            _collider.enabled = false;
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.TryGetComponent(out Ball ball))
            {
                ball.transform.position = _transform.position;
                Hit?.Invoke();
            }
        }
    }
}
