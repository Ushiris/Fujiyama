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
            player.CheckPos = path;
            player.CpWorldPos = path.transform.position;
            Debug.Log("hit");
        }
    }
}
