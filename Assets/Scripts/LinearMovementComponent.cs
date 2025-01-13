using System.Collections;
using UnityEngine;

public class LinearMovementComponent : MonoBehaviour
{
    [SerializeField] private float _maxXPosition;
    [SerializeField] private float _minXPosition;
    [SerializeField] private float _movingSpeed;

    private Vector2 _movingDirection;
    private Transform _transform;
    private Coroutine _movementCoroutine;

    private void Awake()
    {
        _transform = transform;
    }

    public void SetMovingDirection(Vector2 direction)
    {
        _movingDirection = direction.normalized;
    }

    public void EnableMovement()
    {
        if (_movementCoroutine != null)
        {
            DisableMovement();
        }

        _movementCoroutine = StartCoroutine(Move());
    }

    public void DisableMovement()
    {
        if (_movementCoroutine != null)
        {
            StopCoroutine(_movementCoroutine);
            _movementCoroutine = null;
        }
    }

    private IEnumerator Move()
    {
        while (true)
        {
            Vector2 targetPosition = new Vector2(
                _transform.position.x + _movingDirection.x * _movingSpeed * Time.deltaTime,
                _transform.position.y
            );

            _transform.position = targetPosition;

            if (_transform.position.x >= _maxXPosition)
            {
                _movingDirection = Vector2.left;
            }
            else if (_transform.position.x <= _minXPosition)
            {
                _movingDirection = Vector2.right;
            }

            yield return null;
        }
    }
}