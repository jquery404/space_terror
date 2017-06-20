using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	private static Player instance = new Player ();
	public static Player get(){ return instance; }


	public GameObject fireProjectile;
	public GameObject explosionParticle;
	public GameObject sparkParticle;
	public MPJoystick mpJoystick; 
	public Vector3 spawnPosition;
	public bool autoFire;   
	public bool isDead;
	public float fireRate;
	public int laserLevel;
	public int totalEnemyKilled;
	public Vector2 speed;
	public Vector4 bound;
	public Vector2 acceleration;
    public Color collideColor;


	private Transform tr;
	private Transform playerShip;
	private Transform leftGun;
	private Transform rightGun;
	private Vector2 currentSpeed;
	private Vector2 joyStick;
	private int powerLevel;
	private int score;
	private int heath;
	private bool reverseAngle;
	private float nextFire = 0f;
	private float smoothRotation = 180f;
    private Material playerMat;
	private Color matColor;
	private PoolManager pm;


	void Awake(){
		tr = this.transform;
		tr.position = spawnPosition;
		playerShip = tr.Find ("ShipLock").transform;
		leftGun = playerShip.Find ("GunnerLeft").transform;
		rightGun = playerShip.Find ("GunnerRight").transform;
        playerMat = playerShip.Find("PlayerShip").GetComponent<Renderer>().material;
        matColor = playerMat.color;
        pm = PoolManager.instance;
	}

	void Start (){
		pm.CreatePool (fireProjectile, 20);
	}

	void Update (){
		currentSpeed.x = Input.GetAxisRaw ("Horizontal") * Time.deltaTime * speed.x;
		currentSpeed.y = Input.GetAxisRaw ("Vertical") * Time.deltaTime * speed.y;
		joyStick.x = mpJoystick.position.x;
		joyStick.y = mpJoystick.position.y;

		currentSpeed.x += joyStick.x * acceleration.x * Time.deltaTime; 
		currentSpeed.y += joyStick.y * acceleration.y * Time.deltaTime;

		if (currentSpeed.x < 0f) {
			smoothRotation += 1f;
			smoothRotation = Mathf.Clamp (smoothRotation, 180f, 220f);
			reverseAngle = false;
		} else if (currentSpeed.x > 0f) {
			smoothRotation -= 1f;
			smoothRotation = Mathf.Clamp (smoothRotation, 140f, 180f);
			reverseAngle = true;
		} else {
			if (playerShip.localEulerAngles.z > 180f && !reverseAngle) {
				smoothRotation -= .5f;
			} else if (playerShip.localEulerAngles.z < 180f && reverseAngle) {
				smoothRotation += .5f;
			}
		}

		if (autoFire)
        {
            Fire(laserLevel);
            StartCoroutine("collideFlash");
            autoFire = false;
        }


        playerShip.rotation = Quaternion.Euler (0, 0, smoothRotation);
		tr.Translate (currentSpeed.x, 0, currentSpeed.y);
		tr.position = new Vector3 (Mathf.Clamp (tr.position.x, bound.x, bound.y), 
			tr.position.y, Mathf.Clamp (tr.position.z, bound.z, bound.w));
	}

	void Fire(int Level){

		if (Time.time > nextFire && !isDead) {

			if (Level == 1)
			{
				pm.ReuseObject (fireProjectile, leftGun.position, Quaternion.identity);
				pm.ReuseObject (fireProjectile, rightGun.position, Quaternion.identity);
			}
			else if (Level == 2)
			{

			}
			else if (Level == 3)
			{

			}
			nextFire = Time.time + fireRate;
		}
	}

	IEnumerator collideFlash(){
        playerMat.color = collideColor;
        yield return new WaitForSeconds(0.1f);
        playerMat.color = matColor;
        yield return new WaitForSeconds(0.1f);
        playerMat.color = collideColor;
        yield return new WaitForSeconds(0.1f);
        playerMat.color = matColor;
    }
}
