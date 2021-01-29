using System.Linq;
using UnityEngine;
using System.Collections.Generic;

//This class gets called in the beginning of a new game. Its purpose is to setup variables vitial variables and to operate the beginning setup phase of the game.
public class SetupPhase : MonoBehaviour
{

    public GameObject music;
    public GameObject robber;

    public GameObject[] allUIObjectsThatGetDisabled;
    public MainGame MainScript;
    public SettingsMenu settings;
    public SaveLoadSystem LoadSave;

    public bool inLoadProcess;

    public bool roundOne;
    public bool setupPhaseFinished;
    public bool forLastEndTurnClickDuringSetup;

    //This method gets called when the game scene is loaded. It sets up varaibles that are needed.
    void Start()
    {
        //Sets audio info.
        music = GameObject.Find("Music");
        settings.music = music.GetComponent<AudioSource>();
        settings.setVolume();

        //Gets all the settlement placement points and puts it in a variable, which is located in the MainGame class.
        MainScript.settlementClickables = GameObject.FindGameObjectsWithTag("SPP").ToList();
        MainScript.roadClickabes = GameObject.FindGameObjectsWithTag("RP").ToList();
        
        LoadSave.playerRolled = false;

        string save = PlayerPrefs.GetString("saveGame").ToString();
        PlayerPrefs.DeleteKey("saveGame");

        //Figures out if the player was wanting to load.
        if (save != "")
        {
            //Setup the scene from database.
            forLastEndTurnClickDuringSetup = true;
            setupPhaseFinished = true;
            roundOne = false;

            inLoadProcess = true;
            LoadSave.getFromDatabase(save);
            
        }
        else
        {
            //Else the game starts with the setup phase.

            //Roads point disabled.
            foreach (GameObject placementPoint in MainScript.roadClickabes)
            {
                placementPoint.SetActive(false);
            }

            inLoadProcess = false;

            //Logs the start of the game.
            MainScript.logEvents("Game Started");
        
            //Intializes bools on start
            setupPhaseFinished = false;
            roundOne = true;
            forLastEndTurnClickDuringSetup = false;

            //Disables all the UI buttons except for the end turn and pause buttons.
            for(int i = 0; i < allUIObjectsThatGetDisabled.Length; i++)
            {
                allUIObjectsThatGetDisabled[i].SetActive(false);
            }

            //Calls the setupPhase method.
            setupPhase();
        }

    }   

    //This method is called from the Start method.
    public void setupPhase()
    {

        //Cycles the turns starting with player 1, 2, 3, 3, 2, 1
        if(MainScript.playerInfo.currentPlayer == 3 && roundOne == true){
            roundOne = false;
        }else if(roundOne == false){
            MainScript.playerInfo.currentPlayer--;
        }else{
            MainScript.playerInfo.currentPlayer++;
        }

        //Disables the end turn button and enables the settlement placement points(SPP).      
        MainScript.btnEndTurn.gameObject.SetActive(false);
        MainScript.activateAllSPP();
        
        //Sets a text box, on the hud, with the message that is in quotations.
        MainScript.setupHint("The green circles on the vertices are clickables. Click on one to place a settlement.", true);

        //Sets all the players resources to 0 the first time this method is called.
        if(MainScript.playerInfo.currentPlayer == 1 && roundOne == true)
        {
            MainScript.setupResources();
        }
        
        //Sets the UI resource and player text indicators.
        MainScript.setIndicators();

    }

    //This method gets called when a player places a settlement during the setup phase. It enables road placement clickables that the newly placed settlement
    //has access to.
    public void showRoadsAdjacentToNewSettlement(GameObject[] adjacentRoads)
    {
        //Enables the road points adjancent to the newly placed settlement.
        foreach(var roadPoint in adjacentRoads)
        {
            if(roadPoint != null)
            {
                roadPoint.SetActive(true);
            }
            
        }

        //Sets a text box, on the hud, with the message that is in quotations.
        MainScript.setupHint("The black bars around your newly placed settlment can be clicked. Click on one to build a road.", true);
    }

    //When a player places a road during the setup phase, this method gets called.
    public void roadPlaced()
    {

        //Enables the end turn button and disables the road clickables.
        MainScript.btnEndTurn.gameObject.SetActive(true);
        MainScript.showRoadClickables(false);

        //Checks if player one just placed his second settlement and road, if so then declare the setup phase to be over.
        if (MainScript.playerInfo.currentPlayer == 1 && roundOne == false)
        {
            setupPhaseFinished = true;
            MainScript.playerInfo.isSetupPhase = false;

            //Sets a text box, on the hud, with the message that is in quotations.
            MainScript.setupHint("You can now roll the dice, collect resources around you, spend resources to expand, and trade for hard to get resources.", true);

            //Logs that the setup phase is finshed.
            MainScript.logEvents("Setup phase finshed.");
        }
        else
        {
            //Else display message and continue with the setup phase.
            MainScript.setupHint("You can do nothing else, click 'End Turn' to end your turn.", true);
        }
        
    }
}
