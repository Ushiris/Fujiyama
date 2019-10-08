/* Made by Ushiris */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRepeater : MonoBehaviour
{
    public Vector3 to;
    public float seconds = 3.0f;
    PlayerController player;
    float timer = 0;
    float harfTime;
    Vector3 from;
    bool IsAbovePL = false;
    bool plKnow = false;
    float par_b = 0;

    // Start is called before the first frame update
    void Start()
    {
        from = transform.position;
        harfTime = seconds / 2;
    }

    // Update is called once per frame
    void Update()
    {
        //次フレームの座標の決定
        timer += Time.deltaTime;
        timer = (timer > seconds) ? (timer - seconds) : timer;
        float par = ((timer > harfTime) ? (seconds - timer) : timer) / harfTime;
        float x = (to.x - from.x) * (par - par_b);
        float y = (to.y - from.y) * (par - par_b);
        float z = (to.z - from.z) * (par - par_b);
        par_b = par;

        Vector3 add = new Vector3(x, y, z);
        transform.position += add;

        if (IsAbovePL)
        {
            player.ForceMove(add);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            IsAbovePL = true;

            //プレイヤーを記憶する処理
            if (!plKnow)
            {
                GameObject playerGO = collision.gameObject;
                player = playerGO.GetComponent<PlayerController>();
                plKnow = true;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            IsAbovePL = false;
        }
    }
}
