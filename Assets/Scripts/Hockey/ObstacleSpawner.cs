using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hockey
{
    public class ObstacleSpawner : ObjectPool<Obstacle>
    {
        [SerializeField] private float _spawnInterval;
        [SerializeField] private Obstacle _prefab;
        [SerializeField] private SpawnArea _spawnArea;
        [SerializeField] private int _poolCapacity;
        [SerializeField] private SpriteHolder _spriteHolder;

        private List<Obstacle> _spawnedObjects = new List<Obstacle>();

        private IEnumerator _spawningCoroutine;
        
        private void Awake()
        {
            for (int i = 0; i <= _poolCapacity; i++)
            {
                Initalize(_prefab);
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
            var inteval = new WaitForSeconds(_spawnInterval);
            
            while (true)
            {
                Spawn();

                yield return inteval;
            }
        }

        private void Spawn()
        {
            if (ActiveObjects.Count >= _poolCapacity)
                return;

            Obstacle prefabToSpawn = _prefab;

            if (TryGetObject(out Obstacle @object, prefabToSpawn))
            {
                @object.transform.position = _spawnArea.GetPositionToSpawn();
                @object.AssignSprite(_spriteHolder.GetRandomSprite());
                @object.EnableMovement();
                
                _spawnedObjects.Add(@object);
            }
        }

        public void ReturnToPool(Obstacle @object)
        {
            if (@object == null)
                return;
            
            @object.DisableMovement();
            PutObject(@object);

            if (_spawnedObjects.Contains(@object))
                _spawnedObjects.Remove(@object);
        }

        public void ReturnAllObjectsToPool()
        {
            if (_spawnedObjects.Count <= 0)
                return;

            List<Obstacle> objectsToReturn = new List<Obstacle>(_spawnedObjects);
            foreach (var @object in objectsToReturn)
            {
                ReturnToPool(@object);
            }
        }
    }
}