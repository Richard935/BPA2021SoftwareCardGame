using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class btnExitPurchase : MonoBehaviour
{
    public MainGame MainScript;
    public GameObject purchaseMenu;
    public GameObject[] hudExcludingResIndicator;
    public void onClick()
    {
        //Occurs when user wants to close the purchase menu. 
        MainScript.gameBoard.SetActive(true);
        foreach(var UIObject in hudExcludingResIndicator)
        {
            UIObject.SetActive(true);
        }

        purchaseMenu.SetActive(false);
    }
}
