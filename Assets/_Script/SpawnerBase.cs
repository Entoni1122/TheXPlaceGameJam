using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerBase : MonoBehaviour
{
    [SerializeField] int numberOfEntitiesToSpawn;
    [SerializeField] Transform positionToSpawn;
    [SerializeField] GameObject entitiPrefab;
    [SerializeField] float timerToSpawn;
    
    int entitiCounter;
    float timer;

    void Start()
    {
        timer = timerToSpawn;
    }

    void Update()
    {
        if (numberOfEntitiesToSpawn <= entitiCounter)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                SpawnEntitie();
            }
        }
    }


    void SpawnEntitie()
    {
        GameObject entiti = Instantiate(entitiPrefab, positionToSpawn);
    }
}
