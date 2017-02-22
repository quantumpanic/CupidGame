using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ProgressiveRNG : MonoBehaviour {

	public bool Iterate;
	public bool ControlledRandom;
	public bool ProgressiveChance;
	
	// Use this for initialization
	void Start () {
	}

	public int tries;
	public int iterations;

	// Update is called once per frame
	void Update () {
		for (int x = 0; x < iterations; x++) {
			Iteration ();
		}
	}

	void Iteration()
	{
		ClearStreaks ();

		for (int x = 0; x < tries; x++) {
			Roll (ControlledRandom);
		}
		BreakWinStreak ();
		BreakFailStreak ();

		AverageStreaks ();
		MeanStreaks ();
		MinMaxStreaks ();

		Report ();
	}

	public float baseChance = 0.2f;
	public float baseWeight = 0.2f;
	public float variableWeight = 0f;
	public float totalWeight = 0f;
	public float totalChance { get { return baseChance + totalWeight; } }
	float maxChance = 1;

	float GenerateNumber()
	{
		return Random.Range(0f,maxChance);
	}

	bool Success(bool controlledRandom = true)
	{
		if (controlledRandom)
			return GenerateNumber () <= totalChance;
		else
			return GenerateNumber () <= baseChance;
	}

	private int winStreak;
	private int failStreak;
	public List<int> winStreaks = new List<int>();
	public List<int> failStreaks = new List<int>();

	void Roll(bool controlled)
	{
		if (Success (controlled)) {
			totalWeight = totalChance - maxChance;
			print ("Success");
			BreakFailStreak ();
			winStreak++;
		} else {
			if (!ProgressiveChance && totalWeight < baseChance)
				totalWeight += baseWeight;
			else if (ProgressiveChance)
				totalWeight += baseWeight;
			print ("Failed");
			BreakWinStreak ();
			failStreak++;
		}
	}

	void BreakWinStreak()
	{
		if (winStreak > 0) {
			winStreaks.Add (winStreak);
			winStreak = 0;
		}
	}

	void BreakFailStreak()
	{
		if (failStreak > 0) {
			failStreaks.Add (failStreak);
			failStreak = 0;
		}
	}

	private float avgWinStreak;
	private float avgFailStreak;

	void AverageStreaks()
	{
		avgWinStreak = 0;
		foreach (int i in winStreaks)
			avgWinStreak += i;
		avgWinStreak /= winStreaks.Count;

		avgFailStreak = 0;
		foreach (int j in failStreaks)
			avgFailStreak += j;
		avgFailStreak /= failStreaks.Count;
	}

	private int meanWin;
	private int meanFail;

	void MeanStreaks()
	{
		winStreaks.Sort ();
		failStreaks.Sort ();

		meanWin = winStreaks [winStreaks.Count / 2];
		meanFail = failStreaks [failStreaks.Count / 2];
	}

	private int minWin;
	private int minFail;
	private int maxWin;
	private int maxFail;

	void MinMaxStreaks()
	{
		minWin = winStreaks [0];
		minFail = failStreaks [0];

		maxWin = winStreaks [winStreaks.Count - 1];
		maxFail = failStreaks [failStreaks.Count - 1];
	}

	void Report()
	{
		print ("Avg " + avgWinStreak.ToString("F4") + ":" + avgFailStreak.ToString("F4") + ", "
			+ "Mean " + meanWin + ":" + meanFail + ", "
			+ "Min " + minWin + ":" + minFail + ", "
			+ "Max " + maxWin + ":" + maxFail);
	}

	void ClearStreaks()
	{
		winStreak = 0;
		failStreak = 0;
		winStreaks.Clear ();
		failStreaks.Clear ();
	}
}
