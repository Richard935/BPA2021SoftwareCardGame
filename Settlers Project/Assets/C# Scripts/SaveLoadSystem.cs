using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Proyecto26;
using UnityEngine.UI;

public class SaveLoadSystem : MonoBehaviour
{
    public MainGame mainScript;
    public SetupPhase setupScript;

    public GameObject robber;
    public Text txtMessage;

    public bool playerRolled;

    //-------------------------------------------------------------------------------------------------------------------------
    public void Back()
    {
        //Closes save menu.
        txtMessage.text = "";
        txtMessage.gameObject.SetActive(false);

        pauseMenu.SetActive(true);
        saveUI.SetActive(false);
    }
    //-------------------------------------------------------------------------------------------------------------------------
    //Saves game to database.
    public void putToDatabase(string saveName)
    {
        //Gets all the robber information needed to save.
        List<float> tempRobCoords = new List<float>();
        List<float> tempDeactivatedTile = new List<float>();
        if (mainScript.playerInfo.robberMoved)
        {
            tempRobCoords.Add(mainScript.tileRobberIsOn.transform.position.x);
            tempRobCoords.Add(mainScript.tileRobberIsOn.transform.position.y);
            tempRobCoords.Add(mainScript.tileRobberIsOn.transform.position.z);
            
            tempDeactivatedTile.Add(mainScript.refrenceToDeactivatedTile.x);
            tempDeactivatedTile.Add(mainScript.refrenceToDeactivatedTile.y);
            tempDeactivatedTile.Add(mainScript.refrenceToDeactivatedTile.z);
        }


        string basePath = "https://bpacomp2020-default-rtdb.firebaseio.com";
        RequestHelper currentRequest;

        //Starts the process of saving to the database.
        currentRequest = new RequestHelper
        {
            Uri = basePath + "/saves/" + saveName + ".json",

            //Contents of body, SaveData class, is what is put to the database.
            Body = new SaveData()
            {
                //Sets all variables in SaveData class.

                p1Resources = mainScript.playerInfo.playersInfo[0],
                p1ClickSPP = mainScript.playerInfo.nameOfClickedSPP[0],
                p1UpgradeSet = mainScript.playerInfo.nameOfSetUpgraded[0],
                p1ClickRoads = mainScript.playerInfo.nameOfClickedRP[0],

                p2Resources = mainScript.playerInfo.playersInfo[1],
                p2ClickSPP = mainScript.playerInfo.nameOfClickedSPP[1],
                p2UpgradeSet = mainScript.playerInfo.nameOfSetUpgraded[1],
                p2ClickRoads = mainScript.playerInfo.nameOfClickedRP[1],

                p3Resources = mainScript.playerInfo.playersInfo[2],
                p3ClickSPP = mainScript.playerInfo.nameOfClickedSPP[2],
                p3UpgradeSet = mainScript.playerInfo.nameOfSetUpgraded[2],
                p3ClickRoads = mainScript.playerInfo.nameOfClickedRP[2],
                
                turnsTaken = new List<int>(mainScript.playerInfo.turnsTaken),
                currentPlayer = mainScript.playerInfo.currentPlayer,
                isSetupPhase = mainScript.playerInfo.isSetupPhase,

                robberMoved = mainScript.playerInfo.robberMoved,
                robberCoords = tempRobCoords,
                deactivatedTile = tempDeactivatedTile,

                diceRolled = playerRolled
            }

        };
        RestClient.Put<SaveData>(currentRequest)
            .Then(res => Debug.Log(JsonUtility.ToJson(res, true)))
            .Catch(err => Debug.Log(err.Message));

        //Saves name of save file in player prefs.
        PlayerPrefs.SetString(inputName.text, inputName.text);

    }
    //-------------------------------------------------------------------------------------------------------------------------
    //Gets data from database.
    public void getFromDatabase(string saveName)
    {
        string basePath = "https://bpacomp2020-default-rtdb.firebaseio.com";

        //Gets data from 'saveName' in database.
        RestClient.Get(basePath + "/saves/" + saveName + ".json")
        .Then(res => {
            SaveData saveData = new SaveData();
            saveData = JsonUtility.FromJson<SaveData>(res.Text);

            //Calls a method to setup the game with given data.
            setupScene(saveData);

        }) 
        .Catch(err => Debug.Log(err.Message));

    }
    //-------------------------------------------------------------------------------------------------------------------------
    public GameObject pauseMenu;
    public GameObject saveUI;
    public Text inputName;
    public void getSaveName()
    {
        //Opens save menu.
        pauseMenu.SetActive(false);
        saveUI.SetActive(true);
    }
    public void saveGame()
    {
        if(inputName.text != "")
        {
            if (setupScript.setupPhaseFinished)
            {
                //Return to pause menu and put info to database.
                pauseMenu.SetActive(true);
                saveUI.SetActive(false);

                putToDatabase(inputName.text);
            }
            else
            {
                //Error message.
                txtMessage.text = "You can't save the game during the setup phase.";
                txtMessage.gameObject.SetActive(true);
            }
                
        }
        else
        {
            //Error message.
            txtMessage.text = "Didn't enter a name for the save file.";
            txtMessage.gameObject.SetActive(true);
        }

    }
    //-------------------------------------------------------------------------------------------------------------------------
    
    public void prepForLoad()
    {
        if (inputName.text != "" && inputName.text == PlayerPrefs.GetString(inputName.text))
        {
            //Store file name to player prefs and load game scene.
            pauseMenu.SetActive(true);
            saveUI.SetActive(false);

            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetString("saveGame", inputName.text);

            DontDestroyOnLoad(GameObject.Find("Music"));
            SceneManager.LoadScene("Game");
        }
        else
        {
            //Error message.
            txtMessage.gameObject.SetActive(true);
            txtMessage.text = "Didn't enter a name of a save file.";
        }
        
    }

    //-------------------------------------------------------------------------------------------------------------------------
    //Call mutiple methods from different objects to get the game to the save point.
    public void setupScene(SaveData saveData)
    {
        //------------------------------------------------------------------------------------------
        //Game state
        if (!saveData.isSetupPhase)
        {
            setupScript.setupPhaseFinished = true;
        }
        //------------------------------------------------------------------------------------------
        //P1
        mainScript.playerInfo.currentPlayer = 1;        
        foreach (string name in saveData.p1ClickRoads)
        {
            GameObject.Find(name).GetComponent<CreateRoad>().OnMouseDown();
        }
        foreach (var name in saveData.p1ClickSPP)
        {
            GameObject.Find(name).GetComponent<CreateSettlement>().OnMouseDown();
        }
        foreach(string name in saveData.p1UpgradeSet)
        {
            GameObject temp = GameObject.Find("Player1Objects");
            temp.transform.Find(name).GetComponent<UpgradeToCity>().OnMouseDown();
        }
        //------------------------------------------------------------------------------------------
        //P2
        mainScript.playerInfo.currentPlayer = 2;
        foreach (string name in saveData.p2ClickRoads)
        {
            GameObject.Find(name).GetComponent<CreateRoad>().OnMouseDown();
        }
        foreach (string name in saveData.p2ClickSPP)
        {
            GameObject.Find(name).GetComponent<CreateSettlement>().OnMouseDown();
        }
        foreach (string name in saveData.p2UpgradeSet)
        {
            GameObject temp = GameObject.Find("Player2Objects");
            temp.transform.Find(name).GetComponent<UpgradeToCity>().OnMouseDown();
        }
        //------------------------------------------------------------------------------------------
        //P3
        mainScript.playerInfo.currentPlayer = 3;
        foreach (string name in saveData.p3ClickRoads)
        {
            GameObject.Find(name).GetComponent<CreateRoad>().OnMouseDown();
        }
        foreach (string name in saveData.p3ClickSPP)
        {
            GameObject.Find(name).GetComponent<CreateSettlement>().OnMouseDown();
        }
        foreach (string name in saveData.p3UpgradeSet)
        {
            GameObject temp = GameObject.Find("Player3Objects");
            temp.transform.Find(name).GetComponent<UpgradeToCity>().OnMouseDown();
        }
        //------------------------------------------------------------------------------------------
        //Set resources
        mainScript.playerInfo.playersInfo[0] = saveData.p1Resources;
        mainScript.playerInfo.playersInfo[1] = saveData.p2Resources;
        mainScript.playerInfo.playersInfo[2] = saveData.p3Resources;

        for(int i = 0; i < 3; i++)
        {
            mainScript.playerInfo.turnsTaken[i] = saveData.turnsTaken[i];
        }
        //------------------------------------------------------------------------------------------
        if (saveData.robberMoved)
        {
            //sets robber position
            robber.transform.position = new Vector3(saveData.robberCoords[0], saveData.robberCoords[1], saveData.robberCoords[2]);

            //sets a refrence to the newly deactivated tile
            mainScript.refrenceToDeactivatedTile = new Vector3(saveData.deactivatedTile[0], saveData.deactivatedTile[1], saveData.deactivatedTile[2]);
        }
        //------------------------------------------------------------------------------------------
        mainScript.playerInfo.currentPlayer = saveData.currentPlayer;
        //------------------------------------------------------------------------------------------
        //Disables all points.
        foreach (GameObject placementPoint in mainScript.settlementClickables)
        {
            placementPoint.SetActive(false);
        }
        foreach (GameObject placementPoint in mainScript.roadClickabes)
        {
            placementPoint.SetActive(false);
        }

        //Sets if the current player has rolled yet.
        if (saveData.diceRolled)
        {
            //Has rolled.
            mainScript.btnEndTurn.gameObject.SetActive(true);
            mainScript.btnRollDice.gameObject.SetActive(false);
        }
        else
        {
            //Hasn't rolled.
            mainScript.btnEndTurn.gameObject.SetActive(false);
            mainScript.btnRollDice.gameObject.SetActive(true);
        }

        //Finishes load process.
        mainScript.setIndicators();

        setupScript.inLoadProcess = false;
        //------------------------------------------------------------------------------------------
    }
    //-------------------------------------------------------------------------------------------------------------------------
}