using UnityEngine;
using System.Collections;

public class LaserMovement : MonoBehaviour {

    public float fireSpeed;

    private Transform tr;

    void Start()
    {
        tr = transform;
    }
	
	
	
	void Update () {
        tr.Translate(Vector3.forward * fireSpeed * Time.deltaTime);

        if (tr.position.z > 6f) {
            Destroy(gameObject);
        }
	}

    
}
