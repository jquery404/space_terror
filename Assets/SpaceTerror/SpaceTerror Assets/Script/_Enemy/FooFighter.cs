using UnityEngine;
using System.Collections;

public class FooFighter : MonoBehaviour {

    /* Depends on Range and Take multi Range */

    public float Range1;
    public float Range2;
    public GameObject Projectile;
    public float moveSpeed;
    public float ppLength;
    public float wiggleSpeed;
    public float fireRate;

    private Transform tr;
    private Transform Gunner1;
    private Transform Gunner2;
    private float rollingDice;
    private float nextFire = 0;
    private bool AutoPilot = true;
    private bool locked = false;
    private Transform target;
    private float curPosX;
    //private bool invoked;
    private bool called;


    void Awake()
    {
        tr = transform;
        Gunner1 = tr.Find("Gunner_1").transform;
        Gunner2 = tr.Find("Gunner_2").transform;
        //invoked = false;
        called = false;
        rollingDice = Mathf.Floor(Random.Range(0, 10));
        target = GameObject.FindWithTag("Player").transform;
    }

    void Start()
    {
        InvokeRepeating("Abroad", 3f, 5f);
    }
      

	void Update () {
        
        if (!locked)
        {
            tr.Rotate(Vector3.up * 180f);
            locked = true;
        }

       
        if (AutoPilot)
        {
            tr.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }

        if (AutoPilot && tr.position.z < Range1)
        {
            curPosX = tr.position.x;            
            if (rollingDice > 5)
            {
                tr.LookAt(target);
            }
            AutoPilot = false;
        }
        if (!AutoPilot)
        {    
            if (!called)
            {
                tr.LookAt(target);
                float newX = Mathf.PingPong(Time.time * wiggleSpeed, ppLength);
                tr.position = new Vector3(curPosX + newX, tr.position.y, tr.position.z);
                FireProjectile();                
            }
            else 
            {
                tr.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
                tr.Rotate(Vector3.up * 90f * Time.deltaTime);
            }
        }

        if (tr.position.z < -5.5f)
        {
//            GameController.ResetPosition(tr);
        }

	}

    void Abroad()
    {
        called = (called == false) ? true : false;
    }

    void FireProjectile()
    {
        if (Time.time > nextFire)
        {
            Instantiate(Projectile, Gunner1.position, Gunner1.rotation);
            Instantiate(Projectile, Gunner2.position, Gunner2.rotation);
            nextFire = Time.time + fireRate;
        }
    }
    

}
