using UnityEngine;
using System.Collections;

public class DoggyTrail : MonoBehaviour {

    /* Change Movement and Track repeateadly */

    public float Range;
    public float Range1;
    public float Range2;
    public GameObject Projectile;
    public float moveSpeed;    
    public float fireRate;

    private Transform tForm;
    private float rollingDice;
    private float nextFire = 0;
    private bool AutoPilot = true;
    private bool locked = false;
    private Transform target;

    void Awake()
    {
        tForm = transform;        
        rollingDice = Mathf.Floor(Random.Range(0, 10));
        target = GameObject.FindWithTag("Player").transform;
    }

    
	void Update () {

        if (!locked)
        {
            InvokeRepeating("rotateAround", 1f, 0.5f);
            tForm.LookAt(target);
            locked = true;
        }
        //FireProjectile();   
        //tForm.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        //if (AutoPilot)
        //{
          //  tForm.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        //}
        if (!AutoPilot)
        {
            //tForm.Rotate(Vector3.up * 90f * Time.deltaTime);
        }
        else 
        {
            //tForm.Rotate(Vector3.up * Time.time * 45f);
        }

        Range = Vector3.Distance(target.position, tForm.position);
        

        if (AutoPilot && Range < Range1)
        {            
            //AutoPilot = false;
        }

        if (Range < Range1)
        {
            FireProjectile();  
        }
        else 
        {
            tForm.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }

        if (tForm.position.z < -5.5f)
        {
            GameController.ResetPosition(tForm);
        }

	}

    void rotateAround()
    {
        tForm.LookAt(target);
    }

    void FireProjectile()
    {
        if (Time.time > nextFire)
        {
            Instantiate(Projectile, tForm.position, tForm.rotation);
            nextFire = Time.time + fireRate;
        }
    }

}
