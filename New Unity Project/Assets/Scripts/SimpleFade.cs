using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleFade : MonoBehaviour
{
    public float FadeTime = 2f;//seconds
    public Image panel;
    public bool isWhite = false;
    bool isActive = false;
    float timer = 0f;
    

    // Update is called once per frame
    void Update()
    {
        if(isActive)
        {
            timer += Time.deltaTime;
            float par = timer / FadeTime;
            float col = isWhite ? 1 : 0;
            if(par>1)
            {
                par = 1;
            }
            panel.color = new Color(col, col, col, par);
        }
    }

    public void set(bool IO)
    {
        isActive = IO;
    }

    public void Setting(float duration, Image img, bool white = false)
    {
        FadeTime = duration;
        panel = img;
        isWhite = white;
    }
}
