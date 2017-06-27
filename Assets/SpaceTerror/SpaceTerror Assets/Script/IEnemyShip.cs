using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyShip{
	
	void Fire ();
	void DoDamage ();
	void DropPower ();
	void GotHit (int damage);
}
