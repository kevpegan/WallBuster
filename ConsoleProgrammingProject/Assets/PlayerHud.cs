using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerHud : MonoBehaviour
{
    [SerializeField]
    Slider cooldownBar;
    [SerializeField]
    float  playerDamage;
    [SerializeField]
    float  playerVelocity;
    [SerializeField]
    GameObject playerCharacter;
    [SerializeField]
    Text playerVelocityText;
    [SerializeField]
    Text playerDamageText;

    float currAttackCD;
    float totalAttackCD;


    // Use this for initialization
    void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
        //update needed values
        playerDamage = (float)System.Math.Round(playerCharacter.GetComponent<PlayerAttack>().playerDamage, 2);
        playerVelocity = (float)System.Math.Round(playerCharacter.GetComponent<PlayerPhysics>().mActualVelocity.magnitude, 2);
        currAttackCD = playerCharacter.GetComponent<PlayerAttack>().currAttackCD;
        totalAttackCD = playerCharacter.GetComponent<PlayerAttack>().attackCD;

        //do stuff with them
        cooldownBar.value = currAttackCD / totalAttackCD;

        playerVelocityText.text = ("Velocity: " + playerVelocity);
        playerDamageText.text = ("Damage: " + playerDamage);
    }
}
