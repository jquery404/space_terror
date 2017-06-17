using UnityEngine;
using System.Collections;

public class GameEnd : MonoBehaviour {

    public Texture2D win;
    public Texture2D lose;
    private GameObject screen;
    private Rect winInset;
    private Rect loseInset;

    void Awake()
    {
        screen = GameObject.Find("WinScreen");
        winInset = new Rect(-128, -64, 256, 128);
        loseInset = new Rect(-128, -128, 256, 256);
    }

	void Start () {
//        if (GameManager.isWin)
//        {
//            screen.GetComponent<GUITexture>().pixelInset = winInset;
//            screen.GetComponent<GUITexture>().texture = win;
//        }
//        else
//        {
//            screen.GetComponent<GUITexture>().pixelInset = loseInset;
//            screen.GetComponent<GUITexture>().texture = lose;
//        }
	}
	
	
	void Update () {
//        if (Input.touchCount > 0)   
//        {
//            Touch touch = Input.GetTouch(0);
//            if (touch.phase == TouchPhase.Ended)
//            {
//                // RESET FUNCTION SHOULD CALL
//                GameManager.PlayerScore= 0f;
//                GameManager.PlayerHealth = 100f;
//                Application.LoadLevel(1);
//            }
//        }
	}
}
