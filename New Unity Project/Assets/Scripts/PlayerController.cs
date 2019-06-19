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
    public float speed = 1;
    public float JumpFouce;
    public GameObject Director;
    public LR looking;
    public CheckPoint from;
    public CheckPoint to;
    public List<KeyCode> right;
    public List<KeyCode> left;

    float DefaultSpeed;
    bool IsGround = false;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        DefaultSpeed = speed;
        rb = gameObject.GetComponent<Rigidbody>();
        Look(to);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsInput(right))
        {
            if (looking != LR.right)
            {
                Std.Swap<CheckPoint>(ref to, ref from);
                Look(to);
                looking = LR.right;
            }
            rb.position += transform.TransformDirection(Vector3.forward) * speed;
        }
        else if (IsInput(left))
        {
            if (looking != LR.left)
            {
                Std.Swap<CheckPoint>(ref to, ref from);
                Look(to);
                looking = LR.left;
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

    bool IsInput(List<KeyCode> keyCodes)
    {
        foreach(var a in keyCodes)
        {
            if(Input.GetKey(a))
            {
                return true;
            }
        }

        return false;
    }
}
