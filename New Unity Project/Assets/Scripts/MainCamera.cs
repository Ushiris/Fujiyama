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

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        playerController = player.GetComponent<PlayerController>();
        rb = gameObject.AddComponent<Rigidbody>();

        rb.velocity = Vector3.zero;
        rb.useGravity = false;
        
        Vector3 toNext = playerController.GetCameraPosXZ(diff);
        toNext.y += diff.y;
        transform.position = toNext;

        transform.LookAt(player.transform);
    }
    
    void LateUpdate()
    {
        Vector3 toNext = playerController.GetCameraPosXZ(diff);
        toNext.y += diff.y;
        Vector3 toNextDiff = Vector3.Lerp(transform.position, toNext, 1 / 60f);
        transform.position = toNextDiff;
        transform.position = new Vector3(transform.position.x, toNext.y, transform.position.z);

        rb.velocity = playerController.rb.velocity * 0.85f;
        
        transform.LookAt(player.transform.position);
    }
}
