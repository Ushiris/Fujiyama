using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderActPos : MonoBehaviour
{
    public GameObject Another;
    public GameObject Ladder;
    public float duration;

    PlayerController player;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            player = other.gameObject.GetComponent<PlayerController>();
            player.isActionable = true;
            player.EventEffect = LadderEnter;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            player.isActionable = false;
        }
    }

    public void LadderEnter()
    {
        Vector3 to = Another.gameObject.transform.position;
        player.Look(Ladder.transform.position);
        player.LadderEnter(to,duration);
    }
}
