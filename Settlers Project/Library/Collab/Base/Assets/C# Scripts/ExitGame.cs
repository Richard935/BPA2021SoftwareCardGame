﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitGame : MonoBehaviour
{
    public void OnClick()
    {
        SceneManager.LoadScene(sceneName: "Main Menu");
    }
}