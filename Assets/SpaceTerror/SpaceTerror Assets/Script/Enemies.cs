using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour {

	public Vector3[] spawnPoints;
	public float spawnSpeed;

	private enum SpawnType { RANDOM, ASSEMBLE, PARADE, PYRAMID };
	private enum EnemyType { ASTEROID, MINICRAFT, BANZAI, FOOFIGHTER, DOGGYTRAIL, MOTHERSHIP };


	void Start () {

	}

	void Update () {

	}
}
