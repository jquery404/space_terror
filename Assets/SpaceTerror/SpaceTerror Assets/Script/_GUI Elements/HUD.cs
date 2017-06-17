using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour {

    public enum HUDType
    {
        Score, Health, Level
    }    
    public enum HUDCats
    {
        Text, Texture
    }    
    public HUDType CurrentType = HUDType.Score;
    public HUDCats CurrentCats = HUDCats.Text;

    private GUITexture PlayerHealthBar;
    private float HealthBarWidth;
    private float HealthBarHeight;
    private float HealthBarX;
    private float HealthBarY;
    private float gBarWidth;        // current guitexture bar width
    
    void Start()
    {
        if (CurrentType == HUDType.Health && CurrentCats == HUDCats.Texture)
        {
            PlayerHealthBar = gameObject.GetComponent<GUITexture>();
            HealthBarWidth = PlayerHealthBar.pixelInset.width;
            HealthBarHeight = PlayerHealthBar.pixelInset.height;
            HealthBarX = PlayerHealthBar.pixelInset.x;
            HealthBarY = PlayerHealthBar.pixelInset.y;
        }

        InvokeRepeating("UpdateHUD", 1f, .5f);        
    }

    void UpdateHUD() 
    {
//        if (CurrentType == HUDType.Score)
//        {
//            if (CurrentCats == HUDCats.Text)
//            {
//                GetComponent<GUIText>().text = GameManager.PlayerScore.ToString();
//            }
//            else if (CurrentCats == HUDCats.Texture)
//            { 
//                
//            }
//        }
//        else if (CurrentType == HUDType.Health)
//        {
//            if (CurrentCats == HUDCats.Text)
//            {
//                GetComponent<GUIText>().text = GameManager.PlayerHealth.ToString();
//            }
//            else if (CurrentCats == HUDCats.Texture)
//            {
//                // % value of healthbar texture
//                if (GameManager.PlayerHealth >= 0)
//                {
//                    gBarWidth = HealthBarWidth * (GameManager.PlayerHealth / 100);
//                    PlayerHealthBar.pixelInset = new Rect(HealthBarX, HealthBarY, gBarWidth, HealthBarHeight);
//                }
//                else if (GameManager.PlayerHealth < 0 && PlayerHealthBar.pixelInset.width != 0)
//                {                    
//                    PlayerHealthBar.pixelInset = new Rect(HealthBarX, HealthBarY, 0, HealthBarHeight);
//                }
//            }            
//        }
//        else if (CurrentType == HUDType.Level)
//        {
//            GetComponent<GUIText>().text = "Level " + GameManager.CurrentLevel.ToString();
//        }
        
    }
}
