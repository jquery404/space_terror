using UnityEngine;
using System.Collections;

public class GameStatsButton : MonoBehaviour
{

    public enum ButtonType 
    { 
        PlayAgain, BackToMain, Submit
    }
    public ButtonType CurrentBtn = ButtonType.PlayAgain;
    public Material[] FontMats;

    private Transform tr;

	void Start () 
    {
        tr = transform;	
	}

    void OnMouseDown()
    {
        tr.GetComponent<GUIText>().material = FontMats[1];
    }

    void OnMouseUp()
    {
        tr.GetComponent<GUIText>().material = FontMats[0];
        
        tr.parent.gameObject.SetActiveRecursively(false);

        if (CurrentBtn == ButtonType.PlayAgain)
        {
            // if user click on PlayAgain Button
            Transform HQ = GameObject.Find("EnemyBaseHQ").transform;
//            GameManager.DisableActiveChildren(HQ);
            GameController GC = (GameController)GameObject.FindObjectOfType(typeof(GameController));
//            GC.RestartGamePlay();
            
        
        }
        else if (CurrentBtn == ButtonType.BackToMain)
        {
            // if user click on BackToMain Button
        
        }
        else if (CurrentBtn == ButtonType.Submit)
        {
            // if user click on Submit Button
        
        }


    }

    
}
