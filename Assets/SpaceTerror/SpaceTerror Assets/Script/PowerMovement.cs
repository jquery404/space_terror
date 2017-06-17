using UnityEngine;
using System.Collections;

public class PowerMovement : MonoBehaviour {

    public float moveSpeed; 

    private Transform tForm;
    
    void Awake()
    {
        tForm = transform;
        tForm.Rotate(Vector3.up * 180f);
    }
	
	void Update () {
        tForm.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        tForm.Rotate(Vector3.forward * Time.deltaTime * 180f, Space.World);

        if (tForm.position.z < -5.5f)
        {
            Destroy(gameObject);
        }
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
//            col.gameObject.GetComponent<PlayerMovement>().PowerUp();
            Destroy(gameObject);
        }
    }
}
