using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {

    private Transform tForm;
    private string objType;
    private GameController other;
    private Transform Target;
    private float initPos;

	void Awake () {
        tForm = transform;
        objType = tForm.name;
        other = gameObject.GetComponent<GameController>();

	}

    void Start() {
        if (objType == "BadA") 
        {
            Debug.Log("A");
            Target = GameObject.FindWithTag("Player").transform;
        }
        else if (objType == "BadB")
        {

        }
        else if (objType == "BadC")
        {

        }
        initPos = tForm.position.x;
    }


	void Update () {

        if (objType == "BadA")
        {
            tForm.Translate(Vector3.forward * 5f * Time.deltaTime);
            //LookAtOnce(tForm, Target);
            
        }
        
        
	}



    public void LookAtOnce(Transform source, Transform target)
    {
        source.LookAt(target);
    }


    void PingPongMove(float initPos, float length, float wiggle)
    {
        float x = Mathf.PingPong(Time.time * wiggle, length) + initPos;
        float y = transform.position.y;
        float z = transform.position.z;

        transform.position = new Vector3(x, y, z);
    }

}
