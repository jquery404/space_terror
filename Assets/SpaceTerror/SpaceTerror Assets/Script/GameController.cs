using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitBudget
{
    public float weights { get; set; }
    public float points { get; set; }

}

public class GameController : MonoBehaviour
{

    public enum SpawnType
    {
        Random, Assemble, Parade, Pyramid
    }
    public enum EnemyType
    {
        Asteroid, MiniCraft, Banzai, FooFighter, DoggyTrail, MotherShip
    }

    public EnemyType CurrentEnemyType = EnemyType.Asteroid;
    public SpawnType CurrentSpawnType = SpawnType.Random;
    public float CurrentSpawnRate = 1f;
    

    public GameObject Asteroid;
    public GameObject MiniCraft;
    public GameObject Banzai;
    public GameObject DoggyTrail;
    public GameObject FooFighter;
    public GameObject MotherShip;

    public Transform[] SpawnPoints;
    public GameObject GameOverGUI;
    public Vector3 playerDefaultPos;

    // privates variable
    private Transform EnemyBaseHQ;
    private bool SpawnEnemies;
    private float GameStartTimer;
    private float currentOffset;    
    private float HiddenUnit = 4;

    // cache variable
    private GameObject ScoreGUI;
    private GameObject HealthGUI;
    private GameObject Player;
    private Transform trPlayer;
  
    private List<int> HitList = new List<int> { 0 };
    private IEnumerator enemyWave;

    private UnitBudget[] Units = new UnitBudget[6];
    private Dictionary<int, GameObject> Enemies = new Dictionary<int, GameObject>(6);
    private Dictionary<int, UnitBudget> UnitBudgets = new Dictionary<int, UnitBudget>(6);    

    /*
     * n = level, t = time, D = damage, I = Increase rate, T = Threshold      
     * 
     * SpawnRate = 50 * n / t[n-1] - 30 * D[t-1]     
     * DamageRate = En - E(n-1) / n - (n-1)
     * LevelBudget = T + n * I
     * UnitBudgets[EnemyTypes.MiniCraft]
     * weight, points
     * 
     */
    

    void Awake()
    {        
        SpawnEnemies = false;
        // Create an Empty Gameobject to hold all enemies reference
        GameObject EnemyHQobj= new GameObject("EnemyBaseHQ");
        EnemyBaseHQ = EnemyHQobj.transform;
    }


    void Start()
    {
        GameStartTimer = Time.time;
        AddEnemies();
        enemyWave = CreateEnemiesWave();

        StartCoroutine(enemyWave);

        PoolManager.instance.CreatePool(Enemies[0], 20);                    // Asteroid
        //PoolManager.instance.CreatePool(Enemies[1], 20);                  // MiniCraft
    }


    void AddEnemies()
    {
        Enemies.Add(0, Asteroid);
        //Enemies.Add(1, MiniCraft);
       // Enemies.Add(2, Banzai);
        //Enemies.Add(3, FooFighter);
        //Enemies.Add(4, DoggyTrail);
        //Enemies.Add(5, MotherShip);
    }

    void SpawnManager(float threshold, float inc)
    {
        float CurrentBudget = LevelBudget(threshold, inc);        
        // purchased some units
        Checkout(CurrentBudget);
    }

    // T = threshold , I = increment
    private float LevelBudget(float T, float I)
    {
        return (T + GameManager.CurrentLevel * I);
    }

    private void Checkout(float CurrentBudget)
    {
        while (CurrentBudget > 0)
        {
            float totalWeight = 0;

            for (int i = 0; i < (int)(Units.Length - HiddenUnit); i++)
            {
                if (CurrentBudget >= UnitBudgets[i].points)
                {
                    totalWeight += UnitBudgets[i].weights;
                }
            }

            float PickNumber = Random.Range(0, totalWeight);
            int CloseNumber = ClosestNumber(PickNumber);

            if (CurrentBudget > UnitBudgets[CloseNumber].points)
            {
                // update ur budget after u purchased  
                CurrentBudget -= UnitBudgets[CloseNumber].points;
            }
            else
            {
                CurrentBudget = 0;
            }

            HitList.Add(CloseNumber);
        }

    }
    
    public int ClosestNumber(float picknum)
    {
        float CurrentCost = 0f;
        float infiniteMax = 400f;
        int index = 0;

        for (int i = 0; i < (int)(Units.Length - HiddenUnit); i++)
        {
            if (UnitBudgets[i].weights == picknum)
            {
                CurrentCost = picknum;
                index = i;
                return index;
            }
            else
            {
                float d = Mathf.Abs(picknum - UnitBudgets[i].weights);

                if (d < infiniteMax)
                {
                    CurrentCost = UnitBudgets[i].weights;
                    index = i;
                    infiniteMax = d;
                }
            }

        }

        return index;
    }

   
    IEnumerator CreateEnemiesWave()
    {
        yield return null;
        
        while (true)
        {
            
            #region SpawnTime
            // if total enemy 0 and player is not dead spawn more enemies
            if (SpawnEnemies && !GameManager.isDead)
            {
                GameManager.CurrentLevel++;

                if (CurrentSpawnType == SpawnType.Random)
                {
                    for (int i = 0; i < HitList.Count; i++)
                    {
                        PoolManager.instance.ReuseObject(Enemies[HitList[i]],
                            SpawnPoints[Random.Range(0, SpawnPoints.Length)].position,
                            Quaternion.identity);
                        GameManager.TotalEnemies++;
                    }
                }



            }           
            #endregion

            // check if any enemy dead
            EnemyDead();
            

            yield return new WaitForSeconds(5.0f);
            StopCoroutine(enemyWave);
        }


    }
    
    // increase / decrease enemy spawn rate
    void UpdateSpawnRate()
    {
        // Number of Enemies in Level n = (n * 50)/ complete time level[n-1] - 30 * damage;
        CurrentSpawnRate = CurrentSpawnRate * 3;
    }
    
    // update spawn types
    void UpdateSpawnType()
    {
        CurrentSpawnType = SpawnType.Pyramid;
    }
    
    // update enemy types
    void UpdateEnemyType()
    {
        CurrentEnemyType = EnemyType.MiniCraft;
    }

    public static void ResetPosition(Transform form)
    {
        float x = Random.Range(-4.5f, 4.5f);
        float y = 0f;
        float z = 6f;

        form.position = new Vector3(x, y, z);        
    }


    public Transform[] getSpawnPoints()
    {
        return SpawnPoints;
    }

    // number of active enemies. if no enemies active then send new enemy spawn request
    void EnemyDead()
    {
        int activeEnemy = 0;
        int totalEnemy = EnemyBaseHQ.childCount;
        for (int i = 0; i < totalEnemy; i++)
        {
            if (EnemyBaseHQ.GetChild(i).gameObject.activeSelf)
            {
                activeEnemy++;            
            }
        }
        
        // if there is no enemy then its time to spawn enemy
        if (activeEnemy == 0)
        {
            SpawnEnemies = true;
            Debug.Log(" -spawn- ");
        }
        Debug.Log("ActiveEnemy: "+ activeEnemy + " - TotalEnemy "+totalEnemy);

    }

    // if Player is dead then stop the game and show game stats GUI
    public void isGameOver()
    {

        if (Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
        }

        Player.SetActive(false);

        GameOverGUI.SetActive(true);

        if (!ScoreGUI && !HealthGUI)
        {
            ScoreGUI = GameObject.Find("Player Scorebar");
            HealthGUI = GameObject.Find("Player Healthbar");
        } 
        
        ScoreGUI.SetActive(false);
        HealthGUI.SetActive(false);

        GameOverGUI.transform.FindChild("FinalScore").GetComponent<GUIText>().text = GameManager.PlayerScore.ToString();
        GameOverGUI.transform.FindChild("EnemiesKilled").GetComponent<GUIText>().text = GameManager.TotalKill.ToString();
        int Accuracy = (int)Mathf.Floor((GameManager.TotalKill/GameManager.TotalEnemies)*100);       
        GameOverGUI.transform.FindChild("Accuracy").GetComponent<GUIText>().text = Accuracy.ToString();
       
    }
    
    
    // Restart The GamePlay
    public void RestartGamePlay()
    {              
        // score and health value reset
        GameManager.PlayerScore = 0;
        GameManager.TotalKill = 0;
        GameManager.PlayerHealth = 100;
        // turn back score and health gui
        ScoreGUI.SetActive(true);
        HealthGUI.SetActive(true);
        // reset GameManager isDead
        GameManager.isDead = false;
        // reset GameManage.CurrentLevel
        GameManager.CurrentLevel = 0;
        // reset HiddenUnit (default value 4)
        HiddenUnit = 4;
        // turn back our player in default position
        if (trPlayer == null)
        {
            trPlayer = Player.transform;
        }

        trPlayer.position = playerDefaultPos;
        Player.SetActive(true);
        Player.transform.FindChild("Idle").gameObject.SetActive(false);
    }
}
