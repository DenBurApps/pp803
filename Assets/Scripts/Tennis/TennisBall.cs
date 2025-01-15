using UnityEngine;

namespace Tennis
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CircleCollider2D))]
    public class TennisBall : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _speedBoost = 2f;
        [SerializeField] private float _xShiftMultiplier = 0.1f;

        private Vector2 _defaultPosition;
        private Rigidbody2D _rigidbody;
        private Transform _transform;

        private void Awake()
        {
            _transform = transform;
            _defaultPosition = _transform.position;
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        public void Reset()
        {
            _rigidbody.velocity = Vector2.zero;
            _transform.position = _defaultPosition;
        }

        public void LaunchBall()
        {
            float yDirection = Random.Range(0.5f, 1f) * (Random.value > 0.5f ? 1 : -1);
            Vector2 initialForce = new Vector2(_transform.position.x, yDirection).normalized * _speed;
            _rigidbody.velocity = initialForce;
        }

        public void ThrowBack()
        {
            float xOffset = Random.Range(-_xShiftMultiplier, _xShiftMultiplier);

            Vector2 newVelocity = new Vector2(xOffset, 1f).normalized * _speedBoost;

            _rigidbody.velocity = newVelocity;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out Opponent opponent))
            {
                Vector2 currentVelocity = _rigidbody.velocity;
                float xOffset = (transform.position.x - collision.transform.position.x) * _xShiftMultiplier;
                Vector2 boostedVelocity = new Vector2(
                    currentVelocity.x + xOffset,
                    Mathf.Abs(currentVelocity.y)
                ).normalized * (_speed + _speedBoost);
                _rigidbody.velocity = boostedVelocity;
            }
        }
    }
}