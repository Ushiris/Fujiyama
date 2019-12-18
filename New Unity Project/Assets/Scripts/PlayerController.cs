using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private const float FRY_VELOCITY_Y = 0.3f;

    //動作のパラメータ
    public float speed;
    public float JumpFouce;
    public float JumpCoolTime = 0.8f;

    //初期設定が必要な変数
    public LR looking;
    public CheckPoint from;
    public CheckPoint to;
    public SaveManager SavePoint;
    public Animator anim;
    public AudioClip jumpSE;
    public AudioClip walkSE;
    public float footTiming = 0.5f;
    public Image actImage;
    public Sprite actUI;
    public Remind MainObj;

    //入力の設定
    public List<KeyCode> right = new List<KeyCode> { KeyCode.D, KeyCode.RightArrow };
    public List<KeyCode> left = new List<KeyCode> { KeyCode.A, KeyCode.LeftArrow };
    public List<KeyCode> jump = new List<KeyCode> { KeyCode.Space };
    public List<KeyCode> action = new List<KeyCode> { KeyCode.W };
    public List<KeyCode> exit = new List<KeyCode> { KeyCode.Escape };
    readonly List<KeyCode> debug = new List<KeyCode> { KeyCode.B };
    readonly List<KeyCode> d_respawn = new List<KeyCode> { KeyCode.R };
    bool[] PlayerInput = { false, };

    //キャラクターのステート
    public bool isActionable = false;
    public bool IsGondra = false;
    public bool IsGondraGetOut = false;
    public bool IsNotWalkMovie = false;
    bool IsGround = false;
    bool IsLadder = false;
    bool IsJumping = false;
    bool LRmoved = false;
    bool IsRiding = false;
    bool IsPause { get; set; }
    bool AcceptJump { get; set; }
    bool IsMovieMode = false;
    bool isLadderDown = false;
    bool isLeftBind = false;
    bool isRightBind = false;
    float MovieTimer;
    float MovieDuration;
    float JumpTimer = 0.8f;
    float FootAudioTimer = 0;

    //コンポーネント置き場
    Rigidbody rb;
    AudioSource A_source;

    //変数の初期値
    float DefaultSpeed;

    //前フレームの入力状態の保存
    bool[] beforeInput = { false };

    //アクションボタンで続行されるアクション
    public delegate void Action();
    public Action EventEffect;

    //ムービーカットに必要な変数
    Vector3 targetPos;
    Vector3 startPos;

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
        AcceptJump = true;
        actImage.sprite = actUI;
        actImage.color = new Color(1, 1, 1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(isActionable&&!IsPause&&!IsMovieMode)
        {
            actImage.color = Color.white;
        }
        else
        {
            actImage.color = Color.clear;
        }
        //ムービー時に操作を受け付けない
        if(IsMovieMode)
        {
            MovieTimer -= Time.deltaTime;

            if(MovieTimer<=0f)
            {
                IsMovieMode = false;
            }

            if (!IsNotWalkMovie)
            {
                GoWalk((MovieDuration- MovieTimer) / MovieDuration);
            }
            return;
        }

        //ジャンプのクールタイム処理
        if (IsJumping)
        {
            JumpTimer -= Time.deltaTime;
            if (JumpTimer <= 0f && Mathf.Abs(rb.velocity.y) < FRY_VELOCITY_Y)
            {
                IsJumping = false;
                Invoke("JumpOK", FRY_VELOCITY_Y);
                JumpTimer = JumpCoolTime;
            }
        }


        LRmoved = false;

        //プレイヤーからの入力を受け取る
        InputCheck();

        //ゲームの終了
        if (PlayerInput[(int)Commands.exit])
        {
            GameDirector.ShatDown();
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
            AcceptJump = false;
            Jump();
        }

        //他スクリプトで指定されたアクションを発生させる
        if (PlayerInput[(int)Commands.action])
        {
            EventEffect();
        }

        //左右の入力なしなら、左右の速度を減衰させる
        if (!LRmoved)
        {
            float x = Mathf.Abs(rb.velocity.x * 4 / 5f) < 0.05f ? (0.0f) : (rb.velocity.x * 4 / 5f);
            float z = Mathf.Abs(rb.velocity.z * 4 / 5f) < 0.05f ? (0.0f) : (rb.velocity.z * 4 / 5f);
            rb.velocity = new Vector3(x, rb.velocity.y, z);
        }
        else if (!IsJumping && rb.velocity.y < FRY_VELOCITY_Y)
        {
            FootAudioTimer -= Time.deltaTime;
            if (FootAudioTimer < 0)
            {
                FootAudioTimer = footTiming;
                A_source.PlayOneShot(walkSE);
            }
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
    }

    public void GetMemoryFragment(int id)
    {
        Invoke("Remind", 2.0f);
        IsPause = true;
        rb.velocity = Vector3.zero;
        GameDirector.Remind(id);
        Invoke("Resume", 5.5f);
    }

    public void Remind()
    {
        MainObj.remined();
    }

    private void Resume()
    {
        IsPause = false;
    }
    
    //CheckPoint用のLookAt関数。高さを無視します。
    public void Look(CheckPoint to)
    {
        Vector3 lookPos = new Vector3(to.transform.position.x, transform.position.y, to.transform.position.z);
        transform.LookAt(lookPos);
    }

    //はしご用の（略）
    public void Look(Vector3 target)
    {
        Vector3 lookPos = new Vector3(target.x, transform.position.y, target.z);
        transform.LookAt(lookPos);
    }

    //入力の確認。不正なタイミングや不正な同時入力のコマンドを無視します。
    void InputCheck()
    {
        if(IsPause)
        {
            for(int i=0;i< PlayerInput.Length;i++)
            {
                PlayerInput[i] = false;
            }
            return;
        }


        bool[] state =
            {
            Std.CheckKeyList(right)||Input.GetAxis("PS4LR")>0.7f||Input.GetAxis("PS4LR_s")>0.7f,
            Std.CheckKeyList(left)||Input.GetAxis("PS4LR")<-0.7f||Input.GetAxis("PS4LR_s")<-0.7f,
            Std.CheckKeyList(jump),
            Std.CheckKeyList(action),
            Std.CheckKeyList(exit),
            Std.CheckKeyList(debug),
            Std.CheckKeyList(d_respawn)
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
        if (state[(int)Commands.jump] && !IsCanJamp() && !AcceptJump)
        {
            state[(int)Commands.jump] = false;
        }
        if (!isActionable)
        {
            state[(int)Commands.action] = false;
        }
        if(isRightBind)
        {
            state[(int)Commands.right] = false;
        }
        if(isLeftBind)
        {
            state[(int)Commands.left] = false;
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
        IsGround = Mathf.Abs(rb.velocity.y) < FRY_VELOCITY_Y && hitG;
    }

    //ジャンプできるかどうかを調べます。
    public bool IsCanJamp()
    {
        return !(IsLadder || !IsGround || IsGondra || IsJumping || !AcceptJump || Mathf.Abs(rb.velocity.y) > FRY_VELOCITY_Y);
    }

    //動くことが可能かどうかを判断します。
    public bool IsMovable()
    {
        return !IsGondra && !IsGondraGetOut;
    }

    //ゴンドラ乗車時の処理。プレイヤーの動きを静止させる。
    public void GondraEnter()
    {
        rb.velocity = Vector3.zero;
        rb.useGravity = false;
        IsMovieMode = true;
        MovieTimer = 100;
        IsNotWalkMovie = true;
    }

    //ゴンドラ下車時の処理。要するに原状復帰。
    public void GondraExit()
    {
        IsGondraGetOut = false;
        rb.useGravity = true;
        MovieTimer = 0;
        IsNotWalkMovie = true;
    }

    public void LadderEnter(Vector3 to,float duration)
    {
        IsLadder = transform.position.y < to.y;
        IsPause = true;
        rb.useGravity = false;
        SetAnimStates(true);
        string setFlagName = (transform.position.y > to.y) ? "isLadderDown" : "isLadder";
        anim.SetBool(setFlagName, true);
        rb.velocity = (to - transform.position)/duration;
        Invoke("LadderExit", duration);
        Invoke("Resume", duration);
    }

    public void LadderExit()
    {
        Debug.Log(transform.forward.x * 1000);
        rb.AddForce(new Vector3(0, 200, 0));
        Invoke("push", FRY_VELOCITY_Y);
        LadderEnd();
    }

    public void push()
    {
        rb.AddForce(new Vector3(transform.forward.x * 300, 0, transform.forward.z * 300));
    }

    private void LadderEnd()
    {
        IsLadder = false;
        rb.useGravity = true;
        SetAnimStates(true);
    }
    
    //リスポーン処理。最後に触れたSavePointの情報を用いて再誕します。
    public void Respawn()
    {
        transform.position = SavePoint.transform.position;
        from = SavePoint.p_cp;
        to = SavePoint.t_cp;
        looking = SavePoint.look;
        rb.velocity = Vector3.zero;
        Look(to);
    }

    //XZ平面におけるカメラのあるべき位置を返す関数。
    public Vector3 GetCameraPosXZ(Vector3 diff)
    {
        return transform.position + ((looking == LR.left) ? transform.right * diff.z : -transform.right * diff.z);
    }

    //アニメーションへ状態を送信するメソッド。
    public void SetAnimStates(bool reset = false)
    {
        anim.SetBool("isRunning", LRmoved && !IsLadder && !IsJumping&& Mathf.Abs(rb.velocity.y) < FRY_VELOCITY_Y && !reset);
        anim.SetBool("isJumping", IsJumping && !IsGround && !IsLadder && !reset);
        anim.SetBool("isUp", rb.velocity.y > FRY_VELOCITY_Y && IsJumping && !reset);
        anim.SetBool("isLadder", IsLadder && !reset);
        anim.SetBool("Gondra", IsGondra && !reset);
        anim.SetBool("IsFall", rb.velocity.y < -FRY_VELOCITY_Y && !reset);
        anim.SetBool("isLadderDown", isLadderDown && !reset);
    }

    //跳びます。
    private void Jump()
    {
        rb.AddForce(new Vector3(0, JumpFouce));
        A_source.PlayOneShot(jumpSE);
    }

    private void JumpOK()
    {
        AcceptJump = true;
    }

    public void SetGoWalkMode(Vector3 to,float duration)
    {
        Look(to);
        startPos = transform.position;
        targetPos = to;
        IsMovieMode = true;
        MovieTimer = duration;
        MovieDuration = duration;
    }

    public void GoWalk(float progles)
    {
        anim.SetBool("isRunning", true);
        transform.position = Vector3.Lerp(startPos, targetPos, progles);
    }

    public void Bind()
    {
        if(looking == LR.right)
        {
            isRightBind = true;
        }
        else
        {
            isLeftBind = true;
        }
    }

    public void React()
    {
        isRightBind = false;
        isLeftBind = false;
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