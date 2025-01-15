using System;
using UnityEngine;

namespace Tennis
{
    [RequireComponent(typeof(Collider2D))]
    public class BallCatcher : MonoBehaviour
    {
        public event Action GotBall;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<TennisBall>(out TennisBall ball))
            {
                GotBall?.Invoke();
            }
        }
    }
}
