using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEditor;

public class MainGame : MonoBehaviour
{    
    public GameObject gameBoard;

    //Stores objects created during run time.
    public GameObject[] parentOfCreatedObjects;

    //Stores the settlement clickables or settlement placement points.
    public List<GameObject> settlementClickables;
    public List<GameObject> roadClickabes;

    //Pulls in the end turn btn and roll dice button. Set in unity editor.
    public Button btnEndTurn;
    public Button btnRollDice;

    //Robbers info.
    public GameObject tileRobberIsOn;
    public Vector3 refrenceToDeactivatedTile;
    public GameObject stealFromPlayerUI;

    //Pulls in the text boxes for each resource, current player indicator, and victory points indicator. Set in unity editor.
    public Text txtPlayerIndicator;
    public Text[] txtResourceBoxes;
    string[] textBeforeNum = { "Wood = ", "Brick = ", "Ore = ", "Wheat = ", "Sheep = ", "Total Victory Points = " };

    //Gets the SetupPhase script to be able to refrence the methods.
    public SetupPhase setupScript;
    
    //Class that holds all the players info.
    public PlayerVariables playerInfo = new PlayerVariables();
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    private void Start()
    {
        //Sets defualt save information.
        playerInfo.isSetupPhase = true;
        playerInfo.robberMoved = false;
    }

    //Logs run time events to text file.
    public List<string> eventLog;
    public void logEvents(string eventDescription)
    {
        eventLog.Add(eventDescription);
        StreamWriter writer = new StreamWriter("Assets/Resources/EventLog.txt");

        foreach (var e in eventLog)
        {
            writer.WriteLine(e);
            writer.WriteLine();
        }
        writer.Close();
        //AssetDatabase.Refresh();
    }

    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //initializes player variables when called by the SetupPhase script.
    public void setupResources()
    {
        for (int i = 0; i < 6; i++)
        {
            playerInfo.playersInfo[0].Add(0);
            playerInfo.playersInfo[1].Add(0);
            playerInfo.playersInfo[2].Add(0);
        }
    }
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //Sets the text boxes that indicate resources, current player, and total victory points.
    public void setIndicators()
    {
        //If current player is equal to four, then change it to one, since they're is only three players.
        if(playerInfo.currentPlayer == 4)
        {
            playerInfo.currentPlayer = 1;
        }

        //Takes current players number and subtracts one for use as an index value.
        int index = playerInfo.currentPlayer - 1;

        //Loops through setting each text box in txtResourceBoxes array.
        for (int i = 0; i < txtResourceBoxes.Length; i++)
        {
            txtResourceBoxes[i].text = textBeforeNum[i].ToString() + playerInfo.playersInfo[index][i].ToString();
        }

        //Sets player indicator text box.
        txtPlayerIndicator.text = "Player " + playerInfo.currentPlayer + "'s Turn";    
        
    }
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //Adds tile access to a player, on a list.
    public void addPlayersResourceAccess(Vector3[] tileInfo)
    {
        //Each vector = (resourceNum, tile number, give amount)

        int index = playerInfo.currentPlayer - 1;

        //Adds vectors to a current players list for use later.
        foreach (var item in tileInfo)
        {
               playerInfo.playersResTiles[index].Add(item);
        }

    }
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //Gives out resources accordingly. 
    public void AllocateResources(int numRolled)
    {
        Vector3 deactivateTile1 = new Vector3(refrenceToDeactivatedTile.x, refrenceToDeactivatedTile.y, 1);
        Vector3 deactivateTile2 = new Vector3(refrenceToDeactivatedTile.x, refrenceToDeactivatedTile.y, 2);

        //For each player.
        for (int i = 0; i < 3; i++)
        {
            //Loop throughs each vector in list.
            foreach(var item in playerInfo.playersResTiles[i])
            {
                //Checks if the y value in vetor is equal to numRolled and if the vecotor is not equal to the tile the robber is on.
                if(Convert.ToInt32(item.y) == numRolled && item != deactivateTile1 && item != deactivateTile2)
                {
                    //Adds the z(depends on if settelment(1) or city(2)) value in vector to the specific resource the tile gives, indicated by the x)
                    playerInfo.playersInfo[i][Convert.ToInt32(item.x)] += Convert.ToInt32(item.z);
                }
            }
        }

        //Refresh the text boxes.
        setIndicators();     
    }
    
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //The following methods use the players road access list.

    //This method removes the road clickable objects that are passed into this method from all the players road access list to prevent null errors when using the lists.
    public void removeRoadAccess(GameObject removeThis)
    {
        //Loops through all players road access list, removing the passed in object.
        for(int i = 0; i < 3; i++)
        {
            playerInfo.playersRoadAccess[i].Remove(removeThis);
        } 

    }

    //Shows the roads the current player has access to. 
    public void showRoadClickables(bool onOrOff)
    {
        //Takes current players number and subtracts one for use as an index value.
        int index = playerInfo.currentPlayer - 1;

        //Activates all objects in current players road access list. Skipping any possible null items in list.
        foreach (var item in playerInfo.playersRoadAccess[index])
        {
            if(item != null)
            {
                item.SetActive(onOrOff);
            }      
        }
    }
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //The following methods add and removes settlement clickables that a player now can use or no longer use.

    //When a road is placed this method is called.
    public void addSettlementAccess(GameObject pointOne, GameObject pointTwo)
    {
        //Takes current players number and subtracts one for use as an index value.
        int index = playerInfo.currentPlayer - 1;

        //Adds objects to list if that object isn't null.
        if(pointOne != null)
        {
            playerInfo.playersSettlementAccess[index].Add(pointOne);
        }
        if (pointTwo != null)
        {
            playerInfo.playersSettlementAccess[index].Add(pointTwo);
        }
    }

    //This method is called when settlement placement points get destroyed or apon settlement creation.
    public void removeSettlementAccess(GameObject obj1, GameObject obj2, GameObject obj3, GameObject obj4)
    {
        //Removes objects from all list it could be stored in.
        for(int i = 0; i < 3; i++)
        {
            playerInfo.playersSettlementAccess[i].Remove(obj1); 
            playerInfo.playersSettlementAccess[i].Remove(obj2);
            playerInfo.playersSettlementAccess[i].Remove(obj3);
            playerInfo.playersSettlementAccess[i].Remove(obj4);

        }
        settlementClickables.Remove(obj1);
        settlementClickables.Remove(obj2);
        settlementClickables.Remove(obj3);
        settlementClickables.Remove(obj4);
    }
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //The following methods activate and deactivate settlement placement points.
    public void activateAllSPP() {
        //Activates all existing settlement placement points.
        foreach (GameObject child in settlementClickables)
        {     
            child.gameObject.SetActive(true);
        }
    }
    public void deactivateAllSPP()
    {
        //Deactivates all existing settlement placement points.
        foreach (GameObject child in settlementClickables)
        {
            child.gameObject.SetActive(false);
        }
    }
    public void activatePlayersSPP()
    {
        //Takes current players number and subtracts one for use as an index value.
        int index = playerInfo.currentPlayer - 1;

        //Activates all settlement placement points that the current player has access to.
        foreach (GameObject placementPoint in playerInfo.playersSettlementAccess[index])
        {
            if (placementPoint != null) {
                placementPoint.SetActive(true);
            }
            else
            {
                Debug.Log(placementPoint);
            }
        }
              
    }
    public void deactivatePlayersSPP()
    {
        //Takes current players number and subtracts one for use as an index value.
        int index = playerInfo.currentPlayer - 1;

        //Deactivates all settlement placement points that the current player has access to.
        foreach (GameObject placementPoint in playerInfo.playersSettlementAccess[index])
        {
            placementPoint.SetActive(false);
        }
    }
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //Stores settlements that a player has or owns. Called on settlement creation.
    public void addSettlement(GameObject settlement)
    {
        //Takes current players number and subtracts one for use as an index value.
        int index = playerInfo.currentPlayer - 1;

        if(settlement != null)
        {
            playerInfo.playersSettlements[index].Add(settlement);
        }

    }
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //Called when a settlement is upgraded to a city. 
    public void increaseLevel(GameObject settlementClicked, GameObject replacement)
    {
        int index;

        //Takes current players number and subtracts one for use as an index value.
        int playerToIndex = playerInfo.currentPlayer - 1;

        List<Vector3> currentPlayersList = playerInfo.playersResTiles[playerToIndex]; 

        //Changes z to 2, which is used in the production of resources.
        index = playerInfo.playersSettlements[playerToIndex].IndexOf(replacement);
        currentPlayersList[index] = new Vector3(currentPlayersList[index].x, currentPlayersList[index].y, 2);

        //Removes clicked settelment from list.
        playerInfo.playersSettlements[playerToIndex].Insert(index, replacement);
        playerInfo.playersSettlements[playerToIndex].Remove(settlementClicked);

    }
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //The following code gets used for the robber event.
    
    public GameObject[] btnStealFromPlayer;
    public GameObject txtStealInstructions;

    //===============================================================================================
    //Occurs after robber was moved.

    //-----------------------------------------------------------------------------------------------
    public void StealingResource()
    {
        //The player can steal a resource from an enemy player. If the enemy's player settlement is in vicinity of where the robber was placed.

        //Calls findPlayerOnTile method, returns 3 booleans.
        bool[] playersToStealFrom = findPlayerOnTile();
        bool btnIsActive = false;

        //Activates each players button that the current player can steal from.
        for(int i =0; i < 3; i++)
        {
            if (playersToStealFrom[i] && doesPlayerHaveResources(i))
            {
                btnStealFromPlayer[i].SetActive(true);
                btnIsActive = true;
            }
        }

        //If a 'steal from player' button is active, then display the steal menu. Else call checkPlayersResourceAmount method.
        if (btnIsActive)
        {
            gameBoard.SetActive(false);
            txtStealInstructions.SetActive(true);
            stealFromPlayerUI.SetActive(true);
        }
        else
        {
            checkPlayersResourceAmount();
        }    

    }
    //-----------------------------------------------------------------------------------------------
    //Finds players, other than current, on the tile the robber is now on and returns bools for each player.
    private bool[] findPlayerOnTile()
    {
        //Presets booleans to false.
        bool[] canStealFromPlayers = { false, false, false };


        int index = playerInfo.currentPlayer - 1;
        //If current player is not player 1.
        if(index != 0)
        {
            //Loop through player 1's ResTiles list.
            foreach (var vector in playerInfo.playersResTiles[0])
            {
                //If vectors x and y componets equal the deactivated tiles x and y.
                if(vector.x == refrenceToDeactivatedTile.x && vector.y == refrenceToDeactivatedTile.y)
                {
                    canStealFromPlayers[0] = true;
                    break;
                }
            }
        }

        //If current player is not player 2.
        if (index != 1)
        {
            //Loop through player 2's ResTiles list.
            foreach (var vector in playerInfo.playersResTiles[1])
            {
                //If vectors x and y componets equal the deactivated tiles x and y.
                if (vector.x == refrenceToDeactivatedTile.x && vector.y == refrenceToDeactivatedTile.y)
                {
                    canStealFromPlayers[1] = true;
                    break;
                }
            }
        }

        //If current player is not player 3.
        if (index != 2)
        {
            //Loop through player 3's ResTiles list.
            foreach (var vector in playerInfo.playersResTiles[2])
            {
                //If vectors x and y componets equal the deactivated tiles x and y.
                if (vector.x == refrenceToDeactivatedTile.x && vector.y == refrenceToDeactivatedTile.y)
                {
                    canStealFromPlayers[2] = true;
                    break;
                }
            }
        }

        return canStealFromPlayers;
    }
    //-----------------------------------------------------------------------------------------------
    //Checks if player has a resource to steal.
    public bool doesPlayerHaveResources(int index)
    {
        //If a resource number is not equal to 0, then return true. Else if all false return false.
        for(int i = 0; i < 5; i++)
        {
            if(playerInfo.playersInfo[index][i] != 0)
            {
                return true;
            }
            
        }
        return false;
    }
    //===============================================================================================
    private int[] playerMustRemo = { 0, 0, 0 };

    public GameObject hud;
    public GameObject removeUI;

    public Text[] txtOnRemoUI;
    public string[] strPreset = {"Wood Selected = ", "Brick Selected = ", "Ore Selected = ", "Wheat Selected = ", "Sheep Selected = " };
    public Slider[] sliOnRemove;
    
    public int[] eachAmountSelected = { 0, 0, 0, 0, 0 };
    //-----------------------------------------------------------------------------------------------
    //Occurs after steal resource. Checks all players if they have 8 or more resources, they must remove half of them if true.
    public void checkPlayersResourceAmount()
    {
        int resTotal = 0;

        //Loops through each player.
        for(int player = 0; player < 3; player++)
        {
            //Gets sum of players resources.
            for(int resIndex = 0; resIndex < 5; resIndex++)
            {
                resTotal = resTotal + playerInfo.playersInfo[player][resIndex];
            }
            //If sum is equal to or greater than 8, then set remove amount. Else set remove amount as 0.
            if(resTotal >= 8)
            {
                playerMustRemo[player] = (resTotal / 2);
            }
            else
            {
                playerMustRemo[player] = 0;
            }
            
            resTotal = 0;
            
        }
        //Calls setRemoveHalfResCanvas method.
        setRemoveHalfResCanvas();
    }
    //-----------------------------------------------------------------------------------------------
    //Setups the remove resources interface.
    private void setRemoveHalfResCanvas()
    {

        for(int player = 0; player < 3; player++)
        {
            //0 means that the player doesn't 8 or more resources.
            if(playerMustRemo[player] != 0)
            {
                //Set instructions with players info.
                txtOnRemoUI[0].text = "Player " + (player + 1) + ", you have\r\n"+ "over 8 total resources.";
                txtOnRemoUI[1].text = "You must\r\n" + "remove " + playerMustRemo[player] + " resources.";

                //Resets and sets max value for each slider.
                for(int i = 0; i < 5; i++)
                {
                   sliOnRemove[i].value = 0;
                   sliOnRemove[i].maxValue = playerInfo.playersInfo[player][i];
                   txtOnRemoUI[(i + 2)].text = strPreset[i] + 0;
                }

                removeUI.SetActive(true);
                gameBoard.SetActive(false);
                hud.SetActive(false);

                break;

            }else if (player == 2)
            {
                //Resumes back to game.
                setupHint("You can now roll the dice, collect resources around you, spend resources to expand, and trade for hard to get resources.", true);
                txtHint.fontSize = 14;

                setIndicators();
                removeUI.SetActive(false);
                gameBoard.SetActive(true);
                hud.SetActive(true);
            }
        }
        
    }
    //-----------------------------------------------------------------------------------------------
    //Removes resources the player has chosen to remove.
    public void remove()
    {
        //Loop through each player.
        for (int player = 0; player < 3; player++)
        {
            if (playerMustRemo[player] != 0)
            {       
                
                if (playerMustRemo[player] == (eachAmountSelected[0] + eachAmountSelected[1] + eachAmountSelected[2] + eachAmountSelected[3] + eachAmountSelected[4]))
                {
                    //Remove amount selected.
                    for(int i = 0; i < 5; i++)
                    {
                        playerInfo.playersInfo[player][i] -= eachAmountSelected[i];
                    }

                    playerMustRemo[player] = 0;

                    setRemoveHalfResCanvas();
                }
                else
                {
                    //Error message.
                    messageBox.gameObject.SetActive(true);
                    messageBox.text = "You have not selected the correct amount of resoources.";
                }

            }
        }
                
    }
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public bool canPurchase(int[] cost)
    {
        //Finds if player can afford the item.
        for(int i = 0; i < 5; i++)
        {
            if(playerInfo.playersInfo[(playerInfo.currentPlayer - 1)][i] < cost[i])
            {
                return false;
            }

        }

        return true; ;
    }
    //-/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public GameObject winScreen;
    public GameObject enterName;
    
    public Text[] txtHighScores;
    public Text messageBox;
    public Text turnsTaken;
     
    int[] numHighScoreTurns = { 0, 0, 0 };
    string[] names = { "", "", "" };
    //---------------------------------------------------------------------------------------------------------------------------------------------------
    //Checks if player has 10 or more victory points.
    public void checkForWin()
    {
        if(playerInfo.playersInfo[playerInfo.currentPlayer - 1][5] >= 10)
        {
            //Setup win menu.

            //Get high scores from text file.
            readHighScore();

            for (int i = 0; i < 3; i++)
            {
                txtHighScores[i].text = names[i] + " won in " + numHighScoreTurns[i].ToString() + " turns.";
            }
            turnsTaken.text = "You won in " + playerInfo.turnsTaken[playerInfo.currentPlayer - 1] + " turns!";

            //Checks if player placed on the leader board.
            if (playerInfo.turnsTaken[playerInfo.currentPlayer - 1] <= numHighScoreTurns[0] || 
                playerInfo.turnsTaken[playerInfo.currentPlayer - 1] <= numHighScoreTurns[1] || 
                playerInfo.turnsTaken[playerInfo.currentPlayer - 1] <= numHighScoreTurns[2]   )
            {
                //Ask for name.
                enterName.SetActive(true);
            }
            else
            {
                //Tell player they did't place.
                messageBox.text = "You didn't place on the leader board. Better luck next time.";
                messageBox.gameObject.SetActive(true);
            }

            //Shows win menu.
            hud.SetActive(false);
            gameBoard.SetActive(false);
            winScreen.SetActive(true);
        }
    }
    //---------------------------------------------------------------------------------------------------------------------------------------------------
    private void writeHighScore()
    {
        //Writes the high scores to a text file.
        string path = "Assets/Resources/HighScores.txt";
        StreamWriter writer = new StreamWriter(path);

        for (int i = 0; i < 3; i++)
        {
            writer.WriteLine(names[i]);             //Name
            writer.WriteLine(numHighScoreTurns[i]); //Turns taken
        }
        writer.Close();
        //AssetDatabase.Refresh();
    }
    //---------------------------------------------------------------------------------------------------------------------------------------------------
    private void readHighScore()
    {
        //Reads the high scores from a text file.
        string path = "Assets/Resources/HighScores.txt";
        StreamReader reader = new StreamReader(path);

        for(int i = 0; i < 3; i++)
        {
            names[i] = reader.ReadLine();
            numHighScoreTurns[i] = Convert.ToInt32(reader.ReadLine());
        }
        reader.Close();
    }
    //-/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // <param name="playerName"></param>
    public void placeWinner(string playerName)
    {
        //Find if player won in less turns then a person on highscore
        //1st (set 2 as 3 and 1 as 2) 2nd(set 2 as 3) 3rd(replace 3)
        //Do this after name has been given.
        if (playerInfo.turnsTaken[playerInfo.currentPlayer - 1] <= numHighScoreTurns[0])
        {
            //Move second place to third.
            names[2] = names[1];
            numHighScoreTurns[2] = numHighScoreTurns[1];

            //Move first place to second.
            names[1] = names[0];
            numHighScoreTurns[1] = numHighScoreTurns[0];

            //Put user in 1st place.
            names[0] = playerName;
            numHighScoreTurns[0] = playerInfo.turnsTaken[playerInfo.currentPlayer - 1];

        }
        else if (playerInfo.turnsTaken[playerInfo.currentPlayer - 1] <= numHighScoreTurns[1])
        {
            //Move second place to third.
            names[2] = names[1];
            numHighScoreTurns[2] = numHighScoreTurns[1];
            //Put user in 2nd place.
            names[1] = playerName;
            numHighScoreTurns[1] = playerInfo.turnsTaken[playerInfo.currentPlayer - 1];
        }
        else if (playerInfo.turnsTaken[playerInfo.currentPlayer - 1] <= numHighScoreTurns[2])
        {
            //Put user in 3rd place.
            names[2] = playerName;
            numHighScoreTurns[2] = playerInfo.turnsTaken[playerInfo.currentPlayer - 1];
        }

        //Refresh score board.
        for (int i = 0; i < 3; i++)
        {
            txtHighScores[i].text = names[i] + " took " + numHighScoreTurns[i].ToString() + " turns.";
        }
        
        //Write scores
        writeHighScore();
    }
    //-----------------------------------------------------------------------------------------------
    public Image imgHint;
    public Text txtHint;
    public void setupHint(string message, bool show)
    {
        //Setups and shows the hints.
        if (show)
        {
            txtHint.text = message;
            imgHint.gameObject.SetActive(true);
            txtHint.gameObject.SetActive(true);
        }
        else
        {
            imgHint.gameObject.SetActive(false);
            txtHint.gameObject.SetActive(false);
        }
    }
    //-----------------------------------------------------------------------------------------------
}
