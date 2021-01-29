using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobberMove : MonoBehaviour 
{

    public MainGame MainScript;

    //x = index of resource type, y = number on tile, z = addition amount (1 or 2)
    public Vector3 TileInfo;

    public GameObject parentOfNumSprites;
    public GameObject robber;

    //When number tile is clicked.
    public void OnMouseDown()
    {

        MainScript.setupHint("The robber stops resource production of the tile it resides. Also when moved if another player resides on the tile the robber is moved " +
            "to, you can steal a random resource from that player.", true);
        MainScript.txtHint.fontSize = 12;

        MainScript.logEvents("Robber was moved.");

        //Reatives the previous tile the robber was on.
        if(MainScript.tileRobberIsOn != null)
        {
            MainScript.tileRobberIsOn.SetActive(true);
        }     

        //Sets robber position.
        robber.transform.position = gameObject.transform.position;

        //Sets a refrence to the newly deactivated tile.
        MainScript.refrenceToDeactivatedTile = TileInfo;

        //Disables all the box colliders in the number peices.
        BoxCollider[] numSpritesBoxColliders = parentOfNumSprites.GetComponentsInChildren<BoxCollider>();
        foreach (var boxCollider in numSpritesBoxColliders)
        {
            boxCollider.enabled = false;
        }

        //Check if other players has same vector, if so then allow player to steal from that player.

        MainScript.playerInfo.robberMoved = true;

        MainScript.tileRobberIsOn = gameObject;

        //Start the stealing process.
        MainScript.StealingResource();

        gameObject.SetActive(false);
        MainScript.btnEndTurn.gameObject.SetActive(true);
    }

}
