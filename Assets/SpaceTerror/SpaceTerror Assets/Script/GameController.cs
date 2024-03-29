using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

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

    public float[] SpawnPoints;
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
    
    
    
    //private Dictionary<EnemyTypes, GameObject> Enemies = new Dictionary<EnemyTypes, GameObject>(3);    
    //private Dictionary<EnemyTypes, UnitBudget> UnitBudgets = new Dictionary<EnemyTypes, UnitBudget>(3);
    
    private List<int> HitList = new List<int>();
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
        Units[0] = new UnitBudget();
        Units[0].weights = 0.5f;
        Units[0].points = 1f;

        Units[1] = new UnitBudget();
        Units[1].weights = 0.3f;
        Units[1].points = 3f;

        Units[2] = new UnitBudget();
        Units[2].weights = 0.2f;
        Units[2].points = 5f;

        Units[3] = new UnitBudget();
        Units[3].weights = 0.15f;
        Units[3].points = 8f;

        Units[4] = new UnitBudget();
        Units[4].weights = 0.1f;
        Units[4].points = 9f;

        Units[5] = new UnitBudget();
        Units[5].weights = 0.05f;
        Units[5].points = 10f;


        UnitBudgets.Add(0, Units[0]);
        UnitBudgets.Add(1, Units[1]);
        UnitBudgets.Add(2, Units[2]);
        UnitBudgets.Add(3, Units[3]);
        UnitBudgets.Add(4, Units[4]);
        UnitBudgets.Add(5, Units[5]);

        SpawnEnemies = false;

        // Create an Empty Gameobject to hold all enemies reference
        GameObject EnemyHQobj= new GameObject("EnemyBaseHQ");
        EnemyBaseHQ = EnemyHQobj.transform;


        //Debug.Log(UnitBudgets[EnemyTypes.MiniCraft].weights);
        //Debug.Log(UnitBudgets[0].weights);      

    }


    void Start()
    {
        GameStartTimer = Time.time;

        //Enemies.Add(EnemyTypes.Asteroid, AsteroidEnemy);
        //Enemies.Add(EnemyTypes.MiniCraft, MiniCraftEnemy);
        //Enemies.Add(EnemyTypes.Banzai, BanzaiEnemy);

        Enemies.Add(0, Asteroid);
        Enemies.Add(1, MiniCraft);
        Enemies.Add(2, Banzai);
        Enemies.Add(3, FooFighter);
        Enemies.Add(4, DoggyTrail);
        Enemies.Add(5, MotherShip);

        StartCoroutine(CreateEnemiesWave());
        //InvokeRepeating("", 1.0f, 5.0f);
    }


    void Update()
    {
       
    }

    void SpawnManager(float threshold, float inc)
    {
        float CurrentBudget = LevelBudget(threshold, inc);
        Debug.Log("Budget : " + CurrentBudget);
        
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
            
            //Debug.Log(" - " + UnitBudgets[CloseNumber].points);
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
                if (GameManager.CurrentLevel == 2)
                {
                    HiddenUnit--;
                }
                else if (GameManager.CurrentLevel == 3)
                {
                    HiddenUnit--;
                }
                else if (GameManager.CurrentLevel == 4)
                {
                    HiddenUnit--;
                }
                else if (GameManager.CurrentLevel == 5)
                {
                    HiddenUnit--;
                }


                SpawnManager(3, 5);
                int Total = HitList.Count;                

                if (CurrentSpawnType == SpawnType.Random)
                {
                    for (int i = 0; i < Total; i++)
                    {
                        Vector3 Position = new Vector3(transform.position.x + SpawnPoints[Random.Range(0, SpawnPoints.Length)], transform.position.y, 6.5f);
                        //Instantiate(Enemies[HitList[i]], Position, Quaternion.identity);
                        //Instantiate(Enemies[HitList[i]], Position, Quaternion.identity);
                        GameObject newEnemy = Instantiate(Enemies[HitList[i]], Position, Quaternion.identity) as GameObject;
                        newEnemy.transform.parent = EnemyBaseHQ;
                        GameManager.TotalEnemies++;
                    }
                }
                else if (CurrentSpawnType == SpawnType.Assemble)
                {
                    for (int i = 0; i < Total; i++)
                    {
                        Vector3 Position = new Vector3(transform.position.x + SpawnPoints[2] + 2 * i, transform.position.y, 6.5f);
                        Instantiate(Enemies[HitList[i]], Position, Quaternion.identity);
                    }

                }
                else if (CurrentSpawnType == SpawnType.Parade)
                {

                    for (int i = 0; i < Total; i++)
                    {
                        if (i % 2 == 0)
                        {
                            Vector3 Position = new Vector3(transform.position.x + SpawnPoints[2], transform.position.y, 6.5f);
                            Instantiate(Enemies[HitList[i]], Position, Quaternion.identity);
                        }
                        else
                        {
                            Vector3 Position = new Vector3(transform.position.x + SpawnPoints[2] + 1, transform.position.y, 6.5f);
                            Instantiate(Enemies[HitList[i]], Position, Quaternion.identity);
                            yield return new WaitForSeconds(1f);
                        }
                    }

                }
                else if (CurrentSpawnType == SpawnType.Pyramid)
                {
                    int j = Total;

                    for (int i = 1; i <= Total; i++)
                    {
                        currentOffset = SpawnPoints[0];

                        for (int k = 1; k <= j; k++)
                        {
                            currentOffset += 1f;
                        }

                        for (int x = 1; x <= i; x++)
                        {
                            currentOffset += 1f;
                            Vector3 Position = new Vector3(currentOffset, transform.position.y, 6.5f);
                            Instantiate(Enemies[HitList[i]], Position, Quaternion.identity);
                            currentOffset += 1f;
                        }
                        j--;
                        yield return new WaitForSeconds(1f);
                    }
                }
                Debug.Log(GameManager.TotalEnemies);
                SpawnEnemies = false;
                // clear hitlist 
                HitList.Clear();
            }           
            #endregion

            // check if any enemy dead
            EnemyDead();
            

            yield return new WaitForSeconds(5.0f);
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

    // number of active enemies. if no enemies active then send new enemy spawn request
    void EnemyDead()
    {
        int enemyCount = 0;
        int totalEnemy = EnemyBaseHQ.childCount;
        for (int i = 0; i < totalEnemy; i++)
        {
            if (EnemyBaseHQ.GetChild(i).gameObject.activeSelf)
            {
                enemyCount++;            
            }
        }
        
        // if there is no enemy then its time to spawn enemy
        if (enemyCount == 0)
        {
            SpawnEnemies = true;
            Debug.Log(" -spawn- ");
        }
        Debug.Log(enemyCount+" - "+totalEnemy);

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
        
        ScoreGUI.SetActiveRecursively(false);
        HealthGUI.SetActiveRecursively(false);

        GameOverGUI.transform.Find("FinalScore").GetComponent<Text>().text = GameManager.PlayerScore.ToString();
        GameOverGUI.transform.Find("EnemiesKilled").GetComponent<Text>().text = GameManager.TotalKill.ToString();
        int Accuracy = (int)Mathf.Floor((GameManager.TotalKill/GameManager.TotalEnemies)*100);       
        GameOverGUI.transform.Find("Accuracy").GetComponent<Text>().text = Accuracy.ToString();
       
    }
    
    
    // Restart The GamePlay
    public void RestartGamePlay()
    {              
        // score and health value reset
        GameManager.PlayerScore = 0;
        GameManager.TotalKill = 0;
        GameManager.PlayerHealth = 100;
        // turn back score and health gui
        ScoreGUI.SetActiveRecursively(true);
        HealthGUI.SetActiveRecursively(true);
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
        Player.SetActiveRecursively(true);
        Player.transform.Find("Idle").gameObject.SetActiveRecursively(false);
    }
}
