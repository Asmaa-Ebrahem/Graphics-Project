using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour {
	public GameObject[] Foods;
	// Use this for initialization
	void Start () {
		int RandomNumber = Random.Range (0, Foods.Length);
		Foods [RandomNumber].SetActive (true);
		for (int i = 0; i < Foods.Length; i++) 
		{
			if (i != RandomNumber) {
				Foods [i].SetActive (false);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
