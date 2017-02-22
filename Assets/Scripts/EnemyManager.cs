using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

	public GameManager gameManager;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void BeginSpawning()
	{
		InvokeRepeating ("AddEnemies", startInterval, enemyInterval);
	}

	public void StopSpawning()
	{
		CancelInvoke ("AddEnemies");
	}

	public List<GameObject> conveyers = new List<GameObject> ();
	public float enemyInterval;
	public float startInterval;

	void AddEnemies()
	{
		// make 1 girl and 1 boy by shuffling the list
		gameManager.ShuffleGenderIndexes();

		foreach (GameObject conv in conveyers) {
			var enemy = PoolingManager.Instance.Grab("enemy");
			if (enemy) {
				enemy.transform.localPosition = new Vector2(conv.transform.localPosition.x, conv.transform.localPosition.y + 500);
				conv.GetComponent<Conveyer> ().AddObject (enemy);
				gameManager.NewEnemy (enemy);
			}
		}
	}
}
