using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowImage : MonoBehaviour
{
    public Sprite image;
    public Image frame;
    bool isFade = false;
    float timer = 0;

    // Update is called once per frame
    void Update()
    {
        if (isFade)
        {
            timer += Time.deltaTime;
            if(timer<1f)
            {
                frame.color = new Color(1f, 1f, 1f, timer);
            }
            if (timer >= 2f)
            {
                frame.color = new Color(1f, 1f, 1f, 1 - (timer - 2f) / 2);
            }
            if (timer >= 6f)
            {
                timer = 0f;
                Destroy(gameObject);
                isFade = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            frame.sprite = image;
            frame.color = new Color(1f,1f,1f,0f);
            isFade = true;
        }
    }
}
