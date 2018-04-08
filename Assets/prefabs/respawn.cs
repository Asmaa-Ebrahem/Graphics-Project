using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class respawn : MonoBehaviour {
    public static int levelN = 0;
    private Vector3 startPos;
    private Quaternion startRot;

	// Use this for initialization
	void Start () {
        startPos = transform.position;
        startRot = transform.rotation;
	}
    
    void nextlevel()
    {
        levelN++;

        if (levelN > 1)
        {
            levelN = 0;
        }
       // Application.LoadLevel(levelN);
        SceneManager.LoadScene(levelN);
    }

    // check collision with trigger//
    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "death")
        {
            transform.position = startPos;
            transform.rotation = startRot;
            GetComponent<Animator>().Play("LOSE00", -1, 0f);
            GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
            GetComponent<Rigidbody>().angularVelocity = new Vector3(0f, 0f, 0f);
        }
        else if (col.tag == "checkpoint")
        {
            startPos = col.transform.position;
            startRot = col.transform.rotation;
            Destroy(col.gameObject);
        }
        else if (col.tag == "goal")
        {
            Destroy(col.gameObject);
            GetComponent<Animator>().Play("WIN00", -1, 0f);
            Invoke("nextlevel", 2f);
        }
        }
    void Update()
    {
        
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
