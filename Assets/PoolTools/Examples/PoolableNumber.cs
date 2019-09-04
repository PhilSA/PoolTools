using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PoolTools;

public class PoolableNumber : MonoBehaviour, IPoolable
{
    public float LifeTime = 1f;
    public TextMesh TextMesh;

    public IPool Pool { get; set; }
    public bool IsPoolableActive { get; set; }
    public int IndexInPool { get; set; }

    public Transform Transform { get; set; }

    private float _spawnTime;

    public void OnRecycleToPool()
    {
    }

    public void OnSpawnFromPool()
    {
        _spawnTime = Time.time;
    }

    private void Awake()
    {
        Transform = this.transform;
    }

    public void Update()
    {
        if (Time.time > _spawnTime + LifeTime)
        {
            Pool.RecyclePoolable(IndexInPool);
        }
    }
}
