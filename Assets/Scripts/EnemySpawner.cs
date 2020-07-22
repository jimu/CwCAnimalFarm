using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Pool
{
    private GameObject[] pool;
    private int nextAvailable = 0;

    public Pool(GameObject prefab, int length)
    {
        pool = new GameObject[length];

        for (int i = 0; i < length; ++i)
        {
            pool[i] = GameObject.Instantiate(prefab);
        }
    }

    public GameObject Get()
    {
        for (int count = pool.Length; count > 0; count--)
        {
            GameObject next = pool[nextAvailable];
            nextAvailable = (nextAvailable + 1) % pool.Length;
            if (!next.activeSelf)
                return next;
        }

        return null;
    }

    public GameObject Activate(Vector3 position, Quaternion rotation)
    {
        GameObject item = Get();
        if (item != null)
        {
            item.transform.position = position;
            item.transform.rotation = rotation;
            item.SetActive(true);
            return item;
        }

        return null;
    }

}

// needs to be a MonoBehavior to use StartCoroutine
public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    public GameObject[] enemyPrefabs;
    [SerializeField] int poolSize = 4;
    public float minDelay = 0.2f;
    public float maxDelay = 4f;

    List<Pool> enemyPool;

    bool spawning = false;

    /// <summary>
    /// creates pool for each enemy.
    /// 
    /// modifies enemyPool[]
    /// expects enemyPrefabs[] to be filled with valid Prefabs
    /// 
    /// </summary>
    private void Awake()
    {
        enemyPool = new List<Pool>();//[enemyPrefabs.Length];
        for(int i=0; i < enemyPrefabs.Length; ++i)
            enemyPool.Add(new Pool(enemyPrefabs[i], poolSize));
    }

    /// <summary>
    /// turns auto-spawning on/off.
    /// 
    /// when on, enemies are spawned every minDelay to maxDelay seconds
    /// 
    /// </summary>
    /// <param name="spawning">Spawns enemies if true</param>
    /// <returns>true if spawning is on</returns>
    
    private void OnDestroy()
    {
        spawning = false;
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnAnimalLoop());
    }

    public void StopSpawning()
    {
        spawning = false;
    }


    private IEnumerator SpawnAnimalLoop()
    {
        spawning = true;
        while (spawning)
        {
            float delay = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(delay);
            if (spawning)
                SpawnEnemy();
        }
    }

    public void SpawnEnemy(int type = -1)
    {
        int enemyType = type < 0 ? Random.Range(0, enemyPrefabs.Length) : type;

        float xmin, xmax, zmin, zmax;

        switch (Random.Range(0, 3))
        {
            case 0: xmin = -40f; xmax = 40f; zmin = zmax = 40f; break; // North
            case 1: xmin = xmax = -40f; zmin = 0f; zmax = 40f; break; // West
            default: xmin = xmax = 40f; zmin = 0f; zmax = 40f; break; // East
        }

        Vector3 position = new Vector3(Random.Range(xmin, xmax), 0, Random.Range(zmin, zmax));
        Vector3 target = new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 2f));
        Vector3 direction = Vector3.MoveTowards(Vector3.zero, position, 1f);
        Quaternion rotation = Quaternion.LookRotation(target - position);
        enemyPool[enemyType].Activate(position, rotation);
        Debug.Log("Instantiated " + enemyType + "\n P(" + position + ")\n D(" + direction + ") R(" + rotation + ")");
    }

}
