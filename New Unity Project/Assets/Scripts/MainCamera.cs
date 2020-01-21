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
    
    //プレイヤーが動いた後に更新する
    void LateUpdate()
    {
        //カメラがあるべき目標地点を取得し、いい感じに遅れる。
        Vector3 toNext = playerController.GetCameraPosXZ(diff);
        toNext.y += diff.y;
        Vector3 toNextDiff = Vector3.Lerp(transform.position, toNext, 1 / 60f);
        transform.position = toNextDiff;
        transform.position = new Vector3(transform.position.x, toNext.y, transform.position.z);

        //いい感じに滑らか動作をさせる
        rb.velocity = playerController.rb.velocity * 0.85f;
        
        //プレイヤーを画面の中心にとらえる
        transform.LookAt(player.transform.position);
    }
}
