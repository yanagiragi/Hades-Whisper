using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
    public void NextScene()
    {

        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("MainScene");
    }

    public void exitGame()
    {
        print("Quit");
        Application.Quit();
    }
}
