using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GondraMove : MonoBehaviour
{
    public Vector3 to = Vector3.zero;
    public float speed = 1f;
    public bool isCircle;//球面線形補完に切り替えるトグルスイッチのようにお使いください
    public Vector3 center;
    public Vector3 RotateAxis = new Vector3(0, 1, 0);
    public float RotateAngle = 90f;

    PlayerController player;
    Vector3 before;
    Vector3 DefaultPosition;
    Vector3 from;
    Vector3 target;
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
        from = transform.position;
        target = to;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            progress += speed / 100;
            Vector3 next = isCircle ? Vector3.Slerp(from, target, progress) : Vector3.Lerp(from, target, progress);
            transform.position = next;

            if (isCircle)
            {
                center.y = transform.position.y;
                transform.LookAt(center);
                transform.Rotate(RotateAxis, RotateAngle);
            }
            
            player.ForceMove(transform.position - before);

            if (progress >= 1f)
            {
                isActive = false;
                isActionable = true;
                isDefPos = !isDefPos;
                progress = 0f;
                from = isDefPos ? DefaultPosition : to;
                target = isDefPos ? to : DefaultPosition;
                player.GondraExit();
            }
        }

        before = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            player = collision.gameObject.GetComponent<PlayerController>();
            player.isActionable = true;
            player.EventEffect = Action;

        }
        
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            player.isActionable = false;
        }
    }

    public void Action()
    {
        if (isActionable)
        {
            isActive = true;
            progress = 0f;
            isActionable = false;
            player.GondraEnter();
        }
    }
}
