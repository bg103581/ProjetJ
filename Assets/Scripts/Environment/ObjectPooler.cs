using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ObjectPooler : MonoBehaviour
{
    [HideInInspector] public static ObjectPooler current;
    [SerializeField] private GameObject pooledObject;
    [SerializeField] private int pooledAmount = 20;
    [SerializeField] private bool willGrow = true;

    public List<GameObject> pooledObjects;

    private void Awake() {
        current = this;
    }

    void Start()
    {
        pooledObjects = new List<GameObject>();

        for (int i = 0; i < pooledAmount; i++) {
            GameObject obj = Instantiate(pooledObject);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
    }

    public GameObject InstantiateObject(Vector3 position, Quaternion rotation, Transform parent) {
        GameObject obj = GetPooledObject();
        if (obj == null) {
            Debug.LogError("pooled object is null");
            return null;
        }

        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.transform.SetParent(parent);
        obj.SetActive(true);

        Disc disc = obj.GetComponent<Disc>();
        disc.isLastOvniDisc = false;
        disc.isOvniDisc = false;

        return obj;
    }

    public void DestroyObject(GameObject gameObject) {
        if (gameObject.tag == "Gold") {
            gameObject.transform.DOKill();
            gameObject.SetActive(false);
        }
        else {
            Destroy(gameObject);
        }
    }

    private GameObject GetPooledObject() {
        for (int i = 0; i < pooledObjects.Count; i++) {
            if (!pooledObjects[i].activeInHierarchy) return pooledObjects[i];
        }

        if (willGrow) {
            GameObject obj = Instantiate(pooledObject);
            obj.SetActive(false);
            pooledObjects.Add(obj);
            return obj;
        }

        return null;
    }
}
