using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Proyecto26;
using UnityEngine.UI;

public class SaveLoadSystem : MonoBehaviour
{

    public class Post
    {
        public Scene json;
        /*public string BodyDof;
        public int userId;*/
    }
    //info from database fields
    public Scene loadThis;

    //int userID = 100;
    public void Start()
    {

        //JsonUtility.ToJson(SceneManager.GetActiveScene());

        //Scene sceneSave = SceneManager.CreateScene("SaveGame1");
        
        //getFromDatabase(101);


    
    }

    public void putToDatabase(string saveName)
    {

        string basePath = "https://bpacomp2020-default-rtdb.firebaseio.com";
        RequestHelper currentRequest;

        currentRequest = new RequestHelper
        {
            Uri = basePath + "/saves/" + saveName + ".json",

            Body = new Post
            {
                json = SceneManager.GetActiveScene()
                /*title = "foo",
                BodyDof = "bar",
                userId = 1*/
            }
            
        };
        RestClient.Put<Post>(currentRequest)
            .Then(res => Debug.Log(JsonUtility.ToJson(res, true)))
            .Catch(err => Debug.Log(err.Message));

    }

    public void getFromDatabase(string saveName)
    {
        string basePath = "https://bpacomp2020-default-rtdb.firebaseio.com";
        //Post post = new Post();
        RestClient.Get(basePath + "/saves/" + saveName + ".json")
        .Then(res => {
            Post post = new Post();
            post = JsonUtility.FromJson<Post>(res.Text);
            loadThis = post.json;

        }) 
        .Catch(err => Debug.Log(err.Message));

    }

    public GameObject pauseMenu;
    public GameObject saveUI;
    public InputField inputName;
    public void getSaveName()
    {
        pauseMenu.SetActive(false);
        saveUI.SetActive(true);
    }
    public void saveGame()
    {
        if(inputName != null)
        {
            pauseMenu.SetActive(true);
            saveUI.SetActive(false);

            putToDatabase(inputName.text);
        }
        else
        {
            //user didn't enter name
        }

    }
    public void getSave()
    {
        if (inputName != null)
        {
            pauseMenu.SetActive(true);
            saveUI.SetActive(false);

            getFromDatabase(inputName.text);
        }
        else
        {
            //user didn't enter name
        }
        
    }
    /*private void loadScene()
    {
        Scene savedGame = JsonUtility.FromJson<Scene>(scene);
        if(savedGame != null)
        {
            //load game scene
            SceneManager.LoadScene("Game");
            //Wait for load to finsh then merge
            SceneManager.MergeScenes(savedGame, SceneManager.GetActiveScene());
            //open scene
        }
    }*/

    public void Back()
    {
        pauseMenu.SetActive(true);
        saveUI.SetActive(false);
    }
}