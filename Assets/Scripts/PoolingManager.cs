using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour {

	public static PoolingManager Instance;

	void Awake()
	{
		if (!Instance)
			Instance = this;
	}

	public List<GameObject> arrowPool = new List<GameObject>();
	public List<GameObject> enemyPool = new List<GameObject>();

	// Use this for initialization
	void Start () {
		// make a few arrows and store inside pool
		// pool for arrows and enemies each

		for (int x = 0; x < 20; x++) {
			MakeArrow ();
			MakeEnemy ();
		}
	}

	public GameObject prefabArrow;
	public GameObject prefabEnemy;

	void MakeArrow()
	{
		var arrow = Instantiate (prefabArrow, GameObject.Find("Arrows").transform);
		arrow.transform.localScale = Vector3.one - (Vector3.up * 2);
		arrow.transform.localPosition = Vector3.zero;
		arrow.tag = "arrow";
		arrowPool.Add (arrow);
		arrow.SetActive (false);
	}

	void MakeEnemy()
	{
		var enemy = Instantiate (prefabEnemy, GameObject.Find("Enemies").transform);
		enemy.transform.localScale = Vector3.one * 0.2f;
		enemy.transform.localPosition = Vector3.zero;
		enemy.tag = "enemy";
		enemyPool.Add (enemy);
		enemy.SetActive (false);
		enemy.transform.SetAsFirstSibling ();
	}

	public void Put(GameObject go, string name)
	{
		switch (name) {
		case "arrow":
			arrowPool.Add (go);
			go.SetActive (false);
			break;
		case "enemy":
			enemyPool.Add (go);
			go.SetActive (false);
			break;
		}
	}

	public GameObject Grab(string name)
	{
		GameObject go = null;
		switch (name) {
		case "arrow":
			if (arrowPool.Count <= 0)
				break;
			go = arrowPool [0];
			go.SetActive (true);
			arrowPool.RemoveAt (0);
			break;
		case "enemy":
			if (enemyPool.Count <= 0)
				break;
			go = enemyPool [0];
			go.SetActive (true);
			enemyPool.RemoveAt (0);
			break;
		}

		return go;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
