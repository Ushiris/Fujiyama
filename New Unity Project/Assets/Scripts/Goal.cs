using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Goal : MonoBehaviour
{
    public float FadeTime = 2f;//seconds
    public Image panel;
    bool isActive;
    float timer = 0f;

    // Update is called once per frame
    void Update()
    {
        if(isActive)
        {
            timer += Time.deltaTime;
            panel.color = new Color(0f, 0f, 0f, timer / FadeTime);

            if (timer > FadeTime)
            {
                isActive = false;
                timer = 0;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            isActive = true;
            Invoke("ReturnToSelect", FadeTime + 0.1f);
        }
    }

    private void ReturnToSelect()
    {
        GameDirector.OpenScene("Select");
    }
}
