using UnityEngine;
using System.Collections;

public class MuzzleAnimation : MonoBehaviour {

    private Transform tr;

	
	void Awake () 
    {
        tr = transform; 
	}
		
	void Update () 
    {   
	    tr.localScale = Vector3.one * Random.Range(0.5f,1.5f);
	    tr.localEulerAngles = new Vector3(0, 0, Random.Range(0f,90f));
	}
}
