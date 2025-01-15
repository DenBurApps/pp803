using System;
using UnityEngine;

namespace Hockey
{
    public class ObstacleReturner : MonoBehaviour
    {
        [SerializeField] private ObstacleSpawner _spawner;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Obstacle bullet))
            {
                _spawner.ReturnToPool(bullet);
            }
        }
    }
}
