using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour 
{
    [SerializeField]
    Button play;
	// Use this for initialization
	void Start () 
    {
        play.Select();
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}

    public void OnPlayButton()
    {
        SceneManager.LoadScene(1);
    }

    public void OnExitButton()
    {
        Application.Quit();
    }
}
