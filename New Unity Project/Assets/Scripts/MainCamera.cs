using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public GameObject player;
    Vector3 gap;
    Vector3 targetPos;
    PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        targetPos = player.transform.position;
        gap = transform.position - targetPos;
        playerController = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        targetPos = player.transform.position;
        transform.rotation =player.transform.rotation;
        if (playerController.looking == LR.right)
        {
            transform.Rotate(new Vector3(0, 270, 0));

        }
        else
        {
            transform.Rotate(new Vector3(0, 90, 0));
        }
        transform.position = targetPos;
        transform.Translate(0, 0, -5, Space.Self);
    }
}
