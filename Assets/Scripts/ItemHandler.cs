using TMPro;
using UnityEngine;

public class ItemHandler : MonoBehaviour
{
    [SerializeField] Counter item;
    [SerializeField] TextMeshPro txt;

    void Awake()
    {
        item.txt = txt;
        item.value = 0;
        item.objList.Clear();

        if (item.initValue != 0) //ignore washing and bin
        {
            for (int i = 1; i <= item.initValue; i++)
            {
                GameObject obj = Instantiate(item.obj, transform);
                obj.transform.localPosition = new Vector3(0, -0.4f + 0.2f*item.value, 0);
                obj.transform.localScale = new Vector3(obj.transform.localScale.x*0.9f, obj.transform.localScale.y, obj.transform.localScale.z*0.9f);
                item.objList.Add(obj);
                item.value++;
            }
        }
    }
}
