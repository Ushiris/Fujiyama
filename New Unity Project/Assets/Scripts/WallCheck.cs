using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCheck : MonoBehaviour
{
    public PlayerController player;

    private void OnTriggerEnter(Collider other)
    {
        if(!other.isTrigger)
        {
            player.Bind();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        player.React();
    }
}
