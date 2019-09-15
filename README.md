
**Creating a pool**

```C#
GameObjectPool<MyPoolable> myPool = new GameObjectPool<MyPoolable>();
myPool.CreatePool(50, MyPrefab, true, AutoResize, 1.5f);
```

**Spawning from a pool**

```C#
if (myPool.SpawnPoolable(out MyPoolable f))
{
    // Spawn successful
}
```

**Recycling to pool**

```C#
myPoolable.Pool.RecyclePoolable(myPoolable.IndexInPool);
```
