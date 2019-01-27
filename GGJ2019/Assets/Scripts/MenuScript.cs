using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
    void Start()
    {
        Cursor.visible = true;

        if(GlobalLevelManager.instance != null)
            GlobalLevelManager.instance.isDead = false;
    }

    public void NextScene()
    {
        GetComponent<UnityEngine.UI.Button>().enabled = false;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
    }

    public void exitGame()
    {
        GetComponent<UnityEngine.UI.Button>().enabled = false;
        print("Quit");
        Application.Quit();
    }
}
