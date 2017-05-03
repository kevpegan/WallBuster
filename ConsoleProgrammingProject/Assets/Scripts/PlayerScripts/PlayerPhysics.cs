using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysics : MonoBehaviour
{
    //Relevant Layers
    public int ground = 9;

    //All Player Velocity Vectors
    public Vector2 mMovementVelocityVector,
                   mKnockbackVelocityVector,
                   mKnockbackInfluenceVelocityVector,
                   mActualVelocity;

    //All Player Acceleration Vectors
    public Vector2 mMovementAccelerationVector,
                   mKnockbackAccelerationVector,
                   mKnockbackInfluenceAccelerationVector,
                   mAccelerationDueToGravity,
                   mActualAcceleration;

    //All Scalar Values
    public float mKnockbackDecay,
          mGroundFriction,
          mDamageScalar;

    //All Constant Values
    public const float GRAVITY_VALUE = 20f;

    //Weight Values for combination of vectors
    float mMovementWeight           = 1.0f;
    float mKnockbackWeight          = 1.0f;
    float mKnockbackInfluenceWeight = 1.0f;

    //PlayerPosition
    public Vector2 mPlayerPosition;
    public bool    mGrounded;
    public bool    mIsTouchingPlayer;

    public bool mWasReflectedThisFrame;

    //Player Statistics
    float mMass; //kg

    PlayerAttack pa;


	// Use this for initialization
	void Start ()
    {
        mAccelerationDueToGravity = new Vector2(0, -GRAVITY_VALUE);
        mPlayerPosition = GetComponent<Transform>().position;
        pa = GetComponent<PlayerAttack>();
	}

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        CheckGround();            
        CheckWall();
        Decelerate();
        if (mWasReflectedThisFrame)
        {
            mWasReflectedThisFrame = false;
        }
    }

    public void ApplyImpulse(Vector2 par_ImpulseAmount)
    {
        Debug.Log("Impulse: " + par_ImpulseAmount);
        mActualVelocity += par_ImpulseAmount;
    }

    public void ApplyForce(Vector2 par_force)
    {
        mActualAcceleration += par_force;
    }

    void CombineAccelerations()
    {
        Vector2 addedAccelerations = Vector2.zero;

        addedAccelerations = addedAccelerations + (mMovementAccelerationVector           * mMovementWeight         +
                                                   mKnockbackAccelerationVector          * mKnockbackWeight        +
                                                   mKnockbackInfluenceAccelerationVector * mKnockbackInfluenceWeight);
        mActualAcceleration += addedAccelerations;

        if (mGrounded == false)
        {
            mActualVelocity += mAccelerationDueToGravity * Time.deltaTime;
        }
    }

    void Decelerate()
    {
        if (mGrounded)
        {
            if (Mathf.Abs(mActualVelocity.x) > 0)
            {
                mActualVelocity.x *= mGroundFriction;
            }
        }
    }

    void ApplyAccelerations()
    {
        mMovementVelocityVector           += mMovementAccelerationVector;
        mKnockbackVelocityVector          += mKnockbackAccelerationVector;
        mKnockbackInfluenceVelocityVector += mKnockbackInfluenceAccelerationVector;

        mMovementAccelerationVector = Vector3.zero;
        mKnockbackAccelerationVector = Vector3.zero;
        mKnockbackInfluenceAccelerationVector = Vector3.zero;
    }

    void CombineVelocities()
    {
        Vector2 addedVelocities = Vector2.zero;

        addedVelocities = addedVelocities + (mMovementVelocityVector           * mMovementWeight         + 
                                             mKnockbackVelocityVector          * mKnockbackWeight        + 
                                             mKnockbackInfluenceVelocityVector * mKnockbackInfluenceWeight);

        //mActualVelocity = addedVelocities;
        mActualVelocity += mActualAcceleration;

        mMovementVelocityVector = Vector2.zero;
        mKnockbackVelocityVector = Vector2.zero;
        mMovementVelocityVector = Vector2.zero;
        mActualAcceleration = Vector2.zero;
    }

    void  MovePlayer()
    {
        mPlayerPosition += mActualVelocity * Time.deltaTime;
        GetComponent<Transform>().position = mPlayerPosition;

        CombineAccelerations();
        ApplyAccelerations();
        CombineVelocities();       
        
    }

    void CheckGround()
    {
        BoxCollider2D bc = GetComponent<BoxCollider2D>();
        if (mPlayerPosition.y <= -2.9)
        {
            mPlayerPosition.y = -2.9f;

            GetComponent<Transform>().position = mPlayerPosition;
            

            mGrounded = true; 
            if (mActualVelocity.y < 0)
            {
                mActualVelocity.y = 0;
            }
            if (mActualAcceleration.y < 0)
            {
                mActualAcceleration.y = 0;                
            }
        }
        else if (mGrounded == true && !bc.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            mGrounded = false;
        }
    }

    void CheckWall()
    {
        BoxCollider2D bc = GetComponent<BoxCollider2D>();

        if ( bc.IsTouchingLayers( LayerMask.GetMask("Wall") ) )
        {
            //ApplyImpulse(-mActualVelocity * 2);
        }
    }

    void OnCollisionEnter2D(Collision2D _other)
    {
        ////wall
        //if (_other.gameObject.layer == 9)
        //{
        //    //ApplyImpulse(-mActualVelocity * 2);
        //    mActualVelocity.x *= -1;
        //}

        ////ceiling
        //if (_other.gameObject.layer == 10)
        //{
        //    //ApplyImpulse(-mActualVelocity * 2);
        //    mPlayerPosition.y = 3;
        //    mActualVelocity.y *= -1;
        //}

        if (_other.gameObject.layer == 0)
        {
            Vector2 op = _other.gameObject.GetComponent<PlayerPhysics>().mPlayerPosition;
            Vector2 pushDir = op - mPlayerPosition;
            pushDir.Normalize();

            mIsTouchingPlayer = true;
            pa.otherPlayer = _other.gameObject;
            ApplyImpulse(pushDir * -mActualVelocity.magnitude * 1.2f);
            _other.gameObject.GetComponent<PlayerPhysics>().ApplyImpulse(pushDir * mActualVelocity.magnitude * 1.2f);
        }
    }

    void OnTriggerExit2D(Collider2D _other)
    {
        if (_other.gameObject.layer == 0)
        {
            mIsTouchingPlayer = false;
            pa.otherPlayer = null;
        }
    }

    void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.gameObject.layer == 0 && _other.gameObject != this)
        {
            print(_other.gameObject);
            mIsTouchingPlayer = true;
            pa.otherPlayer = _other.gameObject;
        }
    }
}
