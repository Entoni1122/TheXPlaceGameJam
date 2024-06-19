using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class Spawner : MonoBehaviour
{
    [SerializeField] EntityType entityTypeToSpawn;
    [SerializeField] GameObject entity;
    [SerializeField] Transform spawnPos;
    [SerializeField] Transform targetPos;
    [SerializeField] bool twoSpawnOn = false;
    [ShowIf("twoSpawnOn")] public Transform spawnPos2;
    [ShowIf("twoSpawnOn")] public Transform targetPos2;
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
                GameObject entiti = Instantiate(entity, spawnPos.position, Quaternion.identity);
                entiti.GetComponent<EntityProp>().Init(targetPos, entityTypeToSpawn, ent.color);
                entiti.SetActive(false);
                entitiesSpawned.Add(entiti);
            }
        }
        startSpawn = true;
    }


    [SerializeField] float raylength;
    bool flipFlop = true;
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

                    Transform start = flipFlop ? spawnPos : spawnPos2;
                    Transform target = flipFlop ? targetPos : targetPos2;

                    if (CanSpawn(start))
                    {
                        int randomIndex = UnityEngine.Random.Range(0, entitiesSpawned.Count - 1);
                        entitiesSpawned[randomIndex].transform.position = start.position;
                        entitiesSpawned[randomIndex].GetComponent<EntityProp>().UpdateTarget(target);
                        entitiesSpawned[randomIndex].SetActive(true);
                        entitiesSpawned.RemoveAt(randomIndex);
                        if (twoSpawnOn)
                        {
                            flipFlop = !flipFlop;
                        }
                    }
                }
            }
        }
        Debug.DrawRay(spawnPos.position + spawnPos.forward * raylength, -spawnPos.forward * raylength * 2);
    }
    bool CanSpawn(Transform start) => !Physics.Raycast(start.position + start.forward * raylength, -start.forward, raylength * 2);
}
