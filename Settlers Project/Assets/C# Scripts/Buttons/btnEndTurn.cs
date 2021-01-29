using UnityEngine;

public class btnEndTurn : MonoBehaviour
{
    public MainGame MainScript;
    public SetupPhase SetupScript;
    public SaveLoadSystem LoadSave;
    public btnRoll RollScript;

    //Occurs on End Turn button click.
    public void OnClick()
    {
        //Checks if setup phase is complete or for last turn in setup phase. If not both then move to next player..
        if (SetupScript.setupPhaseFinished == false)
        {
            SetupScript.setupPhase();
        }
        else if (SetupScript.forLastEndTurnClickDuringSetup == false && SetupScript.setupPhaseFinished == true)
        {
        //Completes the setup phase and moves the game to a fully playable state.
            for (int i = 0; i < SetupScript.allUIObjectsThatGetDisabled.Length; i++)
            {
                SetupScript.allUIObjectsThatGetDisabled[i].SetActive(true);
            }
            SetupScript.forLastEndTurnClickDuringSetup = true;
            MainScript.btnEndTurn.gameObject.SetActive(false);
        }
        else
        {
        //Moves to the next player.
            int presentPlayer = MainScript.playerInfo.currentPlayer;
            presentPlayer++;
            MainScript.playerInfo.currentPlayer = presentPlayer;

            MainScript.btnEndTurn.gameObject.SetActive(false);
            MainScript.btnRollDice.gameObject.SetActive(true);
            MainScript.setIndicators();

            RollScript.imgBoxesForDiceImages[0].gameObject.SetActive(false);
            RollScript.imgBoxesForDiceImages[1].gameObject.SetActive(false);

            MainScript.playerInfo.turnsTaken[MainScript.playerInfo.currentPlayer - 1] += 1;
        }
        LoadSave.playerRolled = false;
    }
    
}
