using UnityEngine;
using System.Collections;

public class PowerupCollision : MonoBehaviour {

	
	void Start () {
	
	}
	
	
	void OnTriggerEnter (Collider col) {
        if (col.gameObject.name == "Player")
        {
            Destroy(gameObject);
        }
	}
}
