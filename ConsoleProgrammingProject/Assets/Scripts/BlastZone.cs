using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BlastZone : MonoBehaviour 
{
    public Text vicText;
    public bool pendingDestruction;

    float destructionTimer = 2;
    float currDestructionTime = 0;
	// Use this for initialization
	void Start () 
    {
	}
	
	// Update is called once per frame
	void Update () 
    {

        if (pendingDestruction)
        {
            currDestructionTime += Time.deltaTime;
            if (currDestructionTime >= destructionTimer)
            {
                SceneManager.LoadScene(0);
            }
        }
	}
    void OnTriggerExit2D(Collider2D _other)
    {
        if (_other.gameObject.layer == 0)
        {
            print("Player left the station");
            vicText.text = ("Player " + _other.gameObject.GetComponent<PlayerInput>().playerIndex + " loses.");
            pendingDestruction = true;
        }
    }
}
