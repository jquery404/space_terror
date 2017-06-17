using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	private static Player _player = new Player ();
	public static Player get (){ return _player; }

	public GameObject fireProjectile;
	public GameObject explosionParticle;
	public GameObject sparkParticle;
	public MPJoystick mpJoystick; 
	public Vector3 spawnPosition;
	public bool autoFire;   
	public float fireRate;
	public Vector2 speed;
	public Vector4 bound;
	public Vector2 acceleration;

	private Transform tr;
	private int enemyKilled;
	private int powerLevel;
	private int laserLevel;
	private int score;
	private int heath;
	private float nextFire = 0f;
	private bool isDead;


	void Awake(){
		tr = this.transform;
		tr.position = spawnPosition;
	}

	void Start (){

	}

	void Update (){

	}

	void Fire(int Level){

		if (Time.time > nextFire && !GameManager.isDead) {
			if (Level == 1)
			{


			}
			else if (Level == 2)
			{

			}
			else if (Level == 3)
			{

			}
		}

		nextFire = Time.time + fireRate;
	}
}
