using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		spriteCollection.AddRange(Resources.LoadAll<Sprite> ("Aset"));
		enemyManager.BeginSpawning ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public List<Sprite> spriteCollection = new List<Sprite>();
	public ISpriteModel[] allModels = new ISpriteModel[2] {new RandomMaleModel(), new RandomFemaleModel()};

	public List<int> genderIndexes = new List<int> (2) { 0, 1 };
	private int genderIndexCounter;

	public void ShuffleGenderIndexes()
	{
		List<int> temp = new List<int>(2) { 0, 1 };
		List<int> newList = new List<int> (2);

		foreach (int i in genderIndexes) {
			int rand = Random.Range (0, temp.Count);

			newList.Add (temp [rand]);
			temp.RemoveAt (rand);
		}

		genderIndexes = newList;

		// reset the counter
		genderIndexCounter = 0;
	}

	public int GetGenderCounter()
	{
		int counter = genderIndexCounter;
		genderIndexCounter++;

		return counter;
	}

	public void NewEnemy(GameObject enemy)
	{
		Enemy script = enemy.GetComponent<Enemy> ();
		if (!script)
			return;

		// determine what kind of enemy here
		script.matchType = (MatchType)genderIndexes[genderIndexCounter];
		NewBinaryEnemy(script);
	}

	public void NewBinaryEnemy(Enemy enemy)
	{
		AssignSprites (new RandomBinaryModel(), enemy);
	}

	public ISpriteModel BinarySpriteModel()
	{
		return allModels [Random.Range (0, 2)];
	}

	public void AssignSprites(ISpriteModel model, Enemy enemy)
	{
		model.AssignSprites (this, enemy);
	}

	public bool isPlaying;
	public int curScore = 0;

	public EnemyManager enemyManager;

	public void StartGame()
	{
		enemyManager.BeginSpawning ();
	}

	public List<IMatchable> matchCandidates = new List<IMatchable> (2) { null, null };

	public void AddScore(int score)
	{
		curScore += score;
	}

	public void AddCandidate(IMatchable candidate)
	{
		matchCandidates.Add (candidate);
		matchCandidates.RemoveAt (0);

		// give loevely face
		new LovelyFace().AssignSprites(this, candidate.body.GetComponent<Enemy>());

		if (!matchCandidates.Contains(null)) {
			ResolveMatch ();
		}
	}

	List<IMatchCondition> matchConditions = new List<IMatchCondition> (){ new MatchBoyGirl () };
	IMatchCondition currentMatchCondition = new MatchBoyGirl();

	void ResolveMatch()
	{
		IReward reward = null;

		if (currentMatchCondition.ResolveMatch (matchCandidates, out reward))
			CorrectMatch (reward);
		else
			FalseMatch();
	}

	public void CorrectMatch(IReward reward)
	{
		// remove couple and add score
		reward.ImburseReward(this);
	}

	public void FalseMatch()
	{
		GameOver ();
	}

	public void GameOver()
	{
		isPlaying = false;

		// show some UI
	}

	void EndGame()
	{
		enemyManager.StopSpawning ();

		// reset counters
		curScore = 0;
	}
}

public enum MatchType
{
	Male,
	Female,
	Powerup
}

public interface IMatchCondition
{
	bool ResolveMatch(List<IMatchable> list, out IReward reward);
}

public class MatchBoyGirl : IMatchCondition
{
	public bool ResolveMatch(List<IMatchable> list, out IReward reward)
	{
		bool success = false;
		reward = new NoReward ();

		if (list.Exists (male => male.matchType == MatchType.Male)
		    &&
		    list.Exists (female => female.matchType == MatchType.Female)) {

			// there is at least one male and one female
			reward = new RewardScore100();

			success = true;
		}

		// poof the candidates
		foreach (IMatchable match in list) {
			match.body.GetComponent<Enemy> ().Poof ();
		}

		// reset the list
		list [0] = null;
		list [1] = null;

		return success;
	}
}

public interface IReward
{
	void ImburseReward(GameManager gameManager);
}

public class NoReward : IReward
{
	public void ImburseReward(GameManager gameManager)
	{
		// do nothing
	}
}

public class RewardScore100 : IReward
{
	public void ImburseReward(GameManager gameManager)
	{
		gameManager.AddScore (100);
	}
}