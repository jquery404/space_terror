using UnityEngine;
using System.Collections;

public class MotherShip : MonoBehaviour {

	/* MotherShip Move slowly and Track repeateadly */

    public GameObject Projectile;
    public float moveSpeed;
    public float fireRate;
        
    private Transform tForm;
    private float rollingDice;
    private float nextFire = 0;
    private bool AutoPilot = true;
    private bool locked = false;
    private Transform target;
    private Transform child;

    void Awake()
    {
        tForm = transform;
        target = GameObject.FindWithTag("Player").transform;
        child = tForm.Find("Gunner").transform;
    }

	void Update () {
        if (!locked)
        {
            tForm.Rotate(Vector3.up * 180f);
            locked = true;
        }
        if (AutoPilot)
        {
            Quaternion targetRotation = Quaternion.LookRotation(target.position - tForm.position, Vector3.up);
            child.rotation = Quaternion.Slerp(child.rotation, targetRotation, Time.deltaTime * 1.2f);

            tForm.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
        FireProjectile(child);

        if (tForm.position.z < -5.5f)
        {
            GameController.ResetPosition(tForm);
        }
	}

    void FireProjectile(Transform trans)
    {
        if (Time.time > nextFire)
        {
            Instantiate(Projectile, trans.position, trans.rotation);
            nextFire = Time.time + fireRate;
        }
    }
}
