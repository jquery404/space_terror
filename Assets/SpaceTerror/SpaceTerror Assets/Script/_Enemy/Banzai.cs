using UnityEngine;
using System.Collections;

public class Banzai : MonoBehaviour {

    /* Suisidal Ship */

    public float Range1;
    public float Range2;
    public GameObject Projectile;
    public float moveSpeed;
    public float wiggleSpeed;
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

    void FixedUpdate()
    {

        if (!locked)
        {
            tForm.Rotate(Vector3.up * 180f);
            locked = true;
        }

        tForm.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

        if (rollingDice > 5)
        {
            if (AutoPilot && tForm.position.z < Range1)
            {
                moveSpeed *= wiggleSpeed;
                AutoPilot = false;
                
            }
        }
        else 
        {
            if (AutoPilot && tForm.position.z < Range1)
            {   
                moveSpeed *= wiggleSpeed;
                tForm.LookAt(target);
                AutoPilot = false;                
            }
        }

        if (tForm.position.z < -5.5f)
        {
//            GameController.ResetPosition(tForm);
            //Destroy(gameObject);
        }

    }
}
