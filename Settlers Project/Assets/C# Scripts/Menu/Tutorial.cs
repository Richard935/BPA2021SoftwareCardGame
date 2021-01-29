using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public Sprite[] slides;
    public Image imgBox;
    public Text txtPageLocation;
    public int pageIndex;

    public bool openTutorial;
    public GameObject TutorialMenu;
    public GameObject Pause;

    //-----------------------------------------------------------------------------------------------------------------------------------
    public void nextSlide()
    {
        //Move to next slide if not on last slide.
        if(pageIndex != 10)
        {
            pageIndex++;
            imgBox.sprite = slides[pageIndex];
            txtPageLocation.text = "Page " + (pageIndex + 1) + "/11";
        }
    }
    public void previousSlide()
    {
        //Move to previous slide if not on first slide.
        if (pageIndex != 0)
        {
            pageIndex--;
            imgBox.sprite = slides[pageIndex];
            txtPageLocation.text = "Page " + (pageIndex + 1) + "/11";
        }
    }
    //-----------------------------------------------------------------------------------------------------------------------------------
    public void moveBetweenMenus()
    {
        //Opens and closes the tutorial menu.
        if (openTutorial)
        {
            TutorialMenu.SetActive(true);
            Pause.SetActive(false);
        }
        else
        {
            TutorialMenu.SetActive(false);
            Pause.SetActive(true);
        }
    }
    //-----------------------------------------------------------------------------------------------------------------------------------
}
