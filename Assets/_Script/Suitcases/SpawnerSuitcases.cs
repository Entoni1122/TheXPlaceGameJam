using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerSuitcases : MonoBehaviour
{
    [SerializeField] GameObject entity;
    [SerializeField] Transform spawnPos;
    [SerializeField] Transform targetPos;
    [SerializeField] float timeToSpawn;
    [SerializeField] List<EntetiesPerLevel> entetiesPerLevel;
    private Dictionary<int, int> _dictEntetiesPerLevel;

    int maxSpawnerCount;
    int spawnerCount;
    float spawnTimer;

    protected void Start()
    {
        spawnTimer = timeToSpawn;
        _dictEntetiesPerLevel = new Dictionary<int, int>();
        foreach (EntetiesPerLevel entity in entetiesPerLevel)
        {
            _dictEntetiesPerLevel[entity.level] = entity.count;
        }

        maxSpawnerCount = _dictEntetiesPerLevel[GameManager.instance.RoundCount + 1];
    }

    protected void Update()
    {
        if (spawnerCount < maxSpawnerCount)
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer > timeToSpawn)
            {
                spawnTimer = 0;
                spawnerCount++;
                SpawnEntitie();
            }
        }
    }

    protected void SpawnEntitie()
    {
        if (targetPos)
        {
            GameObject entiti = Instantiate(entity, spawnPos.position, Quaternion.identity, this.transform);
            entiti.GetComponent<MoveSuitcase>().Init(targetPos);
        }
    }
}

[System.Serializable]
public struct EntetiesPerLevel
{
    public int level;
    public int count;
}
