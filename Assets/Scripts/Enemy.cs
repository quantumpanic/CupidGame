using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, IMatchable {

	public GameObject body { get { return gameObject; } }

	ISpriteModel[] allModels = new ISpriteModel[2] {new RandomMaleModel(), new RandomFemaleModel()};
	ISpriteModel curModel;

	public Image headSprite;
	public Image bodySprite;
	public Image dressSprite;
	public Image faceSprite;

	// Use this for initialization
	void Start () {
		curModel = allModels [Random.Range (0, 2)];
	}
	
	// Update is called once per frame
	void Update () {
		Expand ();
		CheckDie ();
	}

	public RectTransform rect;

	public void Advance(int steps = 1)
	{
		// move towards the player n steps

	}

	void Expand()
	{
		rect.localScale *= 1.02f;
		if (rect.localScale.x >= 1)
			rect.localScale = Vector2.one;
	}

	void Shrink()
	{
		rect.localScale = Vector2.one * 0.2f;
	}

	void CheckDie()
	{
		if (rect.localPosition.y < -500) {
			Die ();
		}
	}

	void Die()
	{
		if (name.Contains("L"))
			GameObject.Find ("ConveyerL").GetComponent<Conveyer> ().RemoveObject (gameObject);
		if (name.Contains("R"))
			GameObject.Find ("ConveyerR").GetComponent<Conveyer> ().RemoveObject (gameObject);

		// return to pool
		if (!PoolingManager.Instance.enemyPool.Contains (gameObject)) {
			PoolingManager.Instance.Put (gameObject, "enemy");
		} else {
			print (name + " already in pool");
		}

		// put in back
		transform.SetAsFirstSibling();

		// shrink
		Shrink();

		tag = "enemy";
	}

	public void Poof()
	{
		Invoke("PoofAnim",1f);
	}

	void PoofAnim()
	{
		// poof with lovely animation
		Die ();
	}

	public MatchType matchType { get; set; }

	public void GetHit(bool hitByArrow)
	{
		if (tag == "Untagged")
			return;
		// notify game manager
		tag = "Untagged";

		// if not hit by arrow, just die
		if (!hitByArrow)
			PoofAnim ();
	}
}

public interface IMatchable
{
	GameObject body { get; }
	MatchType matchType { get; }
}

public interface ISpriteModel
{
	void AssignSprites (GameManager gameManager, Enemy enemy);
}

public class RandomMaleModel : ISpriteModel
{
	public void AssignSprites(GameManager gameManager, Enemy enemy)
	{
		enemy.headSprite.sprite = gameManager.spriteCollection.FindAll(hair => hair.name.StartsWith("mhair"))[Random.Range(0,3)];
		enemy.bodySprite.sprite = gameManager.spriteCollection.FindAll(body => body.name.StartsWith("naked"))[Random.Range(0,1)];
		enemy.dressSprite.sprite = gameManager.spriteCollection.FindAll(dress => dress.name.StartsWith("mbody"))[Random.Range(0,3)];
	}
}

public class RandomFemaleModel : ISpriteModel
{
	public void AssignSprites(GameManager gameManager, Enemy enemy)
	{
		enemy.headSprite.sprite = gameManager.spriteCollection.FindAll(hair => hair.name.StartsWith("fhair"))[Random.Range(0,3)];
		enemy.bodySprite.sprite = gameManager.spriteCollection.FindAll(body => body.name.StartsWith("naked"))[Random.Range(0,1)];
		enemy.dressSprite.sprite = gameManager.spriteCollection.FindAll(dress => dress.name.StartsWith("fbody"))[Random.Range(0,3)];
	}
}

public class RandomBinaryModel : ISpriteModel
{
	public void AssignSprites(GameManager gameManager, Enemy enemy)
	{
		// check the game manager counter
		int genderIndex = gameManager.genderIndexes[gameManager.GetGenderCounter()];

		// get the correct gender
		ISpriteModel currentGender = gameManager.allModels[genderIndex];

		enemy.faceSprite.gameObject.SetActive(false);

		// now assign sprites
		currentGender.AssignSprites(gameManager, enemy);
	}
}

public class LovelyFace : ISpriteModel
{
	public void AssignSprites(GameManager gameManager, Enemy enemy)
	{
		enemy.faceSprite.sprite = gameManager.spriteCollection.FindAll(body => body.name.StartsWith("facesad"))[0];
		enemy.faceSprite.gameObject.SetActive(true);
	}
}

public class ShockedFace : ISpriteModel
{
	public void AssignSprites(GameManager gameManager, Enemy enemy)
	{
		enemy.faceSprite.sprite = gameManager.spriteCollection.FindAll(body => body.name.StartsWith("facesurprise"))[0];
		enemy.faceSprite.gameObject.SetActive(true);
	}
}