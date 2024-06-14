using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointSpawner : MonoBehaviour
{
    [SerializeField] Point pointA;
    [SerializeField] GameObject entityPrefab;

    void Update()
    {
        if (!pointA.isOccupied)
        {
            SpawnEntityAtPoint();
        }
    }

    void SpawnEntityAtPoint()
    {
        GameObject entity = Instantiate(entityPrefab, pointA.transform.position, Quaternion.identity);
        var interactableObj = entity.GetComponent<BaseInteractableObj>();
        if (interactableObj != null)
        {
            //interactableObj.SetCurrentPoint(pointA);
        }
        entity.transform.SetParent(pointA.transform);
        pointA.isOccupied = true;
    }
}