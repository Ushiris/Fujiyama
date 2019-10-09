using System.Collections;
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
    END,
}

public class PlayerController : MonoBehaviour
{
    //定数
    private const KeyCode ExitKey = KeyCode.Escape;
    private const KeyCode DebugKey = KeyCode.B;

    //動作のパラメータ
    public float speed;
    public float JumpFouce;

    //初期設定
    public LR looking;
    public CheckPoint from;
    public CheckPoint to;
    public SaveManager SavePoint;

    //入力の設定
    public List<KeyCode> right  = new List<KeyCode> { KeyCode.D,    KeyCode.RightArrow  };
    public List<KeyCode> left   = new List<KeyCode> { KeyCode.A,    KeyCode.LeftArrow   };
    public List<KeyCode> jump   = new List<KeyCode> { KeyCode.Space };
    public List<KeyCode> action = new List<KeyCode> { KeyCode.W     };
    readonly List<KeyCode> exit   = new List<KeyCode> { KeyCode.Escape };
    readonly List<KeyCode> debug  = new List<KeyCode> { KeyCode.B };

    //キャラクターのステート
    bool IsGondra    = false;
    bool IsGround    = false;
    bool IsLadder    = false;
    bool[] PlayerInput = { false, };

    //コンポーネント置き場
    Rigidbody rb;

    //変数の初期値
    float DefaultSpeed;

    //前フレームの状態の保存
    bool[] beforeInput = { false };

    //debug
    Vector3 def_p;
    Quaternion def_q;
    CheckPoint def_f_CP;
    CheckPoint def_t_CP;
    LR def_l;

    // Start is called before the first frame update
    void Start()
    {
        DefaultSpeed = speed;
        rb = gameObject.GetComponent<Rigidbody>();
        Look(to);
        def_p = transform.position;
        def_q = transform.rotation;
        def_f_CP = from;
        def_t_CP = to;
        def_l = looking;
        InputCheck();
    }

    // Update is called once per frame
    void Update()
    {
        bool LRmoved = false;
        InputCheck();

        //ゲームの終了
        if(PlayerInput[(int)Commands.exit])
        {
            Quit();
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
            rb.AddForce(new Vector3(0, JumpFouce));
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

        //debug code
        if (Input.GetKeyDown(DebugKey))
        {
            Reset();
        }
    }

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
        Vector3 moveArrow;

        if (!IsLadder)
        {
            moveArrow = Vector3.forward;
        }
        else
        {
            moveArrow = (InputLR == LR.right) ? Vector3.up : Vector3.down;
        }

        //動作する速度の設定
        x = (transform.TransformDirection(moveArrow) * speed).x;
        y = IsLadder ? (transform.TransformDirection(moveArrow)).y : rb.velocity.y;
        z = (transform.TransformDirection(moveArrow) * speed).z;
        rb.velocity = new Vector3(x, y, z);

        return true;
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

    //入力の確認。不正なタイミングや不正な同時入力のコマンドを無視します。
    void InputCheck()
    {
        if (!Input.anyKey)
        {
            bool[] state =
                {
            Std.CheckKeyList(right),
            Std.CheckKeyList(left),
            Std.CheckKeyList(jump),
            Std.CheckKeyList(action),
            Std.CheckKeyList(exit),
            Std.CheckKeyList(debug)
        };

            if (IsGondra || (state[(int)Commands.right] && state[(int)Commands.left]))
            {
                state[(int)Commands.right] = false;
                state[(int)Commands.left] = false;
            }
            if (state[(int)Commands.jump] && !IsCanJamp())
            {
                state[(int)Commands.jump] = false;
            }

            beforeInput = state;
            PlayerInput = state;
        }
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

    //ジャンプできるかどうかの判定
    public bool IsCanJamp()
    {
        return !(IsLadder || !IsGround || IsGondra || rb.velocity.y > 3.5f);
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
        from = SavePoint.p_cp;
        to = SavePoint.t_cp;
        looking = SavePoint.look;
        Look(to);
    }

    //ゲームをシャットダウンします。
    void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
    UnityEngine.Application.Quit();
#endif
    }

    //debug code
    private void Reset()
    {
        transform.SetPositionAndRotation(def_p, def_q);
        from = def_f_CP;
        to = def_t_CP;
        looking = def_l;
    }
}