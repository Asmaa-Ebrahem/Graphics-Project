﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pathfollower : MonoBehaviour {

	public Transform [] path;//array of transformations.
	public float speed = 5.0f;
	public float reachDist = 1.0f;
	public int currentPoint = 0;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		Vector3 dir =  path [currentPoint].position - transform.position ;
		transform.position += dir * Time.deltaTime * speed;
		if (dir.magnitude <= reachDist) {
			currentPoint++;
		}
		if (currentPoint >= path.Length) {
			currentPoint = 0;
		}


		
	}
	void OnDrowGizmos(){
		if (path.Length > 0) {
			for (int i = 0; i < path.Length; i++) {
				if (path [i] != null) {
					Gizmos.DrawSphere (path [i].position, reachDist);

			
				}
		
		
			}

		}

	}

}
