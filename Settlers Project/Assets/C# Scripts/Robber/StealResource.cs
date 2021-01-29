using UnityEngine.UI;
using UnityEngine;

public class StealResource : MonoBehaviour
{
    
    public MainGame MainScript;
    public Text txtResourceStole;
    public GameObject btnConfirm;
    public int playerSelected;

    //Occurs when player selectes a player to steal from.
    public void OnPlayerSelected()
    {

        string[] resourceType = {"wood", "brick", "ore", "wheat", "sheep"};
        int intRandom;
        System.Random random = new System.Random();

        //Get a random resource that the player selected has.
        while (true)
        {
            intRandom = random.Next(0, 5);
            if (canRemoveResource(playerSelected, intRandom))
            {
                break;
            }
        }
        MainScript.setIndicators();
        MainScript.txtStealInstructions.SetActive(false);

        //The current player what they stole.
        txtResourceStole.text = "You have stolen one " + resourceType[intRandom] + " from player " + (playerSelected + 1) + "."; 
        txtResourceStole.gameObject.SetActive(true);

        //Reset steal selection screen.
        MainScript.btnStealFromPlayer[0].SetActive(false);
        MainScript.btnStealFromPlayer[1].SetActive(false);
        MainScript.btnStealFromPlayer[2].SetActive(false);
        btnConfirm.SetActive(true);

        MainScript.logEvents("Player " + MainScript.playerInfo.currentPlayer + " stole a resource.");
    }
    //Player clicks confirm button.
    public void OnConfirmClick()
    {
        //Close steal menu and call a method to check each players total amount of resources.
        btnConfirm.SetActive(false);
        txtResourceStole.gameObject.SetActive(false);
        MainScript.stealFromPlayerUI.SetActive(false);
        MainScript.gameBoard.SetActive(true);
        MainScript.checkPlayersResourceAmount();
    }

    //Checks if random resource can be removed, if so then remove and give to current player.
    public bool canRemoveResource(int index, int resourceType)
    {
        //If resource can be removed, remove, give to current player, and return true.
        if (MainScript.playerInfo.playersInfo[index][resourceType] != 0)
        {
            MainScript.playerInfo.playersInfo[index][resourceType]--;
            MainScript.playerInfo.playersInfo[MainScript.playerInfo.currentPlayer - 1][resourceType]++;
            return true;
        }
        else
        {
            return false;
        }
    }
}
