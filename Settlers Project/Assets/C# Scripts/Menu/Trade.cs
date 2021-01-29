using UnityEngine;
using UnityEngine.UI;

public class Trade : MonoBehaviour
{
    public GameObject tradeCanvas;
    public MainGame MainScript;

    public GameObject tradeResources;
    public Dropdown playerResSelection;
    public Dropdown getResSelection;
    
    public GameObject btnTrade;

    private string[] resNames = { "Wood", "Brick", "Ore", "Wheat", "Sheep" };

    //-----------------------------------------------------------------------------------------------------------------------------------
    //Opens the trade menu.
    public void openTradeMenu()
    {
        MainScript.gameBoard.SetActive(false);
        MainScript.hud.SetActive(false);

        //Setup trade menu before activating it.
        setupCanvas();
        tradeCanvas.SetActive(true);
    }
    //-----------------------------------------------------------------------------------------------------------------------------------
    private void setupCanvas()
    {
        //Puts what the player can trade in the drop down menu selection.
        bool canTrade = false;
        for (int i = 0; i < 5; i++)
        {
            if (MainScript.playerInfo.playersInfo[(MainScript.playerInfo.currentPlayer - 1)][i] >= 4)
            {
                playerResSelection.options.Add(new Dropdown.OptionData(resNames[i]));

                canTrade = true;
            }
        }
        //Allow trade setup.
        if (canTrade)
        {
            tradeResources.SetActive(true);
        }

    }
    //-----------------------------------------------------------------------------------------------------------------------------------
    public void CheckIfBothSelected()
    {
        //If the player has selected something in both drop down menus, show a Trade button to confirm trade.
        if (playerResSelection.options[playerResSelection.value].text != "(None Selected)" && getResSelection.options[getResSelection.value].text != "(None Selected)")
        {
            btnTrade.SetActive(true);
        }
        else
        {
            btnTrade.SetActive(false);
        }
    }
    //-----------------------------------------------------------------------------------------------------------------------------------
    public void preformTrade()
    {
        //Removes four resources that the player chosen.
        for (int i =0; i < 5; i++)
        {
            if(playerResSelection.options[playerResSelection.value].text == resNames[i]){
                MainScript.playerInfo.playersInfo[(MainScript.playerInfo.currentPlayer - 1)][i] -= 4;
                break;
            }
        }

        //Adds one resource that the player chosen.
        for (int i = 0; i < 5; i++)
        {
            if (getResSelection.options[getResSelection.value].text == resNames[i])
            {
                MainScript.playerInfo.playersInfo[(MainScript.playerInfo.currentPlayer - 1)][i] += 1;
                break;
            }
        }
        
        //Resets the trade menu and resumes game.
        playerResSelection.options.RemoveRange(1, playerResSelection.options.Count - 1);
        
        playerResSelection.value = 0; 
        getResSelection.value = 0;

        tradeResources.SetActive(false);
        MainScript.setIndicators();
        exitTradeMenu();

        MainScript.logEvents("Player " + MainScript.playerInfo.currentPlayer + " traded with bank.");
    }
    //-----------------------------------------------------------------------------------------------------------------------------------
    public void exitTradeMenu()
    {
        //Used for player to leave trade menu without trading. Resumes game.
        playerResSelection.options.RemoveRange(1, playerResSelection.options.Count - 1);
        tradeCanvas.SetActive(false);
        MainScript.gameBoard.SetActive(true);
        MainScript.hud.SetActive(true);
    }

}
