using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeToCity : MonoBehaviour
{
    public List<Vector3> refToTileAccess;
    public GameObject[] cityPrefabs;
    
    public MainGame MainScript;
    public SetupPhase SetupScript;

    //Called when the user clicks on a settlement. Click is detected if this script is enabled on the settlement clicked.
    public void OnMouseDown()
    {
        MainScript.playerInfo.nameOfSetUpgraded[MainScript.playerInfo.currentPlayer - 1].Add(gameObject.name);
        
//----------------------------------------------------------------------------------------------------------------------------------
        //Creates a city and places it related to the clicked objects location.
        var newObject = GameObject.Instantiate(cityPrefabs[MainScript.playerInfo.currentPlayer - 1]);
        newObject.transform.position = gameObject.transform.position + new Vector3(0, 0, .17f);
        newObject.transform.SetParent(MainScript.gameBoard.transform);

        //Find spcific tile data related to the settlement and add 1 to the z. (z = production amount)
        for (int i = 0; i < refToTileAccess.Count; i++)
        {
            foreach(Vector3 vec in MainScript.playerInfo.playersResTiles[MainScript.playerInfo.currentPlayer - 1])
            {
                
                if(refToTileAccess[i] == vec)
                {
                    int index = MainScript.playerInfo.playersResTiles[MainScript.playerInfo.currentPlayer - 1].IndexOf(vec);
                    MainScript.playerInfo.playersResTiles[MainScript.playerInfo.currentPlayer - 1][index] += new Vector3(0, 0, 1);
                    break;
                } 
                
            }
        }

        MainScript.playerInfo.playersSettlements[MainScript.playerInfo.currentPlayer - 1].Remove(gameObject);
        
        Destroy(gameObject);
//----------------------------------------------------------------------------------------------------------------------------------
        //If not loading a save, add a victory point, and disable the ablitiy to upgrade.
        if (!SetupScript.inLoadProcess) { 
            MainScript.playerInfo.playersInfo[MainScript.playerInfo.currentPlayer - 1][5] += 1;
            MainScript.setIndicators();
            MainScript.checkForWin();

            foreach (GameObject settlement in MainScript.playerInfo.playersSettlements[MainScript.playerInfo.currentPlayer - 1])
            {
                settlement.GetComponent<BoxCollider>().enabled = false;
            }
        }
//----------------------------------------------------------------------------------------------------------------------------------
        MainScript.setupHint("You can now roll the dice, collect resources around you, spend resources to expand, and trade for hard to get resources.", true);
        MainScript.logEvents("Player " + MainScript.playerInfo.currentPlayer + " upgraded a settlement to a city.");
    }

}
