using UnityEngine;
using System.Collections;

public class Asteroid : PoolObject {


    public float minSpeed;
    public float maxSpeed;
    public float minRotation;
    public float maxRotation;
    public float minSize;
    public float maxSize;

    
    private Transform tr;
    private Vector3 curPos;
    private float rollingDice;
    private float size;
    private float speed;
    private float rotation;
    


    void Awake()
    {
        tr = transform;
        tr.Rotate(Vector3.up * 180f);
        rollingDice = Mathf.Floor(Random.Range(0, 10));        
    }

    void Start()
    {
        Refresh();
        tr.localScale = new Vector3(size, size, size);
    }


    void Refresh()
    {
        size = Random.Range(minSize, maxSize);
        speed = Random.Range(minSpeed, maxSpeed);
        rotation = Random.Range(minRotation, maxRotation);
    }
   
    void FixedUpdate()
    {
        tr.Translate(Vector3.back * speed * Time.deltaTime);
        tr.Rotate(Vector3.forward * rotation * Time.deltaTime);
        if (curPos.z - tr.position.z > 20f)
        {
            Debug.Log("reset");
            //this.DoDestroy(gameObject);
            this.OnObjectReuse();
        }
        
    }

    public override void OnObjectReuse()
    {
        base.OnObjectReuse();

        Refresh();        
    }
}
