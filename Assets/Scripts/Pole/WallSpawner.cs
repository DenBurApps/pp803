using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pole
{
    public class WallSpawner : ObjectPool<Wall>
    {
        [SerializeField] private float _spawnInterval;
        [SerializeField] private Wall[] _prefabs;
        [SerializeField] private Vector2 _spawnPosition;
        [SerializeField] private int _poolCapacity;

        private List<Wall> _spawnedObjects = new();
        private IEnumerator _spawningCoroutine;

        private void Awake()
        {
            if (_prefabs == null || _prefabs.Length == 0)
                return;

            for (int i = 0; i < _poolCapacity; i++)
            {
                var randomIndex = Random.Range(0, _prefabs.Length);
                Initalize(_prefabs[randomIndex]);
            }
        }

        public void StartSpawning()
        {
            StopSpawning();

            _spawningCoroutine = SpawningCoroutine();
            StartCoroutine(_spawningCoroutine);
        }

        public void StopSpawning()
        {
            if (_spawningCoroutine != null)
            {
                StopCoroutine(_spawningCoroutine);
                _spawningCoroutine = null;
            }
        }

        private IEnumerator SpawningCoroutine()
        {
            var interval = new WaitForSeconds(_spawnInterval);

            while (true)
            {
                SpawnWall();
                yield return interval;
            }
        }

        private void SpawnWall()
        {
            if (_spawnedObjects.Count >= _poolCapacity)
                return;

            var randomIndex = Random.Range(0, _prefabs.Length);
            Wall prefabToSpawn = _prefabs[randomIndex];

            if (TryGetObject(out Wall wallObject, prefabToSpawn))
            {
                wallObject.transform.position = _spawnPosition;
                wallObject.EnableMovement();

                _spawnedObjects.Add(wallObject);
            }
        }

        public void ReturnToPool(Wall wallObject)
        {
            if (wallObject == null)
                return;

            wallObject.DisableMovement();
            PutObject(wallObject);

            _spawnedObjects.Remove(wallObject);
        }

        public void ReturnAllObjectsToPool()
        {
            for (int i = _spawnedObjects.Count - 1; i >= 0; i--)
            {
                ReturnToPool(_spawnedObjects[i]);
            }
        }
    }
}