using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStates : MonoBehaviour
{
    public GameObject player_obj;
    StopWatch jumpTimer;

    //animation
    public bool isWalking;
    public bool isLadderUp;
    public bool isLadderDown;
    public bool isRideRing;
    public bool isHaveAction;
    public bool isJumpCoolTime;

    public bool IsJumping()
    {
        return Mathf.Abs(player_obj.GetComponent<Rigidbody>().velocity.y) > 0.2;
    }

    public bool IsFly()
    {
        return IsJumping() && player_obj.GetComponent<Rigidbody>().velocity.y > 0;
    }
    
    public bool IsFall()
    {
        return IsJumping() && player_obj.GetComponent<Rigidbody>().velocity.y <= 0;
    }

    //可能な動作の判定
    public bool IsMoveable()
    {
        return !isRideRing;
    }
    
    public bool IsJampable()
    {
        return !(IsFly() || IsFall() || isLadderDown || isLadderUp || isRideRing || IsJumping());
    }

    public bool IsActionable()
    {
        return !(IsFly() || IsFall() || isLadderDown || isLadderUp || isRideRing || IsJumping());
    }

    public void AddAction()
    {
        isHaveAction = true;
    }

    public void RemoveAction()
    {
        isHaveAction = false;
    }

    public void RideRing()
    {
        isRideRing = true;
    }

    public void StartJump()
    {
        jumpTimer.resume();
        jumpTimer.timeOut = EndJump;
        jumpTimer.limit = player_obj.GetComponent<PlayerController>().JumpCoolTime;
    }

    public void EndJump()
    {
        isJumpCoolTime = false;
    }

    private void ResetState()
    {
        isWalking = false;
        isLadderUp = false;
        isLadderDown = false;
        isRideRing = false;
    }
}

