using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour 
{
    public Button playButton;
    public Button helpButton;
    public Button quitButton;

	// Use this for initialization
	void Start () 
    {
        playButton.Select();
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}
    
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }
}
