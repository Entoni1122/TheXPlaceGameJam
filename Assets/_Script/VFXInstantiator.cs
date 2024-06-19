using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXInstantiator : MonoBehaviour
{
    [SerializeField] public Transform placeToPSawn;
    [SerializeField] public GameObject Prefab;

    public void SpawnDust()
    {
        Instantiate(Prefab, placeToPSawn);
    }
}
