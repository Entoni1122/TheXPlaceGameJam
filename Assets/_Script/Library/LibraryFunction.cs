using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LibraryFunction
{
    public static float LengthXZ(Vector3 _inVector)
    {
        Vector3 _convertedVector = new Vector3(_inVector.x, 0, _inVector.z);
        return _convertedVector.magnitude;
    }
}
