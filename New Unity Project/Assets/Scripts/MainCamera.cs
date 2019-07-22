using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public GameObject player;
    PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        playerController = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤーの方向と同一の方向を向き、補正をかけます。
        transform.rotation =player.transform.rotation;
        if (playerController.looking == LR.right)
        {
            transform.Rotate(new Vector3(0, 270, 0));

        }
        else
        {
            transform.Rotate(new Vector3(0, 90, 0));
        }

        //プレイヤーと同一の座標へ向かい、そこから離れます。
        transform.position = player.transform.position;
        transform.Translate(0, 2, -4, Space.Self);
        transform.LookAt(player.transform);
    }
}
