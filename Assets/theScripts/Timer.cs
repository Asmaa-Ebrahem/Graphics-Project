using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Timer : MonoBehaviour {

    float StartTime = 30;
    float time;
    public Text timeText;
	public static bool Finished;
	public GameObject play;
	int levelN = 2;
    // Use this for initialization
    void Start () {
        time = StartTime;
        Finished = false;
    }
	 void nextlevel()
	{
		levelN++;

		// Application.LoadLevel(levelN);
		SceneManager.LoadScene(levelN);
	}
	
	// Update is called once per frame
	void Update () {
		if (Finished) {
			play.GetComponent<Animator>().Play("WIN00", -1, 0f);

			Invoke("nextlevel", 2f);
		}
        string minutes = ((int)time / 60).ToString();
        string seconds = (time % 60).ToString("f0");

        if (time > 0)
        {
            time -= Time.deltaTime;
            timeText.text = "Time      " + minutes + " : " + seconds;
        }
        if (time <= 0)
        {
            Finished = true;
        }
    }
}
