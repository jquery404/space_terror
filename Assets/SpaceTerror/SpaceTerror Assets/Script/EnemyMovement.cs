using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {


    public float moveSpeed;
    public bool canFire;
    public bool Tracking;
    public GameObject EnemyProjectile;
    public GameObject ExplosionPrefab;
    public GameObject PointGUI;

    public float Range1;
    public float Range2;
    
    public float TrackingDuration;
    public float fireRate;
    
    private float nextFire = 0f;
    private Vector3 initPos;
    private Transform PlayerShip;
    private Transform tForm;
    private bool autoPilot = true;
    private bool pauseMove = false;
    private Transform enemyGun;
	
	void Start () {
        tForm = transform;
        PlayerShip = GameObject.FindWithTag("Player").transform;
        initPos = tForm.position;
        if (canFire)
        {
            enemyGun = tForm.FindChild("Cube").transform;
        }
	}	
	
	void Update () {

        
        if (autoPilot)
        {
            tForm.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
        }        

        // Player inside range then fire
        /* */
        if (tForm.position.z < Range1 && canFire)
        {
            if (Time.time > nextFire) 
            {
                Fire();
                nextFire = Time.time + fireRate;
            }   
            
        }
        
        /*
        // Player inside range1 look and range2 fire
        if (tForm.position.z < Range1 && canFire) 
        {
            if (autoPilot)
            {
                FollowShip();
            }
            
            autoPilot = false;
            tForm.Translate(Vector3.forward * Time.deltaTime * -moveSpeed);

            if (tForm.position.z < Range2) 
            {
                if (Time.time > nextFire)
                {
                    Fire();
                    nextFire = Time.time + fireRate;
                }   
            }
        }

        
        
        // SuisideShip
        if (tForm.position.z < Range1 && canFire)
        {
            if (autoPilot)
            {
                FollowShip();
            }

            autoPilot = false;
            tForm.Translate(Vector3.forward * Time.deltaTime * -moveSpeed*2);

            
        }
        
        
        // SlowShip
        if (tForm.position.z < Range1 && canFire && !pauseMove)
        {
            if (autoPilot) {
                Invoke("EscapeShip", 3);
            }

            autoPilot = false;



            tForm.position = new Vector3(initPos.x + Mathf.PingPong(Time.time, 3), tForm.position.y, tForm.position.z);
            
            
            if (Time.time > nextFire)
            {
                Fire();
                nextFire = Time.time + fireRate;
            }   

        }
         
        
        // MotherShip
        if ( canFire )
        {
            Quaternion targetRotation = Quaternion.LookRotation(enemyGun.position - PlayerShip.position, Vector3.up);
            enemyGun.rotation = Quaternion.Slerp(enemyGun.rotation, targetRotation, Time.deltaTime * 2f);
                

            //enemyGun.LookAt(PlayerShip);
        }
        */
        if (tForm.position.z < -5.5f) 
        {
            Destroy(gameObject);
        }
	}

    void FollowShip()
    {        
        tForm.LookAt(PlayerShip);
    }

    void EscapeShip()
    {
        pauseMove = true;
        autoPilot = true;
    }

    void Fire()
    {
        Instantiate(EnemyProjectile, tForm.position, tForm.rotation);
    }


    /*
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Laser")
        {
            Destroy(col.gameObject);
            PlayerMovement.Score += 50;
            Vector3 viewportPos = Camera.main.WorldToViewportPoint(tForm.position);
            Instantiate(PointGUI, new Vector3(viewportPos.x, viewportPos.y, 0), Quaternion.identity);
            Instantiate(ExplosionPrefab, tForm.position, Quaternion.identity);
            Destroy(gameObject);            
        }
    }
    */

}
