﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTouchInput : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		#if UNITY_ANDROID
		if (Input.GetTouch (0).phase == TouchPhase.Began) {
			if (Input.GetTouch (0).position.x > Screen.width / 2) {
				// move and shoot right
				Player.main.MoveRight();
			} else if (Input.GetTouch (0).position.x < Screen.width / 2) {
				// move and shoot left
				Player.main.MoveLeft();
			}
		}
		#endif
	}

	void OnMouseDown()
	{
		#if UNITY_EDITOR
		if (Input.mousePosition.x > Screen.width / 2) {
			// move and shoot right
			Player.main.MoveRight();
		} else if (Input.mousePosition.x < Screen.width / 2) {
			// move and shoot left
			Player.main.MoveLeft();
		}

		#endif
	}
}