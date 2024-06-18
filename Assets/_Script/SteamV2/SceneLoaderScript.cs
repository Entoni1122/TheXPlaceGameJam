using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoaderScript : MonoBehaviour
{
    [SerializeField] string sceneToLoad;
    public void ButtonPressed()
    {
        StartCoroutine(LoadYourAsyncScene());
    }
    IEnumerator LoadYourAsyncScene()
    {
        yield return new WaitForSeconds(3f);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}