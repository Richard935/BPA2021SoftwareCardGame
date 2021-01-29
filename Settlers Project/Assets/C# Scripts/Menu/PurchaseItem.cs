using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PurchaseItem : MonoBehaviour
{
    public int[] cost;
    public int item;
    public GameObject purchaseMenu;
    public MainGame MainScript;
    public GameObject[] hudExcludingResIndicator;

    public Text txtMessage;
//-----------------------------------------------------------------------------------------------------------------------------------
    public void onClick()
    {
        //Opens purchase menu when player clicks the Purchase Items button.
        foreach (var UIObject in hudExcludingResIndicator)
        {
            UIObject.SetActive(false);
        }
        MainScript.gameBoard.SetActive(false);
        purchaseMenu.SetActive(true);
    }
//-----------------------------------------------------------------------------------------------------------------------------------
    //Occurs when a user tries to pruchase an item.
    public void onItemClick()
    {

        if (MainScript.canPurchase(cost))
        {
            //Figures out what was purchased (1=Settlement, 2=City, 3=Road, else=Victory Point)
            if (item==1)
            {
                //Show avalible points.
                if(MainScript.playerInfo.playersSettlementAccess[(MainScript.playerInfo.currentPlayer - 1)].Count != 0)
                {
                    MainScript.activatePlayersSPP();
                    removeRes();
                }
                else
                {
                    //Display error message.
                    txtMessage.gameObject.SetActive(true);
                    txtMessage.text = "No access to  settlement placement point";
                }        
            }
            else if (item==2)
            {
                //Check if player has a settlement that they can upgrade. If so then enable the box collider. (Allows for the detection of a mouse click.)
                if(MainScript.playerInfo.playersSettlements[MainScript.playerInfo.currentPlayer - 1].Count > 0)
                {
                    foreach(GameObject settlement in MainScript.playerInfo.playersSettlements[MainScript.playerInfo.currentPlayer - 1])
                    {
                       settlement.GetComponent<BoxCollider>().enabled = true;
                    }
                    MainScript.setupHint("You have purchased a city. Just click on one of your settlements to upgrade it to a city. Cities produce 2 resources and gives 2 victory points.", true);
                    removeRes();
                }
                else
                {
                    //Display error message.
                    txtMessage.gameObject.SetActive(true);
                    txtMessage.text = "You have no settelments.";
                }

            }
            else if (item==3)
            {
                //Show road placement points the player has access to.
                if(MainScript.playerInfo.playersRoadAccess[(MainScript.playerInfo.currentPlayer - 1)] != null){
                    MainScript.showRoadClickables(true);
                    removeRes();
                }
                else
                {
                    //Display error message.
                    txtMessage.gameObject.SetActive(true);
                    txtMessage.text = "No access to road placement point";
                }   
            }
            else
            {
                MainScript.playerInfo.playersInfo[(MainScript.playerInfo.currentPlayer - 1)][5] += 1;

                removeRes();
                MainScript.checkForWin();
            }
        }
        else
        {
            //Display error message.
            txtMessage.gameObject.SetActive(true);
            txtMessage.text = "You don't have enough resources to purchase that.";
        }
        //List order = wood, brick, ore, wheat, sheep, VP
        MainScript.logEvents("Player " + MainScript.playerInfo.currentPlayer + " purchased an item.");
      
    }
//----------------------------------------------------------------------------------------------------------------------------------- 
    //Purchased objects have been placed, now the player pays up the resources it costed.
    private void removeRes()
    {
        for(int i = 0; i < 5; i++)
        {
            MainScript.playerInfo.playersInfo[(MainScript.playerInfo.currentPlayer - 1)][i] -= cost[i];
        }
        MainScript.setIndicators();

        //Resumes game.
        foreach (var UIObject in hudExcludingResIndicator)
        {
            UIObject.SetActive(true);
        }
        MainScript.gameBoard.SetActive(true);
        purchaseMenu.SetActive(false);
        txtMessage.gameObject.SetActive(false);
        txtMessage.text = "";
    }

}
