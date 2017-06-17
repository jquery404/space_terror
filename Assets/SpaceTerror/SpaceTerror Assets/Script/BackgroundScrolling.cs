using UnityEngine;
using System.Collections;

public class BackgroundScrolling : MonoBehaviour {

    public float scrollSpeed;
    private float offset;
    

	void Start () {
	
	}
	

	void Update () {
        offset = Time.time * scrollSpeed;
        GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(0, offset));
	}
}
