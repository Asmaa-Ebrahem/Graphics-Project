using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class respawn : MonoBehaviour {
    int levelN = 1;
    private Vector3 startPos;
    private Quaternion startRot;
	public AudioClip impact;
	AudioSource audioSource;
    private GameObject triggerNpc;
    private bool triggering;
   // public GameObject TheNPC;
	public AudioClip impact1;
	AudioSource audioSource1;
	public AudioClip impact2;
	AudioSource audioSource2;
	public AudioClip impactsabar;
	AudioSource audioSourcesabar;

    public Text winningText;	


	// Use this for initialization
	void Start () {
        startPos = transform.position;
        startRot = transform.rotation;
		audioSource = GetComponent<AudioSource>();
		audioSource1 = GetComponent<AudioSource>();
		audioSource2 = GetComponent<AudioSource>();
		audioSourcesabar=GetComponent<AudioSource>();
//		winningText.text = "";


	}
    
	 void nextlevel()
    {
        levelN++;

		if (levelN > 3)
        {
            levelN = 1;
        }
       // Application.LoadLevel(levelN);
        SceneManager.LoadScene(levelN);
    }

    // check collision with trigger//
    void OnTriggerEnter(Collider col)
    {
		if (col.tag == "death"||col.tag=="car")
        {
            transform.position = startPos;
            transform.rotation = startRot;
            GetComponent<Animator>().Play("LOSE00", -1, 0f);
            GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
            GetComponent<Rigidbody>().angularVelocity = new Vector3(0f, 0f, 0f);
        }
       
		else if (col.tag == "healthy")
        {
            Destroy(col.gameObject);
			audioSource.PlayOneShot(impact, 0.7F);
			ScoreScript.ScoreValue += 5;
            //GetComponent<Animator>().Play("WIN00", -1, 0f);
            //Invoke("nextlevel", 2f);
        }
		else if (col.tag == "unhealthy")
		{
			Destroy(col.gameObject);
			audioSource.PlayOneShot(impact, 0.7F);
			ScoreScript.ScoreValue-=5;
        }
		else if (col.tag == "goal")
		{
			Destroy(col.gameObject);
			//audioSource.PlayOneShot(impact, 0.7F);
			GetComponent<Animator>().Play("WIN00", -1, 0f);
			audioSource1.PlayOneShot(impact1, 0.7F);
			Invoke("nextlevel", 2f);
		    winningText.text = "YOU WIN!";
		}
        if (col.tag == "enemy")
        {
            triggering = true;
            triggerNpc = col.gameObject;
			ScoreScript.ScoreValue-=5;
			audioSource2.PlayOneShot(impact2, 0.7F);
            Destroy(col.gameObject, 3f);

        }
		else if (col.tag == "Cactus")
		{
			//Destroy(col.gameObject);
			audioSourcesabar.PlayOneShot(impactsabar, 0.7F);
			ScoreScript.ScoreValue-=5;
		}

    }
    private void OnTriggerExit(Collider col)
    {
        if (col.tag == "enemy")
        {
            triggering = false;
            triggerNpc = col.gameObject;

        }
    }
    void Update()
    {
		/*if (Timer.Finished==true) {
			GetComponent<Animator>().Play("WIN00", -1, 0f);
			Invoke("nextlevel", 2f);
		}*/
		if (ScoreScript.ScoreValue < 5) {
			transform.position = startPos;
			transform.rotation = startRot;
			GetComponent<Animator>().Play("LOSE00", -1, 0f);
			GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
			GetComponent<Rigidbody>().angularVelocity = new Vector3(0f, 0f, 0f);
		}
        if (triggering)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                // col.g.GetComponent<Animation>().Play("Death");
                triggering = true;
                //Destroy(col.gameObject);
                //Destroy(enemy.gameObject, 3f);
                //StartCoroutine(ActivationRoutine());
            }

        }
        if (Input.GetKeyDown("x"))
        {
            GetComponent<Animator>().Play("back_kick", -1, 0f);
        }
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            GetComponent<Animator>().Play("Attack1", -1, 0f);    
        }
        if (Input.GetKeyDown("g"))
        {
            GetComponent<Animator>().Play("walk", -1, 0f);
        }

    }
  

}
