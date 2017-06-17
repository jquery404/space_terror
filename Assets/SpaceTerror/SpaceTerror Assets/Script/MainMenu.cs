using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

    public bool isQuit;

	void Start () {
       GetComponent<Renderer>().material.color = Color.magenta;
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
