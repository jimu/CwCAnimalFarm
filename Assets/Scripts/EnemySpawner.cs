using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// needs to be a MonoBehavior to use StartCoroutine
public class EnemySpawner : MonoBehaviour
{
    const float NORMAL_MIN_DELAY = 0.2f;
    const float NORMAL_MAX_DELAY = 4f;
    const float HYPER_MAX_DELAY  = 0.5f;
    const float HYPER_MULTIPLIER = 1.2f;
    const float HYPER_MAX_MULTIPIER = 10f;


    [SerializeField]
    public GameObject[] enemyPrefabs;
    [SerializeField] int poolSize = 4;
    public float minDelay = NORMAL_MIN_DELAY;
    public float maxDelay = NORMAL_MAX_DELAY;

    List<Pool> enemyPool;

    private float hyperSpeedMultiplier = 1f;

    bool spawning = false;

    public void SetMurderMode(bool murder = true)
    {
        maxDelay = murder ? HYPER_MAX_DELAY : NORMAL_MAX_DELAY;
        hyperSpeedMultiplier = murder ? HYPER_MULTIPLIER : 1f;
    }

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
        SetMurderMode(false);
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
        GameObject enemy = enemyPool[enemyType].Activate(position, rotation);
        // Debug.Log("Instantiated " + enemyType + "\n P(" + position + ")\n D(" + direction + ") R(" + rotation + ")");

        if (maxDelay == HYPER_MAX_DELAY && enemy != null)
        {
            enemy.GetComponent<Enemy>().speed *= hyperSpeedMultiplier;
            hyperSpeedMultiplier = Mathf.Min(hyperSpeedMultiplier * HYPER_MULTIPLIER, HYPER_MAX_MULTIPIER) ;
        }
    }

}
