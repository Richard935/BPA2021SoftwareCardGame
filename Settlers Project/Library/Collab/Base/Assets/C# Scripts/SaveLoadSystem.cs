using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveLoadSystem : MonoBehaviour
{
    //public static bool SaveScene(SceneManagement.Scene scene, string dstScenePath = "", bool saveAsCopy = false);
    public void saveGame()
    {
        SceneManager.GetActiveScene();
        
    }
}
