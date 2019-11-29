using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddImage : MonoBehaviour
{
    public Sprite sprite;
    public Image child_img;

    // Start is called before the first frame update
    void Start()
    {
        child_img.sprite = sprite;
        child_img.color = new Color(1, 1, 1, 0);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
