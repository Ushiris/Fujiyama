using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//見ている向きについての情報です
public enum LR
{
    right,
    left
}

public class PlayerController : MonoBehaviour
{
    public CheckPoint CP { get; set; }
    public float speed = 1;
    public float JumpFouce;
    public GameObject Director;

    float DefaultSpeed;
    bool IsGround = false;
    Rigidbody rb;
    public LR looking;
    float angle;
    public CheckPoint from;
    public CheckPoint to;

    // Start is called before the first frame update
    void Start()
    {
        PathDirector pathDirector;
        DefaultSpeed = speed;
        rb = gameObject.GetComponent<Rigidbody>();
        pathDirector = Director.GetComponent<PathDirector>();
        CP = pathDirector.StartPoint;
        looking = LR.right;

        from = pathDirector.StartPoint;
        to = from.NextCP;
        rb.position = from.transform.position;
        Look(to);
        //rb.position += transform.TransformDirection(Vector3.forward) * 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            if (looking != LR.right)
            {
                Std.Swap<CheckPoint>(ref to, ref from);
                looking = LR.right;
                Look(to);
            }
            rb.position += transform.TransformDirection(Vector3.forward) * speed;
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            if (looking != LR.left)
            {
                Std.Swap<CheckPoint>(ref to, ref from);
                looking = LR.left;
                Look(to);
            }
            rb.position += transform.TransformDirection(Vector3.forward) * speed;
        }

        //ジャンプの処理。IsGroundは自作の変数であることに注意。
        if(Input.GetKeyDown(KeyCode.Space) && IsGround)
        {
            rb.AddForce(new Vector3(0, JumpFouce));
            speed /= 2;
        }
    }
    
    private void OnCollisionStay(Collision collision)
    {
        //地面に着地したということを確認します。
        if(collision.gameObject.tag=="Plane" && !IsGround)
        {
            IsGround = true;
            speed = DefaultSpeed;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        //地面を離れたということを確認します。
        if (collision.gameObject.tag == "Plane")
        {
            IsGround = false;
        }
    }

    public void Look(CheckPoint to)
    {
        Vector3 lookPos = new Vector3(to.transform.position.x, transform.position.y, to.transform.position.z);
        transform.LookAt(lookPos);
    }
}
