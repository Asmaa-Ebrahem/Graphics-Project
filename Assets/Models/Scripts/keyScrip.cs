using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keyScrip : MonoBehaviour {

	public Vector3[] Positions;
	// Use this for initialization
	void Start () {
		int RandomKey = Random.Range (0,Positions.Length);
		transform.position = Positions [RandomKey]+transform.position;
	}

	
	// Update is called once per frame
	void Update () {
		
	}
}
