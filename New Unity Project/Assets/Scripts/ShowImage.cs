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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isFade)
        {
            timer += Time.deltaTime;
            if (timer >= 1f)
            {
                frame.color = new Color(1f, 1f, 1f, 1 - (timer - 1f) / 2);
            }
            if (timer >= 4f)
            {
                timer = 0f;
                isFade = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            frame.sprite = image;
            frame.color = new Color(1f,1f,1f,1f);
            isFade = true;
        }
    }
}
