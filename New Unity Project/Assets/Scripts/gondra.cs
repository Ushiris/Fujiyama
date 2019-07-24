using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class gondra : MonoBehaviour
{
    public Vector3 to;
    public float speed;
    public Vector3 Alighting;
    public bool IsKnowPL = false;
    public PlayerController player;

    Vector3 from;
    float timer = 0;
    float par_b = 0;
    bool IsJoinPL = false;
    bool IsGo = true;


    // Start is called before the first frame update
    void Start()
    {
        from = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsJoinPL)
        {
            timer += Time.deltaTime;
            if (timer > speed)
            {
                IsGo = true;
            }
            timer = (timer > speed) ? (speed - timer) : timer;

            float par = timer / (speed / 2);
            if (par > 1)
            {
                par = 2 - par;
                if (IsGo)
                {
                    GetOff();
                }
            }
            Vector3 add = (to - from) * (par - par_b);
            player.ForceMove(add);
            transform.position += add;
            par_b = par;
        }
    }

    public void GetOn()
    {
        IsJoinPL = true;
        player.GondraEnter();
    }

    public void GetOff()
    {
        IsJoinPL = false;
        player.GondraExit(Alighting);
        IsGo = false;
    }
}
