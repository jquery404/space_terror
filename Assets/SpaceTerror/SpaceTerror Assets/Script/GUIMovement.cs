using UnityEngine;
using System.Collections;

public class GUIMovement : MonoBehaviour {

	public float guiSpeed;

    private Transform tForm;

	void Start () {
        tForm = transform;

        
        Invoke("KillGUI", 1);

	}
	
	void Update () {
        tForm.Translate(0, Time.deltaTime * guiSpeed,0);
	}

    void KillGUI() {
        Destroy(gameObject);
    }

}
