using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerBase : MonoBehaviour
{
    [SerializeField] protected int numberOfEntitiesToSpawn;
    [SerializeField] protected Transform positionToSpawn;
    [SerializeField] protected GameObject entitiPrefab;
    [SerializeField] protected float timerToSpawn;
    
    protected int entitiCounter;
    protected float timer;

    protected virtual void Start()
    {
        timer = timerToSpawn;
    }


    protected virtual void Update()
    {
        if (entitiCounter >= numberOfEntitiesToSpawn)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                SpawnEntitie();
            }
        }
    }


    protected virtual void SpawnEntitie()
    {
        GameObject entiti = Instantiate(entitiPrefab, positionToSpawn);
    }
}
