﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//見ている向き
public enum LR
{
    right,
    left
}

//入力のリスト
public enum Commands
{
    right,
    left,
    jump,
    action,
    exit,
    debug,
    d_respawn,
    END,
}

public class PlayerController : MonoBehaviour
{
    #region variables
    //定数
    private const KeyCode ExitKey = KeyCode.Escape;
    private const KeyCode DebugKey = KeyCode.B;
    private const float JumpCoolTime = 0.3f;

    //動作のパラメータ
    public float speed;
    public float JumpFouce;

    //初期設定が必要な変数
    public LR looking;
    public CheckPoint from;
    public CheckPoint to;
    public SaveManager SavePoint;
    public Animator anim;
    public AudioClip jumpSE;

    //入力の設定
    public List<KeyCode> right = new List<KeyCode> { KeyCode.D, KeyCode.RightArrow };
    public List<KeyCode> left = new List<KeyCode> { KeyCode.A, KeyCode.LeftArrow };
    public List<KeyCode> jump = new List<KeyCode> { KeyCode.Space };
    public List<KeyCode> action = new List<KeyCode> { KeyCode.W };
    public List<KeyCode> exit = new List<KeyCode> { KeyCode.Escape };
    readonly List<KeyCode> debug = new List<KeyCode> { KeyCode.B };
    readonly List<KeyCode> d_respawn = new List<KeyCode> { KeyCode.R };

    //キャラクターのステート
    bool IsGondra = false;
    bool IsGround = false;
    bool IsLadder = false;
    bool IsJumping = false;
    bool[] PlayerInput = { false, };
    bool LRmoved = false;
    public bool isActionable = false;
    float JumpTimer = 0.3f;
    bool IsPause { get; set; }

    //コンポーネント置き場
    Rigidbody rb;
    AudioSource A_source;

    //リセットされる可能性がある変数の初期値
    float DefaultSpeed;

    //前フレームの入力状態の保存
    bool[] beforeInput = { false };

    //アクションボタンで続行されるアクション
    public delegate void Action();
    public Action EventEffect;

    //debug
    Vector3 def_p;
    Quaternion def_q;
    CheckPoint def_f_CP;
    CheckPoint def_t_CP;
    LR def_l;
    #endregion
    
    // Start is called before the first frame update
    void Start()
    {
        DefaultSpeed = speed;
        rb = gameObject.GetComponent<Rigidbody>();
        A_source = gameObject.GetComponent<AudioSource>();
        Look(to);
        def_p = transform.position;
        def_q = transform.rotation;
        def_f_CP = from;
        def_t_CP = to;
        def_l = looking;
        InputCheck();
        SetAnimStates(true);
    }

    // Update is called once per frame
    void Update()
    {
        //ジャンプのクールタイム処理
        if (IsJumping)
        {
            JumpTimer -= Time.deltaTime;
            if (JumpTimer <= 0f)
            {
                IsJumping = false;
                JumpTimer = JumpCoolTime;
            }
        }

        if (Mathf.Abs(rb.velocity.y) > 0.05f)
        {
            IsJumping = true;
        }

        LRmoved = false;

        //プレイヤーからの入力を受け取る
        InputCheck();

        //ゲームの終了
        if (PlayerInput[(int)Commands.exit])
        {
            GameDirector.ShatDown();
        }

        if (PlayerInput[(int)Commands.action])
        {
            EventEffect();
        }

        //左右に動く
        if (PlayerInput[(int)Commands.right] || PlayerInput[(int)Commands.left])
        {
            Move(PlayerInput[(int)Commands.right] ? LR.right : LR.left);
            LRmoved = true;
        }

        //ジャンプする
        if (PlayerInput[(int)Commands.jump])
        {
            IsJumping = true;
            Invoke("Jump", 0.23f);
        }

        //左右の入力なしなら、左右の速度を減衰させる
        if (!LRmoved)
        {
            float x = Mathf.Abs(rb.velocity.x * 4 / 5f) < 0.05f ? (0.0f) : (rb.velocity.x * 4 / 5f);
            float z = Mathf.Abs(rb.velocity.z * 4 / 5f) < 0.05f ? (0.0f) : (rb.velocity.z * 4 / 5f);
            rb.velocity = new Vector3(x, rb.velocity.y, z);
        }

        //回転力の消去
        rb.angularVelocity = Vector3.zero;

        //アニメーションにプレイヤーの状態を送信
        SetAnimStates();

        //debug code
        if (Input.GetKeyDown(DebugKey))
        {
            DebugReset();
        }
        if (PlayerInput[(int)Commands.d_respawn])
        {
            Respawn();
        }
    }

    //プレイヤーキャラクターをInputLRに動かします。
    private bool Move(LR InputLR)
    {
        //体の向きの変更
        if (looking != InputLR)
        {
            Std.Swap(ref to, ref from);
            looking = InputLR;
        }
        Look(to);

        //動作する向きの設定
        float x = 0, y, z = 0;
        Vector3 moveArrow = Vector3.forward;

        if (IsGround)
        {
            moveArrow = Vector3.forward;
        }
        if (IsLadder && InputLR == LR.right)
        {
            moveArrow = Vector3.up;
        }
        else if (IsLadder && InputLR == LR.left && !IsGround)
        {
            moveArrow = Vector3.down;
        }

        //動作する速度の設定
        x = (transform.TransformDirection(moveArrow) * speed).x;
        y = IsLadder ? (transform.TransformDirection(moveArrow)).y : rb.velocity.y;
        z = (transform.TransformDirection(moveArrow) * speed).z;
        rb.velocity = new Vector3(x, y, z);

        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        //落下判定を行う
        if(other.tag=="LostZone")
        {
            Respawn();
        }

        if(other.tag=="MemoryFragment")
        {
            IsPause = true;
            rb.velocity = Vector3.zero;
            Invoke("Resume", 6.1f);
        }
    }

    private void Resume()
    {
        IsPause = false;
    }
    
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

    //入力の確認。不正なタイミングや不正な同時入力のコマンドを無視します。
    void InputCheck()
    {
        bool[] state =
            {
            IsPause? false:Std.CheckKeyList(right),
            IsPause? false:Std.CheckKeyList(left),
            IsPause? false:Std.CheckKeyList(jump),
            IsPause? false:Std.CheckKeyList(action),
            IsPause? false:Std.CheckKeyList(exit),
            IsPause? false:Std.CheckKeyList(debug),
            IsPause? false:Std.CheckKeyList(d_respawn)
        };

        if (!IsMovable())
        {
            state[(int)Commands.right] = false;
            state[(int)Commands.left] = false;
            state[(int)Commands.jump] = false;
        }
        if (IsGondra || (state[(int)Commands.right] && state[(int)Commands.left]))
        {
            state[(int)Commands.right] = false;
            state[(int)Commands.left] = false;
        }
        if (state[(int)Commands.jump] && !IsCanJamp())
        {
            state[(int)Commands.jump] = false;
        }
        if (!isActionable)
        {
            state[(int)Commands.action] = false;
        }

        beforeInput = state;
        PlayerInput = state;
    }

    //強制的にpositionを+addします
    public void ForceMove(Vector3 add)
    {
        transform.position += add;
    }

    //地面との接触、離脱時に呼ぶ関数。IsGround=hitGになります。
    public void Ground(bool hitG)
    {
        IsGround = hitG;
    }

    //ジャンプできるかどうかを調べます。
    public bool IsCanJamp()
    {
        return !(IsLadder || !IsGround || IsGondra || IsJumping || rb.velocity.y > 3.5f);
    }

    //動くことが可能かどうかを判断します。
    public bool IsMovable()
    {
        return !IsGondra;
    }

    //ゴンドラ乗車時の処理。プレイヤーの動きを静止させる。
    public void GondraEnter()
    {
        IsGondra = true;
        rb.velocity = Vector3.zero;
        rb.useGravity = false;
    }

    //ゴンドラ下車時の処理。alightig分プレイヤーが吹き飛ばされる。
    public void GondraExit(Vector3 Alighting)
    {
        rb.AddForce(Alighting);
        IsGondra = false;
        rb.useGravity = true;
    }
    
    //リスポーン処理。最後に触れたSavePointの情報を用いて再誕します。
    public void Respawn()
    {
        transform.position = SavePoint.transform.position;
        from = SavePoint.p_cp;
        to = SavePoint.t_cp;
        looking = SavePoint.look;
        Look(to);
    }

    //XZ平面におけるカメラのあるべき位置を返す関数。
    public Vector3 GetCameraPosXZ(Vector3 diff)
    {
        return transform.position + ((looking == LR.left) ? transform.right * diff.z : -transform.right * diff.z);
    }

    //アニメーションへ状態を送信するメソッド。
    private void SetAnimStates(bool reset = false)
    {
        anim.SetBool("isRunning", LRmoved && !reset);
        anim.SetBool("isJumping", IsJumping && !IsGround && !reset);
        anim.SetBool("isLadder", IsLadder && !reset);
        anim.SetBool("isUp", rb.velocity.y > 0 && !reset);
        anim.SetBool("Gondra", IsGondra && !reset);
    }

    //跳びます。
    private void Jump()
    {
        rb.AddForce(new Vector3(0, JumpFouce));
        A_source.PlayOneShot(jumpSE);
    }

    //debug code
    private void DebugReset()
    {
        transform.SetPositionAndRotation(def_p, def_q);
        from = def_f_CP;
        to = def_t_CP;
        looking = def_l;
    }
}