using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Conveyer : MonoBehaviour {

	public List<GameObject> objects = new List<GameObject>();

	public ILane lane = new StandardLane ();

	// Use this for initialization
	void Start () {
		AddLane ();
	}
	
	// Update is called once per frame
	void Update () {
		MoveConveyer ();
	}

	void AddLane()
	{
		lane.New (this);
	}

	public int counter = 0;

	public void AddObject(GameObject obj)
	{
		counter++;
		obj.name = "Object-" + gameObject.name + counter.ToString ();
		objects.Add (obj);
	}

	public void RemoveAtIndex(int i)
	{
		RemoveObject(objects[i]);
	}

	public void RemoveObject(GameObject obj)
	{
		if (objects.Contains (obj)) {
			RectTransform rct = obj.GetComponent<RectTransform>();
			Enemy enemy = obj.GetComponent<Enemy> ();

			float sideStep = (enemy.matchType == MatchType.Male) ? 250f : -250f;

			rct.localPosition = new Vector2 (
				sideStep,
				//rct.GetComponent<RectTransform> ().localPosition.y);
				0);
			rct.localScale = Vector2.one;

			objects.Remove (obj);
			enemy.GetHit (false);
		}
	}

	public float speed = 10f;
	public float stopDelay = 10f;

	void MoveConveyer()
	{
		for (int i = 0; i <= objects.Count -1; i++) {
			// move down
			objects [i].GetComponent<RectTransform> ().localPosition = new Vector2 (
					objects [i].GetComponent<RectTransform> ().localPosition.x,
				objects [i].GetComponent<RectTransform> ().localPosition.y - (speed * Time.deltaTime));
		}
	}
}