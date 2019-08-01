/* Made by Ushiris */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//見ている向き
public enum LR
{
    right,
    left
}

public class PlayerController : MonoBehaviour
{
    //動作のパラメータ
    public float speed = 1;
    public float LadderSpeed = 1;
    public float JumpFouce;
    
    //初期設定
    public LR looking;
    public CheckPoint from;
    public CheckPoint to;

    //入力の設定
    public List<KeyCode> right;
    public List<KeyCode> left;

    //キャラクターのステート
    public bool IsGondra = false;
    bool IsGround = false;
    bool IsLadder = false;
    bool IsCollision = false;

    //コンポーネント置き場
    Rigidbody rb;

    //変数の保存や一時的な記録
    float DefaultSpeed;
    LR looked = LR.right;

    //debug
    Vector3 def_p;
    Quaternion def_q;

    // Start is called before the first frame update
    void Start()
    {
        DefaultSpeed = speed;
        rb = gameObject.GetComponent<Rigidbody>();
        Look(to);
        def_p = transform.position;
        def_q = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = Vector3.zero;

        if (IsInput(right))
        {
            if (looking != LR.right)
            {
                Std.Swap(ref to, ref from);
                looking = LR.right;
            }

            Look(to);

            if (IsLadder)
            {
                ForceMove(transform.TransformDirection(Vector3.up) * speed);
            }
            else if (IsMovable())
            {
                ForceMove(transform.TransformDirection(Vector3.forward) * speed);
            }
        }
        else if (IsInput(left))
        {
            if (looking != LR.left)
            {
                Std.Swap(ref to, ref from);
                looking = LR.left;
            }

            Look(to);

            if (IsLadder && !IsGround)
            {
                ForceMove(transform.TransformDirection(Vector3.down) * speed);
            }
            else if (IsMovable())
            {
                ForceMove(transform.TransformDirection(Vector3.forward) * speed);
            }
        }

        //ジャンプの処理。IsGroundは自作の変数であることに注意。
        if (Input.GetKeyDown(KeyCode.Space) && IsCanJamp())
        {
            rb.AddForce(new Vector3(0, JumpFouce));
            speed /= 2;
        }

        //debug
        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.SetPositionAndRotation(def_p, def_q);
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        //地面以外に当たっているということの確認
        if(collision.gameObject.tag!="Plane")
        {
            IsCollision = true;
            looked = looking;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //はしごに足をかけているかどうかの判定
        if (other.tag == "Ladder")
        {
            IsLadder = true;
            speed = LadderSpeed;
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        //梯子を下りた時の処理
        if (other.tag == "Ladder")
        {
            IsLadder = false;
            speed = DefaultSpeed;
            rb.useGravity = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        //衝突の解消
        if (collision.gameObject.tag != "Plane")
        {
            IsCollision = false;
        }
    }

    //CheckPoint用のLookAt関数。高さを無視します。
    public void Look(CheckPoint to)
    {
        Vector3 lookPos = new Vector3(to.transform.position.x, transform.position.y, to.transform.position.z);
        transform.LookAt(lookPos);
    }

    //二つ以上のキー入力をif内に書くと汚いのでまとめました
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

    //強制的にpositionを+addします
    public void ForceMove(Vector3 add)
    {
        transform.position += add;
    }

    public void Ground(bool tf)
    {
        IsGround = tf;
        if(tf)
        {
            speed = DefaultSpeed;
        }
    }

    public bool IsMovable()
    {
        return (!IsCollision || looked != looking) && !IsGondra;
    }

    public bool IsCanJamp()
    {
        return IsGround && !IsLadder;
    }

    public void GondraEnter()
    {
        IsGondra = true;
        rb.velocity = Vector3.zero;
        rb.useGravity = false;
    }

    public void GondraExit(Vector3 Alighting)
    {
        rb.AddForce(Alighting);
        IsGondra = false;
        rb.useGravity = true;
    }
}
