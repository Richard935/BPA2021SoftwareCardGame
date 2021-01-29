using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Holds save data. Described in database description.
public class SaveData
{
    public List<int> p1Resources;
    public List<string> p1ClickSPP;
    public List<string> p1UpgradeSet;
    public List<string> p1ClickRoads;

    public List<int> p2Resources;
    public List<string> p2ClickSPP;
    public List<string> p2UpgradeSet;
    public List<string> p2ClickRoads;

    public List<int> p3Resources;
    public List<string> p3ClickSPP;
    public List<string> p3UpgradeSet;
    public List<string> p3ClickRoads;

    public List<int> turnsTaken;
    public int currentPlayer;
    public bool isSetupPhase;

    public bool robberMoved;
    public List<float> robberCoords;
    public List<float> deactivatedTile;

    public bool diceRolled;
}
