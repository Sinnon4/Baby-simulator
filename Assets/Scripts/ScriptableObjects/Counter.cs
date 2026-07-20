using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "Counter")]
public class Counter : ScriptableObject
{
    public float value;
    public float maxValue;
    public GameObject obj;
    public List<GameObject> objList = new();
    public TextMeshPro txt;

    public GameObject take(GameObject go, Transform t, Vector3 pos)
    {
        if (go.TryGetComponent<BoxCollider>(out BoxCollider bc) && bc.isTrigger) //NEED TO FIGURE OUT BETTER TEST
            if (value > 0)
            {
                value--;
                txt.text = $"{value}";
                
                GameObject obj = objList[objList.Count-1];

                obj.transform.SetParent(t);
                obj.transform.localPosition = pos;
                //set size?

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
}
