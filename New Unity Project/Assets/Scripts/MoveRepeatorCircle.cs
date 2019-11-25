using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRepeatorCircle : MonoBehaviour
{
    public Vector3 to = Vector3.zero;
    public float speed = 1f;
    Vector3 before;
    Vector3 DefaultPosition;
    PlayerController player;
    bool isRide = false;
    bool isActive = false;
    bool isActionable = true;
    float progress = 0f;
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            player = other.GetComponent<PlayerController>();
            isRide = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag=="Player")
        {
            isRide = false;
        }
    }

    public void Action()
    {
        isActive = true;
        progress = 0f;
        isActionable = false;
    }
}
