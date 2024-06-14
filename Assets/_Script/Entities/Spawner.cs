using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] EntityType entityTypeToSpawn;
    [SerializeField] GameObject entity;
    [SerializeField] Transform spawnPos;
    [SerializeField] Transform targetPos;
    [SerializeField] float timeToSpawn;


    int maxSpawnerCount;
    int spawnerCount;
    float spawnTimer;

    protected void Start()
    {
        spawnTimer = timeToSpawn;
        maxSpawnerCount = GameManager.instance.GetCountPerRoundByEntityType(entityTypeToSpawn);
    }

    protected void Update()
    {
        if (spawnerCount < maxSpawnerCount)
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer > timeToSpawn)
            {
                spawnTimer = 0;
                SpawnEntitie();
            }
        }
    }

    protected void SpawnEntitie()
    {
        GameObject entiti = Instantiate(entity, spawnPos.position, Quaternion.identity, this.transform);
        int ID = (spawnerCount + GameManager.instance.RoundCount) % 3;
        entiti.GetComponent<EntityProp>().Init(targetPos, entityTypeToSpawn, ID);
        spawnerCount++;
    }
}
