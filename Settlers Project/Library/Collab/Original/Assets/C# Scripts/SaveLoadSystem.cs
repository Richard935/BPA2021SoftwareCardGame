using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEditor;

public class SaveLoadSystem : MonoBehaviour
{

    /*static void Init()
    {
        SaveLoadSystem window = (SaveLoadSystem)EditorWindow.GetWindowWithRect(typeof(SaveLoadSystem), new Rect(0, 0, 200, 50));
        window.Show();
    }*/
    //public static bool SaveScene(SceneManagement.Scene scene, string dstScenePath = "", bool saveAsCopy = false);
    public void saveGame()
    {
        //change
        //EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), "Assets/Scenes/Save1.unity", true);
        bool saveOK = EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), EditorSceneManager.GetActiveScene().path);
        Debug.Log("Saved Scene " + (saveOK ? "OK" : "Error!"));
        //AssetDatabase.Refresh();
        //PrefabUtility.CreatePrefab("");
    }
}
