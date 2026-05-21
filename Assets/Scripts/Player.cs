using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] GameObject baby, bassinet;
    public bool inRange_baby, hasBaby, inRange_bassinet;
[SerializeField] GameObject actionDial;
[SerializeField] TextMeshProGUI UItext;

    void Awake()
    {
actionDial.SstActive(false);
        resetBabyPos();
    }

    void Update()
    {

if (Input.GetKeyDown(Keycode.Mouse1))
{
actionDial.SetActive(true);
}

RaycastHit hit;
if (Physics.Raycast(transform.position, Vector3.forward*4, out hit, Color.red)
{
if (!hasBaby && hit.transform.tag == "baby") { showText("Pick up baby"); }
else if (hasBaby && hit.transform.tag == "Bassinet")
{ showText("Put down baby"); }
}
else showText("");


        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
RaycastHit hit;
Debug.DrawLine(transform.position, Vector3.forward*4, Color.red);
//if (!hasBaby && Physics.SphereCast(baby.transform.position, 5, transform.forward, out hit, 4, layerMask playerLayer)
if (!hasBaby && Physics.Raycast(transform.position, Vector3.forward, out hit, 4, layerMask babyLayer) //repeat for each station
            //if (inRange_baby && !hasBaby)
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

void showText(string msg) {
UItext.text = msg
}

public void checkOnBaby()
{
baby.transform.localPosition.LeanMoveY(5,3);
LeanTween(baby.transform.localEulerAngles.z, 0, 3);
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

void OnDrawGizmos() {
Gizmos.color = Color.red;
Gizmos.DrawWireSphere(baby.transform.position, 5)
Gizmos.DrawLine(baby.transform.position, baby.transform.position + Vector3.forward*4);
}

}
