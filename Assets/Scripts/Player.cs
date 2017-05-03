using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

	public static Player main;
	public List<ILane> lanes = new List<ILane>();

	void Awake()
	{
		if (!main)
			main = this;
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public RectTransform rect;

	public void MoveRight()
	{
		rect.anchoredPosition = new Vector2 (Screen.width / 4, 200);
		Shoot ();
	}

	public void MoveLeft()
	{
		rect.anchoredPosition = new Vector2 (-Screen.width / 4, 200);
		Shoot ();
	}

	public void MoveTo(float x)
	{
		rect.anchoredPosition = new Vector2 (x, 200);
		Shoot ();
	}

	void Shoot()
	{
		// spawn arrow and fire above
		var arrow = PoolingManager.Instance.Grab("arrow");
		if (arrow) arrow.transform.localPosition = transform.localPosition;
	}
}
