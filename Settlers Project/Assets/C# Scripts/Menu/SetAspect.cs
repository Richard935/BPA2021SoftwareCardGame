using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetAspect : MonoBehaviour
{
    void Update()
    {
        //Sets attached game objects AspectRatioFitter equal to main cameras aspect.
        if (Camera.main.aspect != gameObject.GetComponent<AspectRatioFitter>().aspectRatio)
        {
            gameObject.GetComponent<AspectRatioFitter>().aspectRatio = Camera.main.aspect;
        }    
    }
}
