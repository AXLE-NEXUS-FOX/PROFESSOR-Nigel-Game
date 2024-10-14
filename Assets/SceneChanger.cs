using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void Exit()
    {
        Application.Quit();
    }

    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        Debug.Log("Current Scene Name: " + currentScene.name);
    }

}


