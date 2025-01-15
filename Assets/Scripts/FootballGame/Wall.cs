using System;
using UnityEngine;

namespace FootballGame
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Wall : MonoBehaviour
    {
        private BoxCollider2D _collider;

        private void Awake()
        {
            _collider = GetComponent<BoxCollider2D>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.TryGetComponent(out Ball ball))
            {
                ball.SetDefault();
            }
        }
    }
}