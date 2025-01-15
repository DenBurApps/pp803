using System;
using FootballGame;
using UnityEngine;

namespace Basketball
{
    [RequireComponent(typeof(Collider2D))]
    public class BasketballNet : MonoBehaviour
    {
        [SerializeField] private AudioSource _hitSound;
        
        public event Action BallCatched;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Ball ball))
            {
                if (ball.Rigidbody != null)
                {
                    if (ball.Rigidbody.velocity.y < 0)
                    {
                        _hitSound.Play();
                        BallCatched?.Invoke();
                    }
                }
            }
        }
    }
}
