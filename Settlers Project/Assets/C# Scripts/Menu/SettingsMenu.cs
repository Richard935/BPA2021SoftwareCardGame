using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//This class controls opening and closing the settings menu, volume change, and volume on and off.
public class SettingsMenu : MonoBehaviour
{

    public GameObject menuBeforSettings;
    public GameObject settingsMenu;
    public bool toSettingsMenu;

    //Moves between settings and main menu or pause menu.
    public void showSettings()
    {
        menuBeforSettings.SetActive(!toSettingsMenu);
        settingsMenu.SetActive(toSettingsMenu);
    }

    //Sets volume of audio source.
    public void setVolume()
    {
        sliVolume.value = music.volume * 100;
        txtVolume.text = "Volume = " + sliVolume.value.ToString() + "%";
    }

    public Text txtVolume;
    public AudioSource music;
    public Slider sliVolume;

    //Called when the user changes the selected value of sliVolume.
    public void volumeChange()
    {
        //Displays volume percentage and sets the volume of the music accordingly.
        txtVolume.text = "Volume = "+sliVolume.value.ToString()+"%";
        music.volume = sliVolume.value / 100;
    }

    //Called when the user clicks the musicOnOff button and mutes or unmutes the music.
    public void musicOnOff()
    {
        if (music.mute)
        {
            music.mute = false;
        }
        else
        {
            music.mute = true;
        }
    }

}
