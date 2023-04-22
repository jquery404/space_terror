using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static int CurrentLevel = 0;
    public static float TotalEnemies = 0;
    public static float TotalKill = 0;
    public static int PowerState = 1;
    public static int LaserState = 1;

    public static float PlayerScore = 0;
    public static float PlayerHealth = 100f;    

    public static bool isWin;
    public static bool isDead;


    #region Player Information
    // get and set player information i.e. score, level, enemies killed, total enemies
    public static void SetPlayerInfo(string target, int val)
    {
        PlayerPrefs.SetInt(target, val);
    }
    
    public static void GetPlayerInfo(string target)
    {
        print(PlayerPrefs.GetInt(target));
    }

    #endregion

    #region Color Blinking
    // add some blinking effect with its material color 
    // require param is transform
    public static IEnumerator ColorBlinking(Transform tr, Color color)
    {   
        float val = 0.25f;
        tr.GetComponent<Renderer>().material.color = new Color(color.r - val, color.g, color.b, color.a);
        yield return new WaitForSeconds(0.1f);
        tr.GetComponent<Renderer>().material.color = new Color(color.r + val, color.g, color.b, color.a);
        yield return new WaitForSeconds(0.1f);
        tr.GetComponent<Renderer>().material.color = new Color(color.r - val, color.g, color.b, color.a);
        yield return new WaitForSeconds(0.1f);
        tr.GetComponent<Renderer>().material.color = new Color(color.r + val, color.g, color.b, color.a);
    }
    #endregion

    #region Deactivate All Active Child Object
    public static void DisableActiveChildren(Transform tr)
    {

        for (int i = 0; i < tr.childCount; i++)
        {
            if (tr.GetChild(i).gameObject.activeSelf)
            {
                tr.GetChild(i).gameObject.SetActiveRecursively(false);
            }
        }
     
    }
    #endregion
}
