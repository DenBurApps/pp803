using System;
using System.Collections;
using UnityEngine;

namespace FootballGame
{
    public class Ball : MonoBehaviour
    {
        [SerializeField] private float _maxForce = 10f;
        [SerializeField] private float _currentForce = 2f;
        [SerializeField] private GameObject _bubblePrefab;
        [SerializeField] private int _bubbleCount = 5;
        [SerializeField] private float _maxBubbleSize = 0.1f;
        [SerializeField] private float _minBubbleSize = 0.01f;
        [SerializeField] private Vector2 _bubbleOffset = new Vector2(0.2f, 0.2f);
        [SerializeField] private Transform _startBubblePosition;

        private bool _isDragging;
        private bool _inHole;
        private Transform _transform;
        private Rigidbody2D _rigidbody;
        private LineRenderer _lineRenderer;
        private Vector2 _dragStartPosition;
        private Vector2 _defaultPosition;
        private IEnumerator _movingCoroutine;

        private GameObject[] _bubbles;

        public Rigidbody2D Rigidbody => _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _lineRenderer = GetComponent<LineRenderer>();
            _transform = transform;
            _defaultPosition = _transform.position;

            _bubbles = new GameObject[_bubbleCount];
            for (int i = 0; i < _bubbleCount; i++)
            {
                _bubbles[i] = Instantiate(_bubblePrefab, _startBubblePosition);
                _bubbles[i].SetActive(false);
            }
        }

        private void Start()
        {
            EnableMovement();
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

                        if (distance <= 2f)
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

        public void EnableMovement()
        {
            DisableMovement();

            _movingCoroutine = Movement();
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

        private bool IsReadyToDrag()
        {
            return _rigidbody.velocity.magnitude <= 0.3f;
        }

        private void DragStart(Vector2 position)
        {
            _isDragging = true;
            _dragStartPosition = position;
            _lineRenderer.positionCount = 2;

            foreach (var bubble in _bubbles)
            {
                bubble.SetActive(true);
            }
        }

        private void DragChange(Touch touch)
        {
            Vector2 currentTouchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            Vector2 dragDirection = (_dragStartPosition - currentTouchPosition).normalized;
            float dragDistance = Vector2.Distance(_dragStartPosition, currentTouchPosition);
            
            dragDistance = Mathf.Min(dragDistance, _maxForce);

            Vector2 endLinePosition = _dragStartPosition - dragDirection * dragDistance;

            _lineRenderer.SetPosition(0, _dragStartPosition);
            _lineRenderer.SetPosition(1, endLinePosition);

            for (int i = 0; i < _bubbleCount; i++)
            {
                float t = (float)i / (_bubbleCount - 1);
                
                Vector3 bubblePosition = Vector3.Lerp(_dragStartPosition, currentTouchPosition, t);
                
                bubblePosition += (Vector3)(_bubbleOffset * t);
                
                _bubbles[i].transform.position = bubblePosition;

                float size = Mathf.Lerp(_maxBubbleSize, _minBubbleSize, t);
                _bubbles[i].transform.localScale = Vector3.one * size;
            }
        }
        
        private void DragRelease(Vector2 position)
        {
            _isDragging = false;
            _lineRenderer.positionCount = 0;

            Vector2 releasePosition = Camera.main.ScreenToWorldPoint(position);
            float distance = Vector2.Distance(_dragStartPosition, releasePosition);

            if (distance < 0.1f) return;

            Vector2 direction = (_dragStartPosition - releasePosition).normalized;
            _rigidbody.velocity = Vector2.ClampMagnitude(-direction * _currentForce * distance, _maxForce);

            foreach (var bubble in _bubbles)
            {
                bubble.SetActive(false);
            }
        }
    }
}
