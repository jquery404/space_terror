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
        
    }

	void OnTriggerEnter(Collider col)
	{

	}
}
