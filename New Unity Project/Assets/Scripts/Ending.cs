using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ending : MonoBehaviour
{
    public PlayerController player;
    public Vector3 DeskPos;
    public float AnimDuration;
    bool isActive = false;
    FadeOut fade;

    // Start is called before the first frame update
    void Start()
    {
        if (GameDirector.IsClearGame())
        {
            isActive = true;
            player.SetGoWalkMode(DeskPos, AnimDuration);
            
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
