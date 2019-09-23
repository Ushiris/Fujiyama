using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//見ている向き
public enum LR
{
    right,
    left,
    none
}

public class PlayerController : MonoBehaviour
{
    //動作のパラメータ
    public float speed = 1;
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
    GameObject body;

    //変数の初期値の保存や一時的な記録
    float DefaultSpeed;

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
        LR InputLR = InputLorR();

        //左右の動作
        if (InputLR != LR.none)
        {
            float x = 0, y, z = 0;
            Vector3 moveArrow;

            if (looking != InputLR)
            {
                Std.Swap(ref to, ref from);
                looking = InputLR;
            }
            Look(to);

            if (!IsGondra)
            {
                if (!IsLadder)
                {
                    moveArrow = Vector3.forward;
                    y = rb.velocity.y;
                }
                else
                {
                    moveArrow = InputLR == LR.right ? Vector3.up : Vector3.down;
                    y = (transform.TransformDirection(moveArrow) * speed).y;
                }
                x = (transform.TransformDirection(moveArrow) * speed).x;
                z = (transform.TransformDirection(moveArrow) * speed).z;

                rb.velocity = new Vector3(x, y, z);
            }
        }
        else
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
        
        //ジャンプの処理。IsGroundは自作の変数であることに注意。
        if (Input.GetKeyDown(KeyCode.Space) && IsCanJamp())
        {
            rb.AddForce(new Vector3(0, JumpFouce));
        }

        //debug
        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.SetPositionAndRotation(def_p, def_q);
        }
    }

    //Collision判定（判定相手がisTriggerを持ってない場合に呼び出されます）
    private void OnCollisionEnter(Collision collision)
    {
        //はしごに足をかけ始めたどうかの判定
        if (collision.gameObject.tag == "Ladder")
        {
            IsLadder = true;
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        //はしごに足をかけているかどうかの判定
        if (collision.gameObject.tag == "Ladder")
        {
            IsLadder = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        //梯子を下りた時の処理
        if (collision.gameObject.tag == "Ladder")
        {
            IsLadder = false;
            speed = DefaultSpeed;
            rb.useGravity = true;
        }
    }
    
    //CheckPoint用のLookAt関数。高さを無視します。
    public void Look(CheckPoint to)
    {
        Vector3 lookPos = new Vector3(to.transform.position.x, transform.position.y, to.transform.position.z);
        transform.LookAt(lookPos);
    }

    //二つ以上のキー入力をif内に書くと汚いのでまとめました
    LR InputLorR()
    {
        foreach (var a in right)
        {
            if (Input.GetKey(a))
            {
                return LR.right;
            }
        }

        foreach(var a in left)
        {
            if (Input.GetKey(a))
            {
                return LR.left;
            }
        }

        return LR.none;
    }

    //強制的にpositionを+addします
    public void ForceMove(Vector3 add)
    {
        transform.position += add;
    }

    //地面との接触、離脱時に呼ぶ関数。
    public void Ground(bool hitG)
    {
        IsGround = hitG;
        speed = hitG ? DefaultSpeed : (speed * 2 / 3);
    }

    //ステート系
    public bool IsCanJamp() { return IsGround && !IsLadder; }

    //ゴンドラ関係。
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