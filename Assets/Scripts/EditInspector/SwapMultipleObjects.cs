using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SwapMultipleObjects : MonoBehaviour
{
    [Serializable]
    public struct ObjectsToSwap
    {
        public string initObject;
        public GameObject newObject;
    }
    public List<ObjectsToSwap> peerObjects;
    
    private Dictionary<string, GameObject> objectsDic = new Dictionary<string, GameObject>();

    public void SwapObjects() {

        foreach (ObjectsToSwap objects in peerObjects) {
            objectsDic.Add(objects.initObject, objects.newObject);
        }

        foreach (Transform child in transform) {

            if (objectsDic.ContainsKey(child.name)) {
                Vector3 pos = child.transform.position;
                Quaternion rot = child.transform.rotation;
                Vector3 scale = child.transform.localScale;

                child.gameObject.SetActive(false);
                GameObject newObj = Instantiate(objectsDic[child.name]);
                //newObj.transform.SetParent(transform);
                newObj.transform.position = pos;
                newObj.transform.rotation = rot;
                newObj.transform.localScale = scale;
            }
            else {
                Debug.LogError("error");
            }
        }

    }
}
