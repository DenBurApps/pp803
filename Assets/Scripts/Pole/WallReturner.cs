using UnityEngine;

namespace Pole
{
    public class WallReturner : MonoBehaviour
    {
        [SerializeField] private WallSpawner _spawner;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Wall bullet))
            {
                _spawner.ReturnToPool(bullet);
            }
        }
    }
}
