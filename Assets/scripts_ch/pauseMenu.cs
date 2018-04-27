using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pauseMenu : MonoBehaviour {



	public static bool IsPaused;
	public GameObject PauseMenu; 


	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.Escape)) 
		{
			if (IsPaused)
				Resume();
			else
				pause();
		}
	}

	public void Resume()
	{
		PauseMenu.SetActive (false);
		Time.timeScale = 1f;
		IsPaused = false;
	}

	void pause()
	{
		PauseMenu.SetActive(true);
		Time.timeScale = 0f;
		IsPaused = true;
	}

	public void LoadMenu()
	{
		SceneManager.LoadScene ("MainMenu");
		Debug.Log ("loadnig Menu..");
	}

	public void Exit()
	{
		Debug.Log ("Exit");
		Application.Quit ();
	}
}
