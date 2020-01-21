using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

public class InputManager : MonoBehaviour
{
    //キーコンフィグのデフォルト
    readonly List<KeyCode> right = new List<KeyCode> { KeyCode.D, KeyCode.RightArrow };
    readonly List<KeyCode> left = new List<KeyCode> { KeyCode.A, KeyCode.LeftArrow };
    readonly List<KeyCode> jump = new List<KeyCode> { KeyCode.Space,KeyCode.Joystick1Button0 };
    readonly List<KeyCode> action = new List<KeyCode> { KeyCode.W, KeyCode.Joystick1Button1 };
    readonly List<KeyCode> exit = new List<KeyCode> { KeyCode.Escape };

    //デバッグ用のコマンド
    readonly List<KeyCode> debug = new List<KeyCode> { KeyCode.B };
    readonly List<KeyCode> d_respawn = new List<KeyCode> { KeyCode.R };

    //入力の取得
    public bool[] GetPlayerInputs()
    {
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

        return state;
    }
}
