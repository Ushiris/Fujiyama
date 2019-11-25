using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRepeatorCircle : MonoBehaviour
{
    public Vector3 to = Vector3.zero;
    public float speed = 1f;
    public Vector3 center;

    PlayerController player;
    Vector3 before;
    Vector3 DefaultPosition;
    float progress = 0f;

    bool isRide = false;
    bool isActive = false;
    bool isActionable = true;
    bool isDefPos = true;

    // Start is called before the first frame update
    void Start()
    {
        before = transform.position;
        DefaultPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            progress += speed / 100;
            Vector3 from = isDefPos ? DefaultPosition : to;
            Vector3 target= isDefPos ? to : DefaultPosition;
            Vector3.Slerp(from, target, progress);
            transform.LookAt(center);
            player.ForceMove(transform.position - before);

            if(progress>=1f)
            {
                isActive = false;
                isActionable = true;
                progress = 0f;
            }
        }

        before = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            player = collision.gameObject.GetComponent<PlayerController>();
            isRide = true;
        }
        
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            isRide = false;
            player.EventEffect = Action;
        }
    }

    public void Action()
    {
        if (isActionable)
        {
            isActive = true;
            progress = 0f;
            isActionable = false;
        }
    }
}
