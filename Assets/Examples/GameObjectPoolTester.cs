using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PoolTools;

public class GameObjectPoolTester : MonoBehaviour
{
    public PoolableFirework FireworkPrefab;
    public PoolableNumber NumberPrefab;
    public bool AutoResize = false;
    public float SpawnRate = 10;

	private NumericStringsPool _numStringsPool;
    private GameObjectPool<PoolableFirework> _fireworksPool;
    private GameObjectPool<PoolableNumber> _numbersPool;
    private float _accumulatedDeltaTime;

    // Start is called before the first frame update
    void Start()
    {
		_numStringsPool = new NumericStringsPool();
		_numStringsPool.CreateIntStringsPool(0, 9999, "");
	
        _fireworksPool = new GameObjectPool<PoolableFirework>();
        _fireworksPool.CreatePool(50, FireworkPrefab, true, AutoResize, 1.5f);
		
        _numbersPool = new GameObjectPool<PoolableNumber>();
        _numbersPool.CreatePool(50, NumberPrefab, true, AutoResize, 1.5f);
    } 

    void Update()
    {
        _accumulatedDeltaTime += Time.deltaTime;
        float spawnDelta = (1f / SpawnRate);

        while (_accumulatedDeltaTime >= spawnDelta)
        {
            if (_fireworksPool.SpawnPoolable(out PoolableFirework f))
            {
                f.Transform.position = Random.insideUnitSphere * UnityEngine.Random.Range(0f, 10f);
            }
            if (_numbersPool.SpawnPoolable(out PoolableNumber n))
            {
                n.Transform.position = Random.insideUnitSphere * UnityEngine.Random.Range(0f, 10f);
				
				if(_numStringsPool.GetIntString(UnityEngine.Random.Range(0, 9999), out string str))
				{
					n.TextMesh.text = str;
				}
            }

            _accumulatedDeltaTime -= spawnDelta;
        }
    }
}
