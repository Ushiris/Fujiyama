/* Made by Ushiris */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public GameObject player;
    PlayerController playerController;

    public chaser pivot;
    public Vector3 diff=new Vector3(0, 2, -4);

    // Start is called before the first frame update
    void Start()
    {
        playerController = player.GetComponent<PlayerController>();
        //プレイヤーと同一の座標へ向かい、そこから離れます。
        transform.position = pivot.target.transform.position;
        transform.Translate(diff, Space.Self);
    }
    
    void LateUpdate()
    {
        transform.LookAt(pivot.target.transform);
    }
}
