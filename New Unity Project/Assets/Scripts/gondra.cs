using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gondra : MonoBehaviour
{
    public Vector3 to;
    public float speed;


    Vector3 from;
    float timer = 0;
    float par_b = 0;
    bool IsJoinPL = false;
    bool IsKnowPL = false;
    PlayerController player;


    // Start is called before the first frame update
    void Start()
    {
        from = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(IsJoinPL)
        {
            timer += Time.deltaTime;
            float par = timer / speed;
            Vector3 add = to - from;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            IsJoinPL = true;

            if (!IsKnowPL)
            {
                player = collision.gameObject.GetComponent<PlayerController>();
                IsKnowPL = true;
            }
        }
    }
}
