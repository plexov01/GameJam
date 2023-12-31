using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public enum Scene
    {
        Menu,
        Loading,
        Cutscene,
        TowerDefense,
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