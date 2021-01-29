using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NameEntered : MonoBehaviour
{
    public MainGame MainScript;
    public InputField inputName;
    public Text messageBox;
    
    bool timerOn;
    float stopTimeAt = 5.0f;
    float currentTime;

    //Occurs when user enters name and confirms.
    public void OnClick()
    {
        timerOn = false;


        if (inputName.text != "")
        {
            //Puts winner on the score board.
            MainScript.enterName.SetActive(false);
            MainScript.logEvents("The player that won placed on the scoreboard.");
            MainScript.placeWinner(inputName.text);
        }
        else
        {
            //User didn't enter name.
            messageBox.text = "You didn't enter a name.";
            messageBox.gameObject.SetActive(true);
            currentTime = 0;
            timerOn = true;
        }
    }

    //Timer
    public void Update()
    {
        if (timerOn)
        {
            currentTime += 1 * Time.deltaTime;
            if(currentTime >= stopTimeAt)
            {
                //Hides the message box when timer ends.
                timerOn = false;
                messageBox.gameObject.SetActive(false);
            }
        }
    }
}
