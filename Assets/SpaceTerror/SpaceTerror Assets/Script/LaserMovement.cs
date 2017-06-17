using UnityEngine;
using System.Collections;

public class LaserMovement : PoolObject{

    public float fireSpeed;
    private Transform tr;

    void Start()
    {
        tr = transform;
    }
	
	void Update ()
    {
        tr.Translate(Vector3.forward * fireSpeed * Time.deltaTime);

        if (tr.position.z > 6f)
        {
            this.DoDestroy(gameObject);
        }
	}

    public void DestroyNow()
    {
        this.DoDestroy(gameObject);
    }

    public override void OnObjectReuse()
    {
        base.OnObjectReuse();
    }

}
