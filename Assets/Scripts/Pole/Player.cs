using System;
using UnityEngine;

namespace Pole
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private float _returnSpeed = 2f;
        [SerializeField] private float _gravity = 9.8f;
        [SerializeField] private Transform _groundPosition;
        [SerializeField] private InputDetector _inputDetector;
        [SerializeField] private PlayerAnimator _playerAnimator;
        [SerializeField] private GameObject _shadow;

        private bool _isLaunching = false;
        private float _verticalVelocity = 0f;

        private Vector3 _startPosition;
        private Transform _transform;

        public event Action HitWall;

        private void Awake()
        {
            _transform = transform;
        }

        private void OnEnable()
        {
            _inputDetector.ForceSelected += Launch;
        }

        private void OnDisable()
        {
            _inputDetector.ForceSelected -= Launch;
        }

        private void Start()
        {
            if (_groundPosition == null)
            {
                _startPosition = _transform.position;
            }
            else
            {
                _startPosition = _groundPosition.position;
            }
        }

        private void Update()
        {
            if (_isLaunching)
            {
                _shadow.SetActive(false);
                _verticalVelocity -= _gravity * Time.deltaTime;
                _transform.position += new Vector3(0, _verticalVelocity * Time.deltaTime, 0);
                _playerAnimator.SetFlying();

                if (_transform.position.y <= _startPosition.y)
                {
                    _transform.position = _startPosition;
                    _playerAnimator.SetRunning();
                    
                    _isLaunching = false;
                }
            }
            else
            {
                _shadow.SetActive(true);
                _transform.position = Vector3.Lerp(_transform.position, _startPosition, _returnSpeed * Time.deltaTime);
            }
        }

        private void Launch(float force)
        {
            if (!_isLaunching)
            {
                _isLaunching = true;
                _playerAnimator.SetJumping();
                _verticalVelocity = force;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Wall wall))
            {
                HitWall?.Invoke();
            }
        }
    }
}
