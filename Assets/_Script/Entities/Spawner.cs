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

    private float spawnTimer;

    private List<GameObject> entitiesSpawned = new List<GameObject>();
    private bool startSpawn;

    private void Awake()
    {
        GameManager.OnRoundStart += StartRound;
    }

    public void StartRound()
    {
        entitiesSpawned.Clear();

        spawnTimer = timeToSpawn;
        List<EntityInfo> list = GameManager.instance.GetEnityInfoToSpawn(entityTypeToSpawn);
        foreach (EntityInfo ent in list)
        {
            for (int i = 0; i < ent.count; i++)
            {
                GameObject entiti = Instantiate(entity, spawnPos.position, Quaternion.identity, this.transform);
                entiti.GetComponent<EntityProp>().Init(targetPos, entityTypeToSpawn, ent.color);
                entiti.SetActive(false);
                entitiesSpawned.Add(entiti);
                GameManager.OnLoseRound += () =>
                {
                    if (entiti)
                    {
                        Destroy(entiti);
                    }
                };
            }
        }
        startSpawn = true;
    }


    protected void Update()
    {
        if (startSpawn)
        {
            if (entitiesSpawned.Count > 0)
            {
                spawnTimer += Time.deltaTime;
                if (spawnTimer > timeToSpawn)
                {
                    spawnTimer = 0;
                    int randomIndex = Random.Range(0, entitiesSpawned.Count - 1);
                    entitiesSpawned[randomIndex].SetActive(true);
                    entitiesSpawned.RemoveAt(randomIndex);
                }
            }
        }
    }
}
