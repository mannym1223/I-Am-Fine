using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneUtils : MonoBehaviour
{
    public void ReloadCurrentScene(float delay = 0.5f)
    {
        StartCoroutine(BeginReloadingScene(delay));
    }

    public void ExitGame(float delay = 0.5f)
    {
        StartCoroutine(BeginExiting(delay));
	}

    private IEnumerator BeginReloadingScene(float delay) 
    {
		yield return new WaitForSeconds(delay);
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

    private IEnumerator BeginExiting(float delay)
    {
        yield return new WaitForSeconds(delay);
#if (UNITY_EDITOR)
		EditorApplication.ExitPlaymode();
#elif (UNITY_STANDALONE)
    Application.Quit();
#elif (UNITY_WEBGL)
    Application.OpenURL("https://itch.io/jam/brackeys-13/rate/3349706");
#endif
    }
}
