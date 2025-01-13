using System;
using UnityEngine;

namespace FootballGame
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Wall : MonoBehaviour
    {
        private BoxCollider2D _collider;
        
        public event Action Hit;

        private void Awake()
        {
            _collider = GetComponent<BoxCollider2D>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.TryGetComponent(out Ball ball))
            {
                Hit?.Invoke();
            }
        }

        public void DisableCollision()
        {
            _collider.enabled = false;
        }

        public void EnableCollision()
        {
            _collider.enabled = true;
        }
    }
}