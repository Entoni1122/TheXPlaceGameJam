using System.Collections;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;


public class SceneLoaderScript : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(LoadMainScene());
    }

    IEnumerator LoadMainScene()
    {
        yield return new WaitUntil(() => NetworkManager.Singleton != null);
        SceneManager.LoadScene("Lobby");
    }
}