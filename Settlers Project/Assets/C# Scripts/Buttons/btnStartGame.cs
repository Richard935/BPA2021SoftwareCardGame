using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class btnStartGame : MonoBehaviour
{
    public void OnClick()
    {
        //Load game scene and don't destroy music.
        DontDestroyOnLoad(GameObject.Find("Music"));
        SceneManager.LoadScene (sceneName: "Game");
    }
}
