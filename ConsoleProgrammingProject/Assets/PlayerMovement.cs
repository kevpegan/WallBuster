using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerPhysics))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    //references
    PlayerPhysics mPlayerPhysics;
    PlayerInput   mPlayerInput;
    PlayerAttack  mPlayerAttack;

    //Values
    public float mJumpHeight; //(meters)
    public float mMovementAcceleration; //(meters / second)

    //Pre-Calculated Vectors
    public Vector2 mJumpVector;
    Vector2 mMoveForwardVector;
    Vector2 mMoveBackwardVector;
    
    

    // Use this for initialization
    void Start ()
    {
        mPlayerPhysics = GetComponent<PlayerPhysics>();
        mPlayerInput   = GetComponent<PlayerInput>();
        mPlayerAttack  = GetComponent<PlayerAttack>();

        //pre-calculate jump velocity
        float g = PlayerPhysics.GRAVITY_VALUE;
        float h = mJumpHeight;
        float jumpY = Mathf.Sqrt(2 * g * h);
        mJumpVector = new Vector2(0, jumpY);
        Debug.Log(jumpY);
	}
	
	// Update is called once per frame
	void Update ()
    {
        CheckMove();
	}

    public void CheckMove()
    {
        if (Mathf.Abs(mPlayerInput.mRawMove.x) > mPlayerInput.mTriggerDeadZone)
        {
            if (!mPlayerAttack.isInHitstun)
            {
                if (mPlayerPhysics.mGrounded)
                {
                    mPlayerPhysics.ApplyForce(new Vector2(mMovementAcceleration * mPlayerInput.mRawMove.x, 0));
                }
                else
                {
                    mPlayerPhysics.ApplyForce( new Vector2(mMovementAcceleration * .1f * mPlayerInput.mRawMove.x, 0) );
                }
            }
        }
    }
    public void Jump()
    {
        if (mPlayerPhysics.mGrounded)
        {
            mPlayerPhysics.ApplyImpulse(mJumpVector);
            mPlayerPhysics.mGrounded = false;
        }
    }
}
