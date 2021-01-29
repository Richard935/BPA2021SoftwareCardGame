using UnityEngine;
using System.Collections.Generic;

//Class that holds all player information and game data.
public class PlayerVariables
{

    //     (Hold each players list)  (Player 1 Res.  )  (Player 2 Res. ) (Player 3 Res.  ) Resource list order = wood, brick, ore, wheat, sheep, VP
    public List<int>[] playersInfo = {new List<int>(6), new List<int>(6), new List<int>(6)};
    //Turns each player has taken.
    public int[] turnsTaken = { 0, 0, 0 };

    //Holds each players settlement objects.
    public List<GameObject>[] playersSettlements = {new List<GameObject>(), new List<GameObject>(), new List<GameObject>()};
    //Each Vector = resourceNum, number on tile, give amount
    public List<Vector3>[] playersResTiles = {new List<Vector3>(), new List<Vector3>(), new List<Vector3>()};

    //Placements points access that each player has.
    public List<GameObject>[] playersRoadAccess = {new List<GameObject>(), new List<GameObject>(), new List<GameObject>()};
    public List<GameObject>[] playersSettlementAccess = { new List<GameObject>(), new List<GameObject>(), new List<GameObject>() };

    //Hold data for saving and loading system.
    public List<string>[] nameOfClickedRP = { new List<string>(), new List<string>(), new List<string>() };
    public List<string>[] nameOfClickedSPP = { new List<string>(), new List<string>(), new List<string>() };
    public List<string>[] nameOfSetUpgraded = { new List<string>(), new List<string>(), new List<string>() };
    public int[] indexForSetNames = {0, 0, 0};

    public int currentPlayer;
    
    public bool isSetupPhase;
    public bool robberMoved;

}

