using System.Linq;
using UnityEngine;

public class CreateSettlement : MonoBehaviour
{

    public GameObject AdjacentSPP1;
    public GameObject AdjacentSPP2;
    public GameObject AdjacentSPP3;
    public GameObject[] AdjacentRoads;

    public Vector3[] resourceAccess;

    public MainGame MainScript;
    public SetupPhase SetupScript;

    public GameObject[] settlementPrefabs;

    //Occurs when player clicks a settlement placement point or SPP.
    public void OnMouseDown()
    {
//----------------------------------------------------------------------------------------------------------------------------------
        //Creates settlement and places it relative to the game object clicked.
        Vector3 position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z - .2f);

        int player = MainScript.playerInfo.currentPlayer - 1;
        
        var newObject = GameObject.Instantiate(settlementPrefabs[player]);
        newObject.transform.position = position;
        newObject.transform.SetParent(MainScript.parentOfCreatedObjects[MainScript.playerInfo.currentPlayer - 1].transform);

        //Sets variables for the script on the newly created settlement. (For upgrading settlements to cities.)
        newObject.GetComponent<UpgradeToCity>().MainScript = MainScript;
        newObject.GetComponent<UpgradeToCity>().SetupScript = SetupScript;
        newObject.GetComponent<BoxCollider>().enabled = false;
        foreach (Vector3 vecTile in resourceAccess)
        {
            newObject.GetComponent<UpgradeToCity>().refToTileAccess.Add(vecTile);
        }
//----------------------------------------------------------------------------------------------------------------------------------
        //Cleans up access list and adds access information to the current players list.
        MainScript.removeSettlementAccess(AdjacentSPP1, AdjacentSPP2, AdjacentSPP3, gameObject);
        MainScript.addSettlement(newObject);

        foreach (var road in AdjacentRoads)
        {
            MainScript.playerInfo.playersRoadAccess[player].Add(road);
        }
        MainScript.addPlayersResourceAccess(resourceAccess);
//----------------------------------------------------------------------------------------------------------------------------------
        //Sets name of new settlement. (Helpful for loading and saving games.)
        MainScript.playerInfo.indexForSetNames[player] += 1;
        newObject.name = MainScript.playerInfo.indexForSetNames[player].ToString();

        MainScript.playerInfo.nameOfClickedSPP[player].Add(gameObject.name);
//----------------------------------------------------------------------------------------------------------------------------------
        //Destroys placement points that can no longer be used.
        Destroy(AdjacentSPP1);
        Destroy(AdjacentSPP2);
        Destroy(AdjacentSPP3);
        Destroy(gameObject);

        //Run code if the game is not in the process of being loaded.
        if (!SetupScript.inLoadProcess)
        {
            MainScript.playerInfo.playersInfo[player][5] += 1;
            MainScript.setIndicators();
            MainScript.checkForWin();
        }
//----------------------------------------------------------------------------------------------------------------------------------
        //Run code if the game is not in the process of being loaded and setup phase is in progress.
        if (SetupScript.setupPhaseFinished == false && !SetupScript.inLoadProcess)
        {
            SetupScript.showRoadsAdjacentToNewSettlement(AdjacentRoads);
            MainScript.deactivateAllSPP();
        }
        else if(!SetupScript.inLoadProcess)
        {
            MainScript.deactivatePlayersSPP();
        }
//----------------------------------------------------------------------------------------------------------------------------------
        MainScript.logEvents("Player " + MainScript.playerInfo.currentPlayer + " placed a settlement.");
    }
}

