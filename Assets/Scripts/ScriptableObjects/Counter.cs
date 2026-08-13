using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "Counter")]
public class Counter : ScriptableObject
{
    public int value, initValue, maxValue, withdrawValue;
    public GameObject obj;
    public bool setSize;
    public Vector3 scale;
    public bool holdObjects;
    public List<GameObject> objList = new();
    public TextMeshPro txt;
    
    public GameObject take(GameObject go, Transform t, Vector3 pos)
    {
        if (go.name == go.tag) //only true for the exisiting objects not instantiated prefabs
            if (value >= withdrawValue)
            {
                value -= withdrawValue;
                txt.text = $"{value}";
                
                GameObject obj_;
                if (holdObjects) obj_ = objList[objList.Count-1];
                else obj_ = Instantiate(obj);

                obj_.transform.SetParent(t);
                obj_.transform.localPosition = pos;
                if (setSize) obj_.transform.localScale = scale;

                if (holdObjects) objList.Remove(obj_);

                return obj_;
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

    public bool deposit(int depos_n = 0, Transform t = null)
    {
        if (value < maxValue)
        {
            if (holdObjects)
            {
                for (int i = value; i <= math.min(value+depos_n, maxValue-1); i++)
                {
                    GameObject obj_ = Instantiate(obj, t);
                    obj_.transform.localPosition = new Vector3(0, -0.4f + 0.2f*value, 0);
                    obj_.transform.localScale = new Vector3(obj_.transform.localScale.x*0.9f, obj_.transform.localScale.y, obj_.transform.localScale.z*0.9f);
                    objList.Add(obj_);
                    value++;
                    Debug.Log(value);
                }
            }
            else
            {
                value++;
                if (value == maxValue) txt.text = "MAX";
                else txt.text = $"{value}";
            }

            return true;
        }
        else return false;
    }
}
