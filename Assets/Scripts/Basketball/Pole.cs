using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Basketball
{
    public class Pole : MonoBehaviour
    {
        [SerializeField] private float _minYPosition;
        [SerializeField] private float _maxYPosition;

        private Transform _transform;

        private void Awake()
        {
            _transform = transform;
        }

        public void SetRandomPosition()
        {
            var randomYPosition = Random.Range(_minYPosition, _maxYPosition);

            _transform.position = new Vector2(_transform.position.x, randomYPosition);
        }
    }
}
