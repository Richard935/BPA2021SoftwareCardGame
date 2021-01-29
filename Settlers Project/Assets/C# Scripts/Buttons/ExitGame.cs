using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitGame : MonoBehaviour
{
    public void OnClick()
    {
        //Load start menu and doesn't destroy music.
        PlayerPrefs.SetInt("desMusic", 1);
        SceneManager.LoadScene(sceneName: "Main Menu");
    }
}