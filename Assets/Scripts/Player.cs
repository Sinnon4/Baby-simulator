using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Transform baby, babyParent, bassinet, changeTable, swing;
    Baby babyScript;
    bool hasBaby, checking, feeding, cleaned;
    [SerializeField] float contactDistance = 2;
    //[SerializeField] GameObject actionDial;
    [SerializeField] TextMeshProUGUI UI_text;
    

    void Awake()
    {
        babyScript = FindAnyObjectByType<Baby>();
        resetBabyPos(bassinet);
    }

    void Update()
    {
        if (hasBaby && !feeding && Input.GetKeyDown(KeyCode.Mouse2))
        {
            feeding = true;
            babyScript.audioSource.Stop();
            showText("feeding");
        }
        else if (hasBaby && feeding && Input.GetKeyDown(KeyCode.Mouse2))
        {
            feeding = false;
            if (babyScript.reasonID == 0) babyScript.sleeps();
            else babyScript.audioSource.Play();
        }

        if (!checking && !feeding && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, contactDistance))
        {
            if (!hasBaby && hit.collider.CompareTag("Baby"))
            {
                if (babyParent == changeTable) showText("Pick up/clean baby");
                else showText("Pick up baby");

                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    hasBaby = true;
                    resetBabyPos(Camera.main.transform);
                    showText("");

                    if (babyScript.isSleeping && Random.Range(0f,1f) < 0.3f) babyScript.wakeUpBaby();
                    else if (cleaned && babyScript.reasonID == 1) babyScript.sleeps();
                }
                else if (babyParent == changeTable && Input.GetKeyDown(KeyCode.Mouse2))
                {
                    print("cleaned nappy");
                    cleaned = true;
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

                    // babyScript.wakeUpBaby();
                    // babyScript.reasonID = 1;
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
            else if (hasBaby && hit.collider.CompareTag("Swing"))
            {
                showText("Put baby in swing");
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    hasBaby = false;
                    resetBabyPos(swing);
                    showText("");

                    if (babyScript.reasonID == 2) babyScript.sleeps();
                }
            }
            else showText("");
        }
        else if (checking && Input.GetKeyDown(KeyCode.Mouse2))
        {
            resetBabyPos(Camera.main.transform);
        }
        else if (!feeding) showText("");
    }

    void resetBabyPos(Transform parent)
    {
        babyParent = parent;
        baby.eulerAngles = babyParent.eulerAngles + Vector3.forward * 90;

        if (parent == swing)
        {
            baby.SetParent(swing);
        }
        else
        {
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
    }

    void showText(string msg)
    {
        if (UI_text.text != msg) UI_text.text = msg;
    }

    void checkOnBaby()
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
}
