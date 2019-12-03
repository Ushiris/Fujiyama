/* Made by Ushiris */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public GameObject player;
    PlayerController playerController;
    
    public Vector3 diff = new Vector3(0, 0, 0);
    Vector3 lookedPos;
    float ave_time = 1 / 60;

    // Start is called before the first frame update
    void Start()
    {
        playerController = player.GetComponent<PlayerController>();
        
        Vector3 toNext = playerController.GetCameraPosXZ(diff);
        toNext.y += diff.y;
        transform.position = toNext;

        transform.LookAt(player.transform);
    }
    
    void LateUpdate()
    {
        ave_time = ave_time + Time.deltaTime / 2;
        Vector3 toNext = playerController.GetCameraPosXZ(diff);
        toNext.y += diff.y;
        Vector3 toNextDiff= Vector3.Lerp(transform.position, toNext, Time.deltaTime);
        transform.position = toNextDiff;
        transform.position = new Vector3(transform.position.x, toNext.y, transform.position.z);
        
        transform.LookAt(player.transform.position);
    }
}
