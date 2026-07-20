using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] Transform baby, babyParent, crib, changeTable, swing;
    [SerializeField] float contactDistance = 2;
    Baby babyScript;
    bool hasBaby, checking, feeding, cleaned, stinkyRoom;
    public GameObject inHand, dirtyNappy;
    //[SerializeField] GameObject actionDial;
    [SerializeField] Counter energy;
    [SerializeField] TextMeshProUGUI UI_text, energyUI, heldItem;
    [SerializeField] Slider energySlider;

    [SerializeField] Counter nappies, wipes;

    void Awake()
    {
        babyScript = FindAnyObjectByType<Baby>();
        resetBabyPos(crib);

        energy.value = energy.maxValue;

        inHand = null;
        heldItem.text = "Held item: -";
    }

    void Update()
    {
        if (!babyScript.isSleeping && energy.value > 0) energy.value -= 0.1f;
        else if (babyScript.isSleeping && energy.value < 100) energy.value += 0.001f;
        energyUI.text = $"Energy %: {Mathf.Round(energy.value)}";
        energySlider.value = energy.value;


        if (hasBaby)
        {
            if (!checking && !feeding && Input.GetKeyDown(KeyCode.Mouse1))
            {
                feeding = true;
                if (babyScript.reasonID == 0) babyScript.isSoothing = true;
                showText("feeding");
            }
            else if (feeding && Input.GetKeyDown(KeyCode.Mouse1))
            {
                feeding = false;
                babyScript.isSoothing = false;
            }
            else if (!checking && !feeding
                && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, contactDistance))
            {
                if (hit.collider.CompareTag("Crib"))
                {
                    showText("Put down baby");
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        hasBaby = false;
                        resetBabyPos(crib);
                        showText("");
                    }
                }
                else if (hit.collider.CompareTag("Change table"))
                {
                    showText("Put down baby");
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        hasBaby = false;
                        resetBabyPos(changeTable);
                        showText("");
                    }
                }
                else if (hit.collider.CompareTag("Swing"))
                {
                    showText("Put baby in swing");
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        hasBaby = false;
                        resetBabyPos(swing);
                        showText("");

                        if (babyScript.reasonID == 2) babyScript.isSoothing = true;
                    }
                }
                else showText("");
            }
            else if (checking && Input.GetKeyDown(KeyCode.Mouse1))
            {
                resetBabyPos(Camera.main.transform);
            }
            else if (!feeding) showText("");
        }

        else if (babyParent == crib)
        {
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, contactDistance))
            {
                if (inHand is null)
                {
                    if (hit.collider.CompareTag("Baby"))
                    {
                        showText("Pick up baby");

                        if (Input.GetKeyDown(KeyCode.Mouse0))
                        {
                            hasBaby = true;

                            if (babyScript.isSleeping && Random.Range(0f,1f) < 0.3f) babyScript.wakeUpBaby();

                            resetBabyPos(Camera.main.transform);
                            showText("");
                        }
                    }
                    else if (hit.collider.CompareTag("Wipes"))
                    {
                        showText("Grab wipe");

                        if (Input.GetKeyDown(KeyCode.Mouse0))
                        {
                            inHand = wipes.take(hit.collider.gameObject, Camera.main.transform, new Vector3(0, -0.2f, 0.45f)); //TBC pos
                        }
                    }
                    else if (hit.collider.CompareTag("Nappies"))
                    {
                        showText("Grab nappy");

                        if (Input.GetKeyDown(KeyCode.Mouse0))
                        {
                            inHand = nappies.take(hit.collider.gameObject, Camera.main.transform, new Vector3(0, -0.2f, 0.45f)); //TBC pos
                        }
                    }
                    else showText("");
                }
                else if (inHand is not null && Input.GetKeyDown(KeyCode.Mouse0))
                {
                    dropItem();
                }
                else showText("");
            }
            else if (inHand is not null && Input.GetKeyDown(KeyCode.Mouse0))
            {
                dropItem();
            }
            else showText("");
        }

        else if (babyParent == changeTable)
        {
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, contactDistance))
            {
                if (inHand is not null)
                {
                    if (inHand.tag == "Wipes" && hit.collider.CompareTag("Baby"))
                    {
                        showText("Wipe baby");

                        if (Input.GetKeyDown(KeyCode.Mouse0))
                        {
                            Destroy(inHand);
                            inHand = null;

                            heldItem.text = "Held item: -";

                            print("cleaned bum");
                            cleaned = true;
                        }
                    }
                    else if (!babyScript.hasNappy && inHand.tag == "Nappies" && hit.collider.CompareTag("Baby"))
                    {
                        if (!cleaned) showText("Baby needs a wipe!");
                        else
                        {
                            showText("Put on nappy");

                            if (Input.GetKeyDown(KeyCode.Mouse0))
                            {
                                Destroy(inHand);
                                inHand = null;

                                heldItem.text = "Held item: -";

                                print("new nappy");
                                babyScript.hasNappy = true;
                            }
                        }
                    }
                    else if (inHand.tag == "Dirty nappy")
                    {
                        if (hit.collider.CompareTag("Baby")) showText("Need to bin dirty nappy");
                        else if (hit.collider.CompareTag("Bin"))
                        {
                            showText("Bin nappy");

                            if (Input.GetKeyDown(KeyCode.Mouse0))
                            {
                                Destroy(inHand);
                                inHand = null;

                                heldItem.text = "Held item: -";
                            }
                        }
                        else showText("");
                    }
                    else if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        if (inHand.tag == "Dirty nappy") stinkyRoom = true;
                        dropItem();
                    }
                    else showText("");
                }
                else //inHand is null
                {
                    if (hit.collider.CompareTag("Baby"))
                    {
                        if (babyScript.hasNappy)
                        {
                            showText("Pick up baby (LC) or remove nappy (RC)");

                            if (Input.GetKeyDown(KeyCode.Mouse0))
                            {
                                hasBaby = true;

                                if (cleaned && babyScript.reasonID == 1) { babyScript.sleeps(); cleaned = false; }

                                resetBabyPos(Camera.main.transform);
                                showText("");
                            }
                            else if (Input.GetKeyDown(KeyCode.Mouse1))
                            {
                                babyScript.hasNappy = false;
                                inHand = Instantiate(dirtyNappy, Camera.main.transform);
                                inHand.transform.localPosition = new Vector3(0, -0.2f, 0.45f);
                                heldItem.text = "Held item: Dirty Nappy";
                            }
                        }
                        else if (!babyScript.hasNappy && !cleaned)
                        {
                            showText("Baby needs a wipe!");
                        }
                        else showText("Baby needs a nappy!");
                    }
                    
                    else if (hit.collider.CompareTag("Wipes"))
                    {
                        if (wipes.value > 0)
                        {
                            showText("Grab wipe");

                            if (Input.GetKeyDown(KeyCode.Mouse0))
                            {
                                inHand = wipes.take(hit.collider.gameObject, Camera.main.transform, new Vector3(0, -0.2f, 0.45f));
                            }
                        }
                        else showText("Need more wipes");
                    }
                    else if (hit.collider.CompareTag("Nappies"))
                    {
                        if (nappies.value > 0)
                        {
                            showText("Grab nappy");

                            if (Input.GetKeyDown(KeyCode.Mouse0))
                            {
                                inHand = nappies.take(hit.collider.gameObject, Camera.main.transform, new Vector3(0, -0.2f, 0.45f));
                            }
                        }
                        else showText("Need more nappies");
                    }
                    else showText("");
                }
            }
            else if (inHand is not null && Input.GetKeyDown(KeyCode.Mouse0))
            {
                dropItem();
            }
            else showText("");
        }

        else if (babyParent == swing)
        {
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, contactDistance))
            {
                if (inHand is null)
                {
                    if (hit.collider.CompareTag("Baby"))
                    {
                        showText("Pick up baby");

                        if (Input.GetKeyDown(KeyCode.Mouse0))
                        {
                            hasBaby = true;

                            babyScript.isSoothing = false; //if picking up from swing

                            resetBabyPos(Camera.main.transform);
                            showText("");
                        }
                    }
                    else if (hit.collider.CompareTag("Wipes"))
                    {
                        showText("Grab wipe");

                        if (Input.GetKeyDown(KeyCode.Mouse0))
                        {
                            inHand = wipes.take(hit.collider.gameObject, Camera.main.transform, new Vector3(0, -0.2f, 0.45f));
                        }
                    }
                    else if (hit.collider.CompareTag("Nappies"))
                    {
                        showText("Grab nappy");

                        if (Input.GetKeyDown(KeyCode.Mouse0))
                        {
                            inHand = nappies.take(hit.collider.gameObject, Camera.main.transform, new Vector3(0, -0.2f, 0.45f));
                        }
                    }
                    else showText("");
                }
            }
            else if (inHand is not null && Input.GetKeyDown(KeyCode.Mouse0))
            {
                dropItem();
            }
            else showText("");
        }
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

    // void pickup(GameObject item, GameObject hitObj, string name)
    // {
    //     if (hitObj.name == name)
    //     { inHand = Instantiate(item, Camera.main.transform); }
    //     else //if picking up previous item
    //     {
    //         inHand = hitObj;
    //         inHand.transform.SetParent(Camera.main.transform);
    //         inHand.GetComponent<Rigidbody>().isKinematic = true;
    //         inHand.GetComponent<BoxCollider>().isTrigger = true;
    //     }
        
    //     inHand.transform.localPosition = new Vector3(0, -0.2f, 0.45f);
    //     heldItem.text = $"Held item: {name}";
    //     print("move helditem text for new setup");
    // }

    void dropItem()
    {
        inHand.transform.SetParent(null);
        inHand.GetComponent<Rigidbody>().isKinematic = false;
        inHand.GetComponent<BoxCollider>().isTrigger = false;
        inHand = null;
        
        heldItem.text = "Held item: -";
        //add condition to destroy old objects if >20 exists
    }

    public void resetRound() //public?
    {
    print("add more here  for round reset (location respawn)");
        cleaned = false;
        // babyScript.isSleeping = false;
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
