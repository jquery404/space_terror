using UnityEngine;
using System.Collections;

public class EnemyCollision : MonoBehaviour {

    public float Points;
    public GameObject PointGUI;
    public GameObject ExplosionPrefab;
    public float Health;
    public GameObject[] powerObj;
        
    private Transform tr;    

    void Awake() {
        tr = transform;
    }


    void Start()
    {
        PoolManager.instance.CreatePool(PointGUI, 5);
        PoolManager.instance.CreatePool(ExplosionPrefab, 5);

        for (int i = 0; i < powerObj.Length; i++)
        {
            PoolManager.instance.CreatePool(powerObj[i], 2);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Laser" && Health > 0)
        {
            col.gameObject.GetComponent<LaserMovement>().DestroyNow();
            Health -= 5;
        }
        else if (col.gameObject.tag == "Laser" && Health <= 0)
        {
            col.gameObject.GetComponent<LaserMovement>().DestroyNow();
            GameManager.PlayerScore += Points;                        
            Vector3 viewportPos = Camera.main.WorldToViewportPoint(tr.position);

            PoolManager.instance.ReuseObject(PointGUI, new Vector3(viewportPos.x, viewportPos.y, 0), Quaternion.identity);
            PoolManager.instance.ReuseObject(ExplosionPrefab, tr.position, Quaternion.identity);

            DropPower();
            
            gameObject.SetActive(false);
            GameManager.TotalKill++;        // no of enemy killed
        }        
	}

    void DropPower()
    {
        float roll = Random.Range(0f, 10f);

        if (roll < 1)
        {
            PoolManager.instance.ReuseObject(powerObj[0], tr.position, Quaternion.identity);
        }
    }
}
