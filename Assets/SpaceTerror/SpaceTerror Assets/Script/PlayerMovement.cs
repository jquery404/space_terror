using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    public MPJoystick moveJoystick;                         // joystick class which control virtual joystick movement
    public GameObject GCInstance;                           // gamecontroller instance
    public GameObject laserProjectile;                      // laser projectile prefab for our player, it will be update later as dynamic array
    public GameObject HitPrefab;                            // small explosion prefab, instantiate when player hit
    public GameObject ExplosionPrefab;                      // big explosion prefab, intantiate when player died
    public bool AutoFire;                                   // player auto fire ability / press btn for fire
    public float HorizontalSpeed;                           // ship horizontal move speed
    public float VerticalSpeed;                             // ship vertical move speed
    public float fireRate;                                  // ship fire rate, how often it will fire projectile
    public float accelerationScaleX;
    public float accelerationScaleY;
    public float boundX1;                                   // ship movement bound for left (-ve x) dir
    public float boundX2;                                   // ship movement bound for right (+ve x) dir
    public float boundY1;                                   // ship movement bound for top (-ve y) dir
    public float boundY2;                                   // ship movement bound for down (+ve y) dir    

    // private vars
    private Transform tr;
    private Transform Ship;
    private Transform leftGun;
    private Transform rightGun;
    private Transform Shield;
    private Transform MainShip;
    private float nextFire = 0f;
    private int totalPower = 0;
    private float joystickX;
    private float joystickY;
    private Color matColor;
    private GameController GC;

    bool Fuck = true;
    bool negative = false;
    float smoothRotation = 180f;

	void Awake () 
    {
        tr = transform;
        //Shield = tr.FindChild("Shield").transform;
        Ship= tr.Find("ShipLock").transform;
        MainShip = Ship.Find("PlayerShip").transform;
        leftGun = Ship.Find("GunnerLeft").transform;
        rightGun = Ship.Find("GunnerRight").transform;
        GC = GCInstance.GetComponent("GameController") as GameController;           // gamecontroller cache        
        matColor = MainShip.GetComponent<Renderer>().material.color;                // player ship default mat color
	}

    void Start()
    {
        //Shield.GetComponent<Renderer>().enabled = false; 
        
        PoolManager.instance.CreatePool(laserProjectile, 20);               // laser
    }

	void Update () 
    {

        float horizontal = Input.GetAxisRaw("Horizontal") * Time.deltaTime * HorizontalSpeed;
        float vertical = Input.GetAxisRaw("Vertical") * Time.deltaTime * VerticalSpeed;

        joystickX = moveJoystick.position.x;
        joystickY = moveJoystick.position.y;

        //horizontal += -Input.acceleration.y * accelerationScaleY;
        //vertical += Input.acceleration.x * accelerationScaleX;

        horizontal += joystickX * accelerationScaleY * Time.deltaTime;
        vertical += joystickY * accelerationScaleX * Time.deltaTime;

        
        
        /* Banking rotation */

        // Debug.Log(Ship.localEulerAngles.z);

        if (horizontal < 0)
        {
            smoothRotation += 1f;
            negative = false;
            smoothRotation = Mathf.Clamp(smoothRotation, 180f, 220f);
        }
        else if (horizontal > 0)
        {
            smoothRotation += -1f;
            negative = true;
            smoothRotation = Mathf.Clamp(smoothRotation, 140f, 180f);
        }

        if (horizontal == 0)
        {
            if (Ship.localEulerAngles.z > 180f && !negative)
            {
                smoothRotation -= 0.5f;
            }
            if (Ship.localEulerAngles.z < 180f && negative)
            {
                smoothRotation += 0.5f;
            }

        }
                
        Ship.rotation = Quaternion.Euler(0, 0, smoothRotation); // Rotate uses Space.Self as default                

        tr.Translate(horizontal, 0, vertical);
        tr.position = new Vector3(Mathf.Clamp(tr.position.x, boundX1, boundX2), tr.position.y, Mathf.Clamp(tr.position.z, boundY1, boundY2));

        if (AutoFire)
        {
            FireProjectile(GameManager.LaserState);
        }
        

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }
	}


    void FireProjectile(int State)
    {
        if (Time.time > nextFire && !GameManager.isDead)
        {
            if (State == 1)
            {
                //Vector3 position = new Vector3(tForm.position.x, tForm.position.y, tForm.position.z + tForm.localScale.z / 2);
                PoolManager.instance.ReuseObject(laserProjectile, leftGun.position, Quaternion.identity);
                PoolManager.instance.ReuseObject(laserProjectile, rightGun.position, Quaternion.identity);
            }
            else if (State == 2)
            {
                Vector3 position = new Vector3(tr.position.x, tr.position.y, tr.position.z + tr.localScale.z / 2);
                PoolManager.instance.ReuseObject(laserProjectile, position, Quaternion.Euler(new Vector3(0, -20, 0)));
                PoolManager.instance.ReuseObject(laserProjectile, position, Quaternion.Euler(new Vector3(0, 20, 0)));
            }
            else if (State == 3)
            {
                Vector3 position = new Vector3(tr.position.x, tr.position.y, tr.position.z + tr.localScale.z / 2);
                Instantiate(laserProjectile, position, Quaternion.Euler(new Vector3(0, -20, 0)));
                Instantiate(laserProjectile, position, Quaternion.identity);
                Instantiate(laserProjectile, position, Quaternion.Euler(new Vector3(0, 20, 0)));
            }
            
            nextFire = Time.time + fireRate;
        }
    }

    public void PowerUp()
    {
        totalPower++;
        if (totalPower < 3)
        {
            GameManager.LaserState++;
        }
        else if(totalPower == 3)
        {
            Shield.GetComponent<Renderer>().enabled = true;
        }
    }

    void OnTriggerEnter(Collider col)
    {
                
        if (col.gameObject.tag == "Enemy" || col.gameObject.tag == "EnemyLaser")
        {            

            // if Player directly collide with enemy then health extra down 
            // then collide with enemy laser
            if (col.gameObject.tag == "Enemy")
            {                
                GameManager.PlayerHealth -= 15;
            }
            else
            {
                GameManager.PlayerHealth -= 5;
            }

            if (GameManager.PlayerHealth > 0)
            {
                Instantiate(HitPrefab, tr.position, Quaternion.identity);               
                StartCoroutine(GameManager.ColorBlinking(MainShip, matColor));
            }
            else
            {
                Instantiate(ExplosionPrefab, tr.position, Quaternion.identity);
                GameManager.isDead = true;                
                GC.isGameOver();
            }
            col.gameObject.GetComponent<LaserMovement>().DestroyNow();
            
        }
        
    }
}
