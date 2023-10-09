using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public int startSceneNumber;

    public void StartGame()
    {
        SceneManager.LoadScene(startSceneNumber, LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        Application.Quit(0);
    }
}
