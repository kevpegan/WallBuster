using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleWall : MonoBehaviour
{

    public float velocityLimit;
    public float wallOrCeiling;
    SpriteRenderer spr;

	// Use this for initialization
	void Start () 
    {
        spr = GetComponent<SpriteRenderer>();
        spr.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter2D(Collision2D _other)
    {
        print("player hit me");
        if (_other.gameObject.layer == 0)
        {
            float magnitude = _other.gameObject.GetComponent<PlayerPhysics>().mActualVelocity.magnitude;
            print(magnitude);
            if (magnitude >= velocityLimit)
            {
                ReflectPlayer(_other.gameObject);
                Destroy(this.gameObject);
            }
            else
                ReflectPlayer(_other.gameObject);
        }
    }

    void ReflectPlayer(GameObject other)
    {
       bool check = other.gameObject.GetComponent<PlayerPhysics>().mWasReflectedThisFrame;
       print("Reflecting Player");
       if (!check)
       {
           if (wallOrCeiling == 0)
           {
               other.gameObject.GetComponent<PlayerPhysics>().mActualVelocity.x *= -1;
           }
           if (wallOrCeiling == 1)
           {
               other.gameObject.GetComponent<PlayerPhysics>().mActualVelocity.y *= -1;
           }

           other.gameObject.GetComponent<PlayerPhysics>().mWasReflectedThisFrame = true;
       }
    }
}
