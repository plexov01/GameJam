using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public enum Scene
    {
        Main,
        Loading,
        Level1,
    }


    private static Scene targetScene;

    public static void LoadScene(Scene targetSceneName)
    {
        targetScene = targetSceneName;
        
        SceneManager.LoadScene(Scene.Loading.ToString());
    }

    public static void SceneLoaderCallback()
    {
        SceneManager.LoadScene(targetScene.ToString());
        
    }


}