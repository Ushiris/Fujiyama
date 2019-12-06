using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    public float FadeTime = 2f;//seconds
    public Image panel;
    public string scene="Title";
    public bool isAlone = true;//単体で動作させる場合にはtrue
    public bool isWhite = false;
    bool isActive = false;
    float timer = 0f;

    // Update is called once per frame
    void Update()
    {
        if(isActive)
        {
            timer += Time.deltaTime;
            float rgb =isWhite ? 1 : 0;
            panel.color = new Color(rgb,rgb,rgb,timer/FadeTime);

            if (timer > FadeTime)
            {
                isActive = false;
                timer = 0;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player"&&isAlone)
        {
            FadeStart();
        }
    }

    public void FadeStart()
    {
        isActive = true;
        Invoke("GoToNextScene", FadeTime + 0.1f);
    }

    private void GoToNextScene()
    {
        GameDirector.OpenScene(scene);
    }
}
