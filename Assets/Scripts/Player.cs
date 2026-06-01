using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Transform baby, babyParent, bassinet, changeTable;
    //Baby baby_;
    bool hasBaby, checking;
    [SerializeField] float contactDistance = 2;
    //[SerializeField] GameObject actionDial;
    [SerializeField] TextMeshProUGUI UI_text;
    

    void Awake()
    {
        //actionDial.SetActive(false);
        resetBabyPos(bassinet);
    }

    void Update()
    {

        if (hasBaby && Input.GetKeyDown(KeyCode.Mouse2))
        {
            //actionDial.SetActive(true);
            checkOnBaby();
        }

        if (!checking && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, contactDistance))
        {
            if (!hasBaby && hit.collider.CompareTag("Baby"))
            {
                showText("Pick up baby");
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    hasBaby = true;
                    // baby.transform.SetParent(Camera.main.transform);
                    resetBabyPos(Camera.main.transform);
                    showText("");
                }
            }
            else if (hasBaby && hit.collider.CompareTag("Bassinet"))
            {
                showText("Put down baby");
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    hasBaby = false;
                    resetBabyPos(bassinet);
                    showText("");
                }
            }
            else if (hasBaby && hit.collider.CompareTag("Change table"))
            {
                showText("Put down baby");
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    hasBaby = false;
                    resetBabyPos(changeTable);
                    showText("");
                }
            }
            else showText("");
        }
        else if (checking && Input.GetKeyDown(KeyCode.Mouse2))
        {
            resetBabyPos(Camera.main.transform);
        }
        else showText("");
    }

    void resetBabyPos(Transform parent)
    {
        babyParent = parent;
        baby.eulerAngles = babyParent.eulerAngles + Vector3.forward * 90;
        if (hasBaby)
        {
            baby.SetParent(Camera.main.transform);
            baby.localPosition = new Vector3(0, -0.4f, 0.6f);
        }
        else
        {
            baby.SetParent(null);
            baby.position = babyParent.position + Vector3.up * 0.8f;
        }
    }

    void showText(string msg)
    {
        if (UI_text.text != msg) UI_text.text = msg;
    }

    public void checkOnBaby()
    {
        checking = true;
        baby.localPosition = new Vector3(0, -0.15f, 0.7f);
        baby.localEulerAngles = Vector3.zero;
    // baby.transform.localPosition.LeanMoveY(5,3);
    // LeanTween(baby.transform.localEulerAngles.z, 0, 3);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(Camera.main.transform.position, Camera.main.transform.position + (Camera.main.transform.forward*contactDistance));
    }

void OnDrawGizmos() {
Gizmos.color = Color.red;
Gizmos.DrawWireSphere(baby.transform.position, 5)
Gizmos.DrawLine(baby.transform.position, baby.transform.position + Vector3.forward*4);
}

}
