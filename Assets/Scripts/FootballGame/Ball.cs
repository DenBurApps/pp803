using System.Collections;
using UnityEngine;

namespace FootballGame
{
    public class Ball : MonoBehaviour
    {
        [SerializeField] private float _maxForce = 10f;
        [SerializeField] private float _currentForce = 2f;

        private bool _isDragging;
        private bool _inHole;
        private Transform _transform;
        private Rigidbody2D _rigidbody;
        private LineRenderer _lineRenderer;
        private Vector2 _dragStartPosition;
        private Vector2 _defaultPosition;
        private IEnumerator _movingCorouitne;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _lineRenderer = GetComponent<LineRenderer>();
            _transform = transform;
            _defaultPosition = _transform.position;
        }

        private IEnumerator Movement()
        {
            while (enabled)
            {
                if (Input.touchCount > 0 && IsReadyToDrag())
                {
                    Touch touch = Input.GetTouch(0);

                    if (touch.phase == TouchPhase.Began)
                    {
                        Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                        float distance = Vector2.Distance(_transform.position, touchPosition);

                        if (distance <= 0.5f)
                        {
                            DragStart(touchPosition);
                        }
                    }
                    else if (_isDragging && touch.phase == TouchPhase.Moved)
                    {
                        DragChange(touch);
                    }
                    else if (_isDragging && touch.phase == TouchPhase.Ended)
                    {
                        DragRelease(touch.position);
                    }
                }

                yield return null;
            }
        }

        public void SetDefault()
        {
            _transform.position = _defaultPosition;
            _rigidbody.velocity = Vector2.zero;
        }

        public void FreezePosition()
        {
            DisableMovement();
            _rigidbody.velocity = Vector2.zero;
        }

        public void EnableMovement()
        {
            DisableMovement();

            _movingCorouitne = Movement();
            StartCoroutine(_movingCorouitne);
        }

        public void DisableMovement()
        {
            if (_movingCorouitne != null)
            {
                StopCoroutine(_movingCorouitne);
                _movingCorouitne = null;
            }
        }

        private bool IsReadyToDrag()
        {
            return _rigidbody.velocity.magnitude <= 0.3f;
        }

        private void DragStart(Vector2 position)
        {
            _isDragging = true;
            _dragStartPosition = position;
            _lineRenderer.positionCount = 2;
        }

        private void DragChange(Touch touch)
        {
            Vector2 currentTouchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            Vector2 dragDirection = (_dragStartPosition + currentTouchPosition).normalized;
            float dragDistance = Vector2.Distance(_dragStartPosition, currentTouchPosition);

            Vector2 endLinePosition = (Vector2)_transform.position + dragDirection * Mathf.Min(dragDistance, _maxForce);
            _lineRenderer.SetPosition(0, _transform.position);
            _lineRenderer.SetPosition(1, endLinePosition);
        }

        private void DragRelease(Vector2 position)
        {
            _isDragging = false;
            /*_lineRenderer.enabled = false;*/

            Vector2 releasePosition = Camera.main.ScreenToWorldPoint(position);
            float distance = Vector2.Distance(_dragStartPosition, releasePosition);

            if (distance < 0.1f) return;

            Vector2 direction = (_dragStartPosition - releasePosition).normalized;
            _rigidbody.velocity = Vector2.ClampMagnitude(direction * _currentForce * distance, _maxForce);
        }
    }
}