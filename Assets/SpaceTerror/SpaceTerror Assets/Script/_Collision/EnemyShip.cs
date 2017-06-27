using UnityEngine;
using System.Collections;

public class EnemyShip : TroopBudget, IEnemyShip {

	public float shipWeight;
	public float shipCost;
	public float Points; 		// point if u kill me 
	public float damageAmount; 	// damage to player
	public float Health; 		// ship health

    public GameObject PointGUI;
    public GameObject ExplosionPrefab;
    public GameObject[] powerObj;
        
    private Transform tr;    

    void Awake() {
        tr = transform;
		this.weights = shipWeight;
		this.cost = shipCost;
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


	public void Fire(){

	}

	public void DoDamage(){

	}

	public void GotHit(int damage){
		this.Health -= damage;
	}

	public void DropPower()
    {
        float roll = Random.Range(0f, 10f);

        if (roll < 1)
        {
            PoolManager.instance.ReuseObject(powerObj[0], tr.position, Quaternion.identity);
        }
    }

	void OnTriggerEnter(Collider col)
	{

	}
}
