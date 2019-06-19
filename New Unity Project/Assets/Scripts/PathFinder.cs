using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    CheckPoint path;
    PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        player = gameObject.GetComponentInParent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "CheckPoint")
        {
            path = other.gameObject.GetComponent<CheckPoint>();
            player.from = path;
            player.to = (player.looking == LR.right) ? (path.NextCP) : (path.PrevCP);
            player.Look(player.to);
        }
    }
}
