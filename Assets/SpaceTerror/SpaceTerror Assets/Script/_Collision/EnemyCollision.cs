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

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Laser" && Health > 0)
        {
            Destroy(col.gameObject);
            Health -= 5;
        }
        else if (col.gameObject.tag == "Laser" && Health <= 0)
        {            
            Destroy(col.gameObject);            
            GameManager.PlayerScore += Points;                        
            Vector3 viewportPos = Camera.main.WorldToViewportPoint(tr.position);
            Instantiate(PointGUI, new Vector3(viewportPos.x, viewportPos.y, 0), Quaternion.identity);
            Instantiate(ExplosionPrefab, tr.position, Quaternion.identity);
            DropPower();
            //Destroy(gameObject);
            gameObject.SetActiveRecursively(false);
            GameManager.TotalKill++;        // no of enemy killed
        }        
	}

    void DropPower()
    {
        float roll = Random.Range(0f, 10f);

        if (roll < 1)
        {
            Instantiate(powerObj[0], tr.position, Quaternion.identity);
        }
    }
}
