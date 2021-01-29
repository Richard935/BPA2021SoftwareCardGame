using UnityEngine;

public class CreateRoad : MonoBehaviour
{
    public GameObject AdjacentSPP1, AdjacentSPP2;
    public GameObject[] AdjacentRoads;

    public MainGame MainScript;
    public SetupPhase SetupScript;

    public GameObject[] roadPrefabs;

    //Occurs when a user clicks a road placement point.
    public void OnMouseDown()
    {

        MainScript.playerInfo.nameOfClickedRP[MainScript.playerInfo.currentPlayer - 1].Add(gameObject.name);
//----------------------------------------------------------------------------------------------------------------------------------
        //Creates road and sets its location and rotation the same as the game object clicked.
        int player = MainScript.playerInfo.currentPlayer - 1;
        var newObject = GameObject.Instantiate(roadPrefabs[player]);
        newObject.transform.position = gameObject.transform.position;
        newObject.transform.rotation = gameObject.transform.rotation;
        newObject.transform.SetParent(MainScript.gameBoard.transform);

        MainScript.removeRoadAccess(gameObject);

        Destroy(gameObject);
//----------------------------------------------------------------------------------------------------------------------------------
        //Add road placement and settlement access to the current players list. 
        foreach (var road in AdjacentRoads)
        {
            MainScript.playerInfo.playersRoadAccess[MainScript.playerInfo.currentPlayer - 1].Add(road);
        }
        MainScript.addSettlementAccess(AdjacentSPP1, AdjacentSPP2);   

        //Calls code for setup phase, if game is in the setup phase. If not then hide all road clickables.
        if(SetupScript.setupPhaseFinished == false && !SetupScript.inLoadProcess)
        {
            SetupScript.roadPlaced();
        }
        else if(!SetupScript.inLoadProcess)
        {
            MainScript.showRoadClickables(false);
        }
//----------------------------------------------------------------------------------------------------------------------------------
        MainScript.logEvents("Player " + MainScript.playerInfo.currentPlayer + " placed road.");

    }
}
