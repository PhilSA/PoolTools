using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PoolTools
{
    public class GameObjectPool<T> : IPool where T : Component, IPoolable
    {
        public bool ManagePoolablesActivation;
        public bool ResizeIfNeeded;
        public float ResizeFactor = 1.5f;

        private T _poolablePrefab;
        private T[] _pool;
        private int[] _availableIndexes;
        private int _availableIndexesCount;

        public void CreatePool(int capacity, T prefab, bool managePoolablesActivation, bool resizeIfNeeded, float resizeFactor)
        {
            _poolablePrefab = prefab;
            _pool = new T[capacity];
            _availableIndexes = new int[capacity];
            _availableIndexesCount = 0;

            ManagePoolablesActivation = managePoolablesActivation;
            ResizeIfNeeded = resizeIfNeeded;
            ResizeFactor = resizeFactor;

            for (int i = 0; i < capacity; i++)
            {
                CreateNewPoolableInstance(i);
            }
        }

        public void DestroyPool()
        {
            for (int i = 0; i < _pool.Length; i++)
            {
                DestroyPoolable(i);
            }

            _pool = null;
        }

        public bool SpawnPoolable(out T poolable)
        {
            if (ResizeIfNeeded && _availableIndexesCount <= 0)
            {
                ResizePool(Mathf.RoundToInt(_availableIndexes.Length * ResizeFactor));
            }

            if (_availableIndexesCount > 0)
            {
                poolable = _pool[_availableIndexes[0]];

                // update available indexes
                _availableIndexes[0] = _availableIndexes[_availableIndexesCount - 1];
                _availableIndexes[_availableIndexesCount - 1] = -1;
                _availableIndexesCount--;

                // Activation
                if (ManagePoolablesActivation)
                {
                    poolable.gameObject.SetActive(true);
                }
                poolable.OnSpawnFromPool();
                poolable.IsPoolableActive = true;

                return true;
            }

            poolable = null;
            return false;
        }

        public bool RecyclePoolable(int atIndex)
        {
            T poolable = _pool[atIndex];
            if (poolable.IsPoolableActive)
            {
                poolable.OnRecycleToPool();
                if (ManagePoolablesActivation)
                {
                    poolable.gameObject.SetActive(false);
                }
                poolable.IsPoolableActive = false;

                // update available indexes
                _availableIndexes[_availableIndexesCount] = poolable.IndexInPool;
                _availableIndexesCount++;

                return true;
            }

            return false;
        }

        public void ResizePool(int newSize)
        {
            int lengthBefore = _pool.Length;
            if (_pool != null && newSize != lengthBefore)
            {
                // Properly dispose instances if reducing size
                if(newSize < lengthBefore)
                {
                    for (int i = newSize; i < lengthBefore; i++)
                    {
                        DestroyPoolable(i);
                    }
                }

                System.Array.Resize(ref _pool, newSize);
                _availableIndexes = new int[newSize];

                // Create new instances
                if (newSize > lengthBefore)
                {
                    for (int i = lengthBefore; i < newSize; i++)
                    {
                        CreateNewPoolableInstance(i);
                    }
                }

                // TODO: no need to recompute if we're growing
                // Recompute available indexes
                int addedCounter = 0;
                for (int i = 0; i < _pool.Length; i++)
                {
                    if(!_pool[i].IsPoolableActive)
                    {
                        _availableIndexes[addedCounter] = _pool[i].IndexInPool;
                        addedCounter++;
                    }
                }
            }
        }

        private void CreateNewPoolableInstance(int atIndex)
        {
            T newInstance = GameObject.Instantiate(_poolablePrefab);
            newInstance.IndexInPool = atIndex;
            newInstance.IsPoolableActive = true;
            newInstance.Pool = this;
            _pool[atIndex] = newInstance;

            RecyclePoolable(atIndex);
        }

        private void DestroyPoolable(int atIndex)
        {
            RecyclePoolable(atIndex);
            GameObject.Destroy(_pool[atIndex].gameObject);
        }
    }
}