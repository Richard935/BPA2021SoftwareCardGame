using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Proyecto26;

public class test : MonoBehaviour
{
    public class Post
    {
        public string title;
        public string BodyDof;
        public int userId;
    }
    //info from database fields
    public string title;

    //int userID = 100;
    public void Start()
    {

        Scene sceneSave = SceneManager.CreateScene("saveGame1");


        putToDatabase(1);
    
    }
    public void putToDatabase(int saveIndex)
    {
        string basePath = "https://bpacomp2020-default-rtdb.firebaseio.com";
        RequestHelper currentRequest;

        currentRequest = new RequestHelper
        {
            Uri = basePath + "/saves/" + saveIndex + ".json",
            Body = SceneManager.GetActiveScene()
            /*new Post
            {
                title = "foo",
                BodyDof = "bar",
                userId = 1
            }*/
        };
        RestClient.Put<Scene>(currentRequest)
            .Then(res => Debug.Log(JsonUtility.ToJson(res, true)))
            .Catch(err => Debug.Log(err.Message));
    }

    public void getFromDatabase(int userID)
    {
        string basePath = "https://bpacomp2020-default-rtdb.firebaseio.com";
        //Post post = new Post();
        RestClient.Get(basePath + "/posts/" + userID + ".json")
        .Then(res => {
            Post post = new Post();
            post = JsonUtility.FromJson<Post>(res.Text);
            title = post.title;
        }) 
        .Catch(err => Debug.Log(err.Message));

    }
}
