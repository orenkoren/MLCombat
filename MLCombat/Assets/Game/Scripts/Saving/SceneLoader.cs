using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MiddleAges.Saving
{
    public class SceneLoader : MonoBehaviour
    {
        public static AsyncOperation loadMainScene;
        private IEnumerator LoadScene(string sceneName, Action onLoad = null)
        {
            loadMainScene = SceneManager.LoadSceneAsync(sceneName);
            while (!loadMainScene.isDone)
            {
                yield return null;
            }
            if (onLoad != null)
                onLoad();
            onLoad?.Invoke();

        }

        public void StartLoadingScene(string sceneName, Action onLoad = null)
        {
            StartCoroutine(LoadScene(sceneName, onLoad));
        }
    }
}