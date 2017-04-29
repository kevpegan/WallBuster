using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerInput : MonoBehaviour
{
    //references
    PlayerMovement mPlayerMovement;
    PlayerAttack   mPlayerAttack;

    //GamePadInfo
    public PlayerIndex playerIndex;
    GamePadState inputState;
    GamePadState prevInputState;
    public Vector2 mRawMove;
    public Vector2 mRawAim;

    public float mTriggerDeadZone = .2f;
    public float mAnalogDeadZone  = 0f;

    public bool mAttackedAlready;

    //Action Bindings
    char mJumpButton = 'A';
    char mAttackButton;
    char mBlockButton;
    char mTechButton;

    //Buttons Pressed
    

    // Use this for initialization
    void Start()
    {
        mPlayerMovement = GetComponent<PlayerMovement>();
        mPlayerAttack   = GetComponent<PlayerAttack>();
    }

    // Update is called once per frame
    void Update()
    {
        GetRawInput();
        CheckActionButtons();
        GetStickValues();
    }

    private void GetStickValues()
    {
        mRawMove = GetMoveStick();
        mRawAim  = GetAimStick();
    }

    void CheckJump()
    {
        if (inputState.Buttons.A == ButtonState.Pressed && prevInputState.Buttons.A != ButtonState.Pressed)
        {
            mPlayerMovement.Jump();
        }
    }

    void CheckAttack()
    {
        if (inputState.Triggers.Right >= mTriggerDeadZone && !mAttackedAlready && !mPlayerAttack.attackOnCD)
        {
            mPlayerAttack.TryAttack();
            mAttackedAlready = true;
        }
    }

    Vector2 GetMoveStick()
    {
        float moveDirectionX = 0;
        float moveDirectionY = 0;

        Vector2 moveDirection = Vector2.zero;

        //get movement in the X
        if (Mathf.Abs(inputState.ThumbSticks.Left.X) >= mAnalogDeadZone)
        {
            moveDirectionX = inputState.ThumbSticks.Left.X;
        }

        //get movement in the Y
        if (Mathf.Abs(inputState.ThumbSticks.Left.Y) >= mAnalogDeadZone)
        {
            moveDirectionY = inputState.ThumbSticks.Left.Y;
        }

        moveDirection = new Vector2(moveDirectionX, moveDirectionY);

        return moveDirection;
    }

    Vector2 GetAimStick()
    {
        float aimDirectionX = 0;
        float aimDirectionY = 0;

        Vector2 aimDirection = Vector2.zero;

        //get aim in the X
        if (Mathf.Abs(inputState.ThumbSticks.Right.X) >= mAnalogDeadZone)
        {
            aimDirectionX = inputState.ThumbSticks.Right.X;
        }
        //get aim in the Y
        if (Mathf.Abs(inputState.ThumbSticks.Right.Y) >= mAnalogDeadZone)
        {
            aimDirectionY = inputState.ThumbSticks.Right.Y;
        }

        aimDirection = new Vector2(aimDirectionX, aimDirectionY);

        return aimDirection;
    }

    void CheckActionButtons()
    {
        CheckJump();
        CheckAttack();
        if (mAttackedAlready)
        {
            if(inputState.Triggers.Right <= mTriggerDeadZone)
            {
                mAttackedAlready = false;
            }
        }
    }

    void GetRawInput()
    {
        prevInputState = inputState;
        inputState = GamePad.GetState(playerIndex);
    }
}
