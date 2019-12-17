using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Omake : MonoBehaviour
{
    Image image;

    private void Start()
    {
        image = GetComponent<Image>();

        if(!GameDirector.isCleared)
        {
            image.color = Color.clear;
        }
    }
}
