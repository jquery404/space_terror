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
	public Transform[] spawnPoints;
	public float spawnSpeed;
	public enum SpawnType { RANDOM, ASSEMBLE, PARADE, PYRAMID };
	public List<int> availEnemyUnits; // available unit for current level


	private int totalEnemyUnit;
	private float levelBudget;
	private float levelWeight;
	private PoolManager pm;
		

	void Awake(){
		// store budget for each troop
		pm = PoolManager.instance;

		for (int i = 0; i < availEnemyUnits.Count; i++) {
			pm.CreatePool (enemyUnits[availEnemyUnits[i]], 10);
		}
		List<int> troops = Checkout ();

	}

	void Start () {
		

	}



	// T = threshold, I = increment
	// LevelBudget = T + n * I
	private float LevelBudget(float T, float I){
		return (T + GameManager.get().curLevel * I);
	}

	// T = threshold, I = increment
	// LevelWeight = T + n * I
	private float LevelWeight(float T, float I){
		return (T + GameManager.get().curLevel * I);
	}

	private int getAvailableUnit(){
		//return Mathf.Clamp(GameManager.get ().curLevel, enemyUnits.Length);
		return 0;
	}

	private List<int> Checkout(){
		List<int> troops = new List<int> ();
		levelBudget = LevelBudget (20, 1);
		levelWeight = LevelWeight (15, 1);
		while (levelBudget > 0) {
			int index = Random.Range (0, availEnemyUnits.Count);
			GameObject go = pm.ReuseObject (enemyUnits [availEnemyUnits [index]], 
				Vector3.zero, 
				Quaternion.identity);
			EnemyShip enemyShip = go.GetComponent<EnemyShip> ();
			Debug.Log ("=>"+enemyShip.weights);
            pm.ReuseObject(enemyUnits[Random.Range(0, availEnemyUnits.Count)], 
                Vector3.zero, 
                Quaternion.identity);

			/*if (levelBudget >= enemyShip.cost &&
				levelWeight >= enemyShip.weights) {
				troops.Add (availEnemyUnits [index]);
				levelWeight -= enemyShip.weights;
				levelBudget -= enemyShip.cost;
			} else {
				availEnemyUnits.Remove (index);
			}*/
			--levelBudget;
		}

		return troops;
	}


}
