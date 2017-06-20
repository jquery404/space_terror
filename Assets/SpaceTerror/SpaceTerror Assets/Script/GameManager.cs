using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	private static GameManager instance = new GameManager();
	public static GameManager get(){ return instance; }

	public int currentLevel;
	public int curLevel { get; set; }
	public int totalEnemy;
	public int totLevel { get; set; }

	private bool isFinish;
	private bool isPause;
	private enum Result { WIN, LOSE};

	void Awake(){
		instance = this;
		curLevel = currentLevel;
		totLevel = totalEnemy;
	}

	void Start () {
		
	}
	
	void Update () {
		
	}


	public void GameOver(){
		int Accuracy = (int)Mathf.Floor ((Player.get ().totalEnemyKilled / totalEnemy) * 100f);  
	}

	public void SaveGame(){
		
	}

	public void RestartGame(){
		
	}
}
