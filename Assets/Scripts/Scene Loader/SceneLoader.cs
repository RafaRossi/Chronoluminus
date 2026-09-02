using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>
{
    protected override bool IsPersistent => true;
    
    public async Task LoadSceneAsync(SceneID sceneId, UnityAction onLoadFinished)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneId.SceneName);

        if (asyncLoad != null)
        {
            asyncLoad.allowSceneActivation = false;

            while (asyncLoad.progress < 0.9f)
            {
                await Task.Yield();
            }

            asyncLoad.allowSceneActivation = true;

            while (!asyncLoad.isDone)
            {
                await Task.Yield();
            }
            
            onLoadFinished?.Invoke();
        }
    }
}
