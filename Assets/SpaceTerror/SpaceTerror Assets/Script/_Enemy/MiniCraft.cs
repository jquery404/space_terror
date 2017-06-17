using UnityEngine;
using System.Collections;

public class MiniCraft : MonoBehaviour {

    /* Damb Move and Track Once */

    public GameObject Projectile;
    public float moveSpeed;
    public float fireRate;

    //private GameController GC;
    private Transform tr;
    private Transform Gunner1;
    private Transform Gunner2;
    private float rollingDice;
    private float nextFire = 0;
    private bool AutoPilot = true;
    private bool locked = false;
    private Transform target;

    void Awake()
    {
        tr = transform;
        Gunner1 = tr.FindChild("Gunner_1").transform;
        Gunner2 = tr.FindChild("Gunner_2").transform;
        rollingDice = Mathf.Floor(Random.Range(0, 10));
        //GC = (GameController)GetComponent("GameController");
       
    }
    
    void Start () {        
        target = GameObject.FindWithTag("Player").transform;
        PoolManager.instance.CreatePool(Projectile, 20);               // laser
    }
	
	
	void Update () {
        
        if(rollingDice > 3)
        {
            if(AutoPilot)
            {
                if (!locked)
                {
                    tr.Rotate(Vector3.up * 180f);
                    
                    locked = true;
                    
                }
                tr.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            }
        }
        else if(rollingDice > 6)
        {
            if(AutoPilot)
            {
                if (!locked)
                {
                    tr.Rotate(Vector3.up * 180f);
                    locked = true;
                }
                tr.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
                FireProjectile();
            }	
        }
        else
        {

            if(AutoPilot)
            {
                tr.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            }
            if(!locked)
            {
                tr.LookAt(target);                
                locked = true;
            }

            FireProjectile();
	        
        }


        if (tr.position.z < -5.5f)
        {
            GameController.ResetPosition(tr);
            //Destroy(gameObject);
        }

    }

    void FireProjectile()
    {
        if (Time.time > nextFire)
        {
            PoolManager.instance.ReuseObject(Projectile, Gunner1.position, Gunner1.rotation);
            PoolManager.instance.ReuseObject(Projectile, Gunner2.position, Gunner2.rotation);

            nextFire = Time.time + fireRate;
        }
    }
}
