using System;
using UnityEngine.UI;
using UnityEngine;

public class SliderChange : MonoBehaviour
{
    public MainGame MainScript;
    public Slider sliChanged;
    public int resourceIndex;
    public void updateWithSlider()
    {

        //Set amount selected and set text object that goes with the slider.
        MainScript.eachAmountSelected[resourceIndex] = Convert.ToInt32(sliChanged.value);
        MainScript.txtOnRemoUI[(resourceIndex + 2)].text = MainScript.strPreset[resourceIndex] + sliChanged.value;

    }
}
