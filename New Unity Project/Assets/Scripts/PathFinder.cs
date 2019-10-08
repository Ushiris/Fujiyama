/* Made by Ushiris */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    CheckPoint path;
    PlayerController player;
    bool isHit = false;

    // Start is called before the first frame update
    void Start()
    {
        player = gameObject.GetComponentInParent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isHit)
        {
            if (other.tag == "CheckPoint")
            {
                path = other.gameObject.GetComponent<CheckPoint>();
                player.from = path;
                player.to = (player.looking == LR.right) ? (path.NextCP) : (path.PrevCP);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag=="CheckPoint")
        {
            isHit = false;
        }
    }
}
