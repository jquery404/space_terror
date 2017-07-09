﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour {

    Dictionary<int, Queue<ObjectInstance>> poolDict = new Dictionary<int, Queue<ObjectInstance>>();

    static PoolManager _instance;

    public static PoolManager instance {
        get{
            if (_instance == null)
                _instance = FindObjectOfType<PoolManager>();

            return _instance;
        }       
    }

	public void CreatePool (GameObject prefab, int poolSize)
    {
        int poolKey = prefab.GetInstanceID();

        GameObject poolHolder = new GameObject(prefab.name + " pool");
        poolHolder.transform.parent = transform;

        if(!poolDict.ContainsKey(poolKey))
        {
            poolDict.Add(poolKey, new Queue<ObjectInstance>());

            for (int i = 0; i < poolSize; i++)
            {
                ObjectInstance newObject = new ObjectInstance (Instantiate(prefab) as GameObject);
                poolDict[poolKey].Enqueue(newObject);
                newObject.SetParent(poolHolder.transform);
            }
        }
    }

    public void ReuseObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        int poolKey = prefab.GetInstanceID();

        if (poolDict.ContainsKey(poolKey))
        {
            ObjectInstance objectToReuse = poolDict[poolKey].Dequeue();
            poolDict[poolKey].Enqueue(objectToReuse);
            objectToReuse.Reuse(position, rotation);

			return objectToReuse.getObject();
        }
    }

    public void DestroyObject(GameObject prefab)
    {
        int poolKey = prefab.GetInstanceID();

        if (poolDict.ContainsKey(poolKey))
        {
            ObjectInstance objectToReuse = poolDict[poolKey].Dequeue();
            objectToReuse.Destroy();
        }
    }



    public class ObjectInstance
    {
        GameObject gameObject;
        Transform transform;

        bool hasPoolObjectComponent;
        PoolObject poolObjectScript;

        public ObjectInstance (GameObject objectInstance)
        {
            gameObject = objectInstance;
            transform = gameObject.transform;
            gameObject.SetActive(false);

            if(gameObject.GetComponent<PoolObject>())
            {
                hasPoolObjectComponent = true;
                poolObjectScript = gameObject.GetComponent<PoolObject>();
            }

        }

		public GameObject getObject(){
			return this.gameObject;
		}

        public void Reuse(Vector3 position, Quaternion rotation)
        {
            if (hasPoolObjectComponent)
                poolObjectScript.OnObjectReuse();

            gameObject.SetActive(true);
            transform.position = position;
            transform.rotation = rotation;
        }
        public void Destroy()
        {
            if (hasPoolObjectComponent)
                poolObjectScript.DoDestroy(gameObject);
        }

        public void SetParent(Transform parent)
        {
            transform.parent = parent;
        }
    }

}
