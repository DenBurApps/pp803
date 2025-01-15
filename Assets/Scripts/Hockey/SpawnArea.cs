using UnityEngine;

namespace Hockey
{
    public class SpawnArea : MonoBehaviour
    {
        [SerializeField] private float _yPosition;
        [SerializeField] private float _minX;
        [SerializeField] private float _maxX;

        public Vector2 GetPositionToSpawn()
        {
            return new Vector2(Random.Range(_minX, _maxX), _yPosition);
        }
    }
}
