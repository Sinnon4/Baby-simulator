using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] GameObject baby, bassinet;
    public bool inRange_baby, hasBaby, inRange_bassinet;

    void Awake()
    {
        resetBabyPos();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (inRange_baby && !hasBaby)
            {
                hasBaby = true;
                inRange_baby = false;
                baby.transform.SetParent(transform);
                baby.transform.localPosition = new Vector3(0,0.4f,1.2f);
                baby.transform.localEulerAngles = Vector3.forward * 90;
            }
            else if (inRange_bassinet && hasBaby)
            {
                hasBaby = false;
                inRange_bassinet = false;
                resetBabyPos();
            }
        }
    }

    void resetBabyPos()
    {
        baby.transform.SetParent(null);
        baby.transform.position = bassinet.transform.position + Vector3.up * 0.8f;
        baby.transform.eulerAngles = bassinet.transform.eulerAngles + Vector3.forward * 90;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!hasBaby && other.tag == "Baby") inRange_baby = true;
        else if (hasBaby && other.tag == "Bassinet") inRange_bassinet = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (!hasBaby && other.tag == "Baby") inRange_baby = false;
        else if (hasBaby && other.tag == "Bassinet") inRange_bassinet = false;
    }
}
