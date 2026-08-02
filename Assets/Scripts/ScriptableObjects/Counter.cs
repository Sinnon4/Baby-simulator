using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "Counter")]
public class Counter : ScriptableObject
{
    public float value, initValue, maxValue;
    public GameObject obj;
    public List<GameObject> objList = new();
    public TextMeshPro txt;
    
    public GameObject take(GameObject go, Transform t, Vector3 pos) //,v3 scale?
    {
        if (go.name == go.tag) //only true for the exisiting objects not instantiated prefabs
            if (value > 0)
            {
                value--;
                txt.text = $"{value}";
                
                GameObject obj = objList[objList.Count-1];

                obj.transform.SetParent(t);
                obj.transform.localPosition = pos;
                if (go.tag == "Clothes") obj.transform.localScale = new Vector3(1,1,1)*0.15f;

                objList.Remove(obj);
                return obj;
            }
            else return null;
        else //picking up off ground
        {
            go.GetComponent<Rigidbody>().isKinematic = true;
            go.GetComponent<BoxCollider>().isTrigger = true;
            go.transform.SetParent(t);
            go.transform.localPosition = pos;
            
            return go;
        }
    }

    public bool deposit(Transform t)
    {
        if (value < maxValue)
        {
            value++;
            txt.text = $"{value}";
            
            t.SetParent(t);
            t.localPosition = Vector3.zero;

            objList.Add(t.gameObject);
            return true;
        }
        else return false;
    }
}
