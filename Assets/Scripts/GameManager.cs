using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class GameManager : MonoBehaviour
{

    public GameObject[] animalPrefabs;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnAnimals());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SpawnAnimal();
        }
    }


    IEnumerator SpawnAnimals()
    {
        while (true)
        {
            float delay = Random.Range(0.2f, 5f);
            yield return new WaitForSeconds(delay);
            SpawnAnimal();
        }
    }
    void SpawnAnimal()
    {
        int animalIndex = Random.Range(0, animalPrefabs.Length);
        Instantiate(animalPrefabs[animalIndex], new Vector3(Random.Range(-20f, 20f), 0, 20f),
          animalPrefabs[animalIndex].transform.rotation);
    }
}
