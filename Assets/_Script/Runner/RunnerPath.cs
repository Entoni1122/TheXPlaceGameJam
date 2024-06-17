using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerPath : MonoBehaviour
{
    [SerializeField] List<Transform> path;
     static List<Transform> pathStatic;

    private void Awake()
    {
        pathStatic = path;
    }

    public static int GetPathCount()
    {
        return pathStatic.Count;  
    }

    public static Transform GetPathAtIndex(int index)
    {
        return pathStatic[index];
    }
}
