using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * n = level, t = time, D = damage, I = Increase rate, T = Threshold      
 * 
 * SpawnRate = 50 * n / t[n-1] - 30 * D[t-1]     
 * DamageRate = En - E(n-1) / n - (n-1)
 * LevelBudget = T + n * I
 * UnitBudgets[EnemyTypes.MiniCraft]
 * weight, points
 * 
 */
public class Enemies : MonoBehaviour {

	public GameObject[] enemyUnits;
	public Vector3[] spawnPoints;
	public float spawnSpeed;
	public enum SpawnType { RANDOM, ASSEMBLE, PARADE, PYRAMID };
	public enum ShipType { ASTEROID, MINICRAFT, BANZAI, FOOFIGHTER, DOGGYTRAIL, MOTHERSHIP };

	private Dictionary<int, TroopBudget> troopsBudget;
	private int availEnemyUnit = 0; // available unit for current level
		

	void Awake(){
		// store budget for each troop
		troopsBudget = new Dictionary<int, TroopBudget>(enemyUnits.Length);
	}

	void Start () {
		float current_budget = LevelBudget (20, 1);
		Debug.Log (current_budget);
		//Checkout (current_budget);
	}

	void Update () {
		

	}


	// T = threshold, I = increment
	// LevelBudget = T + n * I
	private float LevelBudget(float T, float I){
		return (T + GameManager.get().curLevel * I);
	}

	private int getAvailableUnit(){
		//return Mathf.Clamp(GameManager.get ().curLevel, enemyUnits.Length);
		return 0;
	}

	private void Checkout(float currentBudget){
		while (currentBudget > 0) {
			float totalWeight = 0;
			for (int i = 0; i < availEnemyUnit; i++) {
				if(currentBudget >= troopsBudget[i].points){
					totalWeight += troopsBudget [i].weights;
				}
			}
		}
	}

	public void getShipCost(ShipType shipType){
		
	}


	// weights =  amount of space occupied by the troop
	// points = amount that cost to purchase / deploy that troop
	public class TroopBudget{
		public float weights{ get; set; }
		public float points{ get; set; }
	}
}
