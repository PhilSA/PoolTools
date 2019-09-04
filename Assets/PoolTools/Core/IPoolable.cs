using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PoolTools
{
    public interface IPoolable
    {
        IPool Pool { get; set; }
        bool IsPoolableActive { get; set; }
        int IndexInPool { get; set; }

        void OnSpawnFromPool();
        void OnRecycleToPool();
    }
}