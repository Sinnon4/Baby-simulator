using TMPro;
using UnityEngine;

public class WipesHandler : MonoBehaviour
{
    [SerializeField] Counter item;

    void Awake()
    {
        item.txt = GetComponentInChildren<TextMeshPro>();
        item.value = 0;
        item.objList.Clear();

        for (int i = 1; i <= item.maxValue; i++)
        {
            GameObject obj = Instantiate(item.obj, transform);
            obj.transform.localPosition = new Vector3(0, -0.45f + 0.1f*item.value, 0);
            item.objList.Add(obj);
            item.value++;
        }
    }
}
