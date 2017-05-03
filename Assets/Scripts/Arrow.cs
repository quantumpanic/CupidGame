using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Arrow : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Reset ();
	}

	public RectTransform rect;
	public Image image;
	public float speed = 10.0F;
	bool isFading;

	// Update is called once per frame
	void Update () {
		Shrink ();
		rect.anchoredPosition = new Vector2 (rect.anchoredPosition.x, rect.anchoredPosition.y + (Time.deltaTime * speed * 100));
		if (rect.anchoredPosition.y > Screen.height/2 - 300) {
			image.CrossFadeAlpha (0, 0.1f, false);
			if (!isFading)
				Fizzle ();
		}
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.tag != "enemy")
			return;

		// remove the bullet
		Recycle ();

		Enemy enemy = col.GetComponent<Enemy> ();
		new HitEnemy ().DoAfterHit (GameObject.Find ("GameManager").GetComponent<GameManager> (), enemy);
	}
		
	void Shrink()
	{
		rect.localScale *= 0.95f;
		if (rect.localScale.x <= 0.2f)
			rect.localScale = (Vector3.one - (Vector3.up * 2)) * 0.2f;
	}

	void Fizzle()
	{
		isFading = true;
		Invoke ("Recycle", 0.2f);
	}

	void Reset()
	{
		image.color = Vector4.one;
		isFading = false;
		rect.localScale = Vector2.one;
	}

	void Recycle()
	{
		PoolingManager.Instance.Put (gameObject, "arrow");
		Reset ();
	}
}

public interface IHitSomething
{
	void DoAfterHit(GameManager gameManager, Enemy enemy);
}

public class HitEnemy : IHitSomething
{
	public void DoAfterHit(GameManager gameManager, Enemy enemy)
	{
		gameManager.AddCandidate (enemy);
		enemy.GetHit (true);

		foreach (GameObject g in gameManager.enemyManager.conveyers) {
			//g.GetComponent<Conveyer>().RemoveObject (enemy.body);
			g.GetComponent<Conveyer>().RemoveAtIndex(0);
		}
	}
}