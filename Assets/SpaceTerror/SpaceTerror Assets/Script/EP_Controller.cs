using UnityEngine;
using System.Collections;

public class EP_Controller : MonoBehaviour {

    public float fireSpeed;

    private Transform tForm;

    void Start()
    {
        tForm = transform;
    }
	
	
	void Update () {
        tForm.Translate(Vector3.forward * fireSpeed * Time.deltaTime);

        if (tForm.position.z < -5.5f)
        {
            //Destroy(gameObject);
        }
	}
}
