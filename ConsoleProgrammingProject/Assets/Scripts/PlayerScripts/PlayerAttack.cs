using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour 
{
    Vector2 mPos;
    Rigidbody2D rb;
    public BoxCollider2D bc;

    PlayerPhysics pp;
    PlayerInput   pi;

    Vector2 attack;
    public float attackMag;

    public bool isInHitstun;
    public float playerDamage;
    public float attackVal;

    public float attackCD;
    public float currAttackCD;
    public bool  attackOnCD;

    public GameObject otherPlayer;

    public float mAttackLength;

    //attack sphere info
    public float mAttackOrbitVal;
    public float mCurrAngleDegrees;
    public float mDesiredAngleDegrees;
    public Vector2 lerpVec0, lerpVec1;

	// Use this for initialization
	void Start ()
    {
        mPos = GetComponent<Transform>().position;
        rb = GetComponent<Rigidbody2D>();
        pp = GetComponent<PlayerPhysics>();
        pi = GetComponent<PlayerInput>();
        attack = new Vector2(1, 1);
        playerDamage = 1;
        attackVal = .3f;
	}
	
	// Update is called once per frame
	void Update ()
    {
        CooldownAttack();
        PositionAttackBox();
	}

    void PositionAttackBox()
    {
        float x = pi.mRawAim.x;
        float y = pi.mRawAim.y;
        Vector2 center = pp.mPlayerPosition;
        float radius = mAttackOrbitVal;
        float angle = 0;

        if (x != 0)
        {
            float radian = Mathf.Atan(y / x);
            angle = radian * (180 / Mathf.PI);
            if(x < 0)
            {
                angle += 180;
            }
            else if (x > 0 && y < 0)
            {
                angle += 360;
            }
        }
        else if (x == 0 && y > 0)
        {
            angle = 90;
        }
        else if (x == 0 && y < 0)
        {
            angle = 270;
        }

        float newX = center.x + ( radius * Mathf.Cos(angle * Mathf.PI / 180) );
        float newY = center.y + ( radius  * Mathf.Sin(angle * Mathf.PI / 180) );
        Vector2 newPos = new Vector2(newX, newY);
        bc.transform.position = newPos;
    }

    void CooldownAttack()
    {
        if (attackOnCD)
        {
            currAttackCD += Time.deltaTime;
            if (currAttackCD >= attackCD)
            {
                attackOnCD = false;
            }
        }
    }

    public void ApplyDamage(float dmg)
    {
        playerDamage += dmg;
    }

    public void TryAttack()
    {
        attack.x = pi.mRawAim.x;
        attack.y = pi.mRawAim.y;
        



        if (pp.mIsTouchingPlayer)
        {
            if (otherPlayer != null)
            {

                attack *= attackMag;
                otherPlayer.GetComponent<PlayerPhysics>().ApplyImpulse(attack * otherPlayer.GetComponent<PlayerAttack>().playerDamage);
                otherPlayer.GetComponent<PlayerAttack>().ApplyDamage(attackVal);
            }
        }
    }
}
