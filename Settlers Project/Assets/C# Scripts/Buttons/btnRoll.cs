using UnityEngine;
using UnityEngine.UI;

public class btnRoll : MonoBehaviour
{

    public Sprite[] diceImages;
    public Image[] imgBoxesForDiceImages;
    public GameObject parentOfNumSprites;

    public MainGame MainScript;
    public SaveLoadSystem SaveLoad;
    
    //When player rolls.
    public void OnRollClick()
    {
        //-------------------------------------------------------------------------
        //Uses two random number generator to set two dice image boxes.
        System.Random random = new System.Random();
        int roll;
        int sumOfRoll = 0;

        roll = random.Next(1, 7);
        sumOfRoll += roll;
        imgBoxesForDiceImages[0].sprite = diceImages[(roll - 1)];

        roll = random.Next(1, 7);
        sumOfRoll += roll;
        imgBoxesForDiceImages[1].sprite = diceImages[(roll - 1)];

        imgBoxesForDiceImages[0].gameObject.SetActive(true);
        imgBoxesForDiceImages[1].gameObject.SetActive(true);
        //-------------------------------------------------------------------------

        if (sumOfRoll == 7)
        {
            //Start of Robber event. Allows the number pieces to be clicked.
            BoxCollider[] numSpritesBoxColliders = parentOfNumSprites.GetComponentsInChildren<BoxCollider>();
            foreach(var boxCollider in numSpritesBoxColliders)
            {
                boxCollider.enabled = true;
            }
            
            MainScript.setupHint("The black piece is called the robber. Since you rolled a 7 you must move the robber to another tile by clicking on a number piece.", true);
            MainScript.logEvents("A 7 was rolled.");
        }
        else
        {
            //Allocates resources for the total rolled.
            roll--;
            MainScript.AllocateResources(sumOfRoll);
            MainScript.btnEndTurn.gameObject.SetActive(true);
        }

        MainScript.btnRollDice.gameObject.SetActive(false);
        SaveLoad.playerRolled = true;
    }

}
