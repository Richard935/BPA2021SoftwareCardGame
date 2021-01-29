using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseGame : MonoBehaviour
{
    public GameObject PauseScreen;
    public MainGame MainScript;

    //Set in unity inspector.
    public bool toPauseMenu;
    public void onClick()
    {
        //Opens the pause menu.
        PauseScreen.SetActive(toPauseMenu);
        MainScript.gameBoard.SetActive(!toPauseMenu);
        MainScript.hud.SetActive(!toPauseMenu);
    }
}
