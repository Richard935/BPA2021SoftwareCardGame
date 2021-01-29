using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuStart : MonoBehaviour
{
    public GameObject music;
    public SettingsMenu settings;

    void Start()
    {
        //Occurs when the main menu is loaded.

        //Destroys one music object if there is two in the scene. 
        if(PlayerPrefs.GetInt("desMusic") == 1)
        {
            Destroy(music);
            PlayerPrefs.DeleteKey("desMusic");

            music = GameObject.Find("Music");
            settings.music = music.GetComponent<AudioSource>();
            settings.setVolume();
        }
    }
}
