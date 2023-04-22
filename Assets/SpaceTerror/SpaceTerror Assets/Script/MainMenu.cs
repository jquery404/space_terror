using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

    public bool isQuit;

	void Start () {
       GetComponent<Renderer>().material.color = new Color(0.305f, 0.664f, 0.957f, 1.0f);
	}

    void OnMouseDown() {
        
        if (isQuit)
        {
            Application.Quit();
        }
        else 
        {            
            Application.LoadLevel(1);
        }
        
    }
}
