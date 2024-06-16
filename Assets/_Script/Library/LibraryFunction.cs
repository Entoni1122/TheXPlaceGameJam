using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class LibraryFunction
{
    public static float LengthXZ(Vector3 _inVector)
    {
        Vector3 _convertedVector = new Vector3(_inVector.x, 0, _inVector.z);
        return _convertedVector.magnitude;
    }

    private static UnrealPlayerController _unrealPlayerController;
    public static void SetUnrealPlayerController(UnrealPlayerController _InController)
    {
        _unrealPlayerController = _InController;
    }
    public static UnrealPlayerController GetUnrealPlayerController()
    {
        return _unrealPlayerController;
    }
}
