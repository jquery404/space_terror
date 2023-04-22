using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour {


    public float moveSpeed;
    public float rotateSpeed;
    public float minSize;
    public float maxSize;

    public static float weights;
    public static float points;

    
    private Transform tr;
    private float rollingDice;    
    
    
    void Awake()
    {
        tr = transform;
        tr.Rotate(Vector3.up * 180f);
        rollingDice = Mathf.Floor(Random.Range(0, 10));        
    }

    void Start()
    {
        float curScaleX = Random.Range(minSize, maxSize);
        float curScaleY = Random.Range(minSize, maxSize);
        float curScaleZ = Random.Range(minSize, maxSize);

        tr.localScale = new Vector3(curScaleX, curScaleY, curScaleZ);
    }


    void Update()
    {
        tr.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        tr.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);

        if (tr.position.z < -5.5f)
        {
            GameController.ResetPosition(tr);
            //Destroy(gameObject);
        }
    }
}
