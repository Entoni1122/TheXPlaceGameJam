using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXInstantiator : MonoBehaviour
{
    [SerializeField] Transform placeToPSawn;
    [SerializeField] GameObject dustPrefab;

    public void SpawnDust()
    {
        Instantiate(dustPrefab, placeToPSawn);
    }
}
