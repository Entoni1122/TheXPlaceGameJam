using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AutomateStartSceneInEditor : MonoBehaviour
{
    void Awake()
    {
        if (AudioManager.Instance == null)
        {
            SceneManager.LoadScene("GameStart");
        }
    }
}
