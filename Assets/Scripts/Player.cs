using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] Transform baby, babyParent, crib, changeTable;
    [SerializeField] float contactDistance = 2;
    Baby babyScript;
    bool checking, feeding, stinkyRoom, actioned, washing, washed;
    public GameObject inHand, dirtyNappy, dirtyClothes;
    [SerializeField] Vector3 holdPos = new Vector3(0, -0.2f, 0.45f);
    //[SerializeField] GameObject actionDial;
    [SerializeField] Energy energy;
    [SerializeField] TextMeshProUGUI UI_text, energyUI, heldItem;
    [SerializeField] Slider energySlider, washSlider;

    [SerializeField] Counter nappies, wipes, clothes, wash, bin, nappiesRestock, wipesRestock;

    void Awake()
    {
        babyScript = FindAnyObjectByType<Baby>();
        resetBabyPos(crib);
        energy.value = 100;

        inHand = null;
        heldItem.text = "Held item: -";
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        if (!babyScript.isSleeping && energy.value > 0) energy.value -= 0.01f;
        else if (babyScript.isSleeping && energy.value < 100) energy.value += 0.01f;
        energyUI.text = $"Energy %: {Mathf.Round(energy.value)}";
        energySlider.value = energy.value;

        if (washing && washSlider.value < 100)
        {
            washSlider.value += 0.2f;

            if (washSlider.value >= 100)
            {
                washing = false;
                washed = true;
            }
        }

        if (actioned) actioned = false; //reset each frame

        if (inHand == baby.gameObject)
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
                if (hit.collider.CompareTag("Crib") || hit.collider.CompareTag("Change table"))
                {
                    showText("Put down baby");
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        inHand = null;
                        if (hit.collider.CompareTag("Crib")) resetBabyPos(crib);
                        else if (hit.collider.CompareTag("Change table")) resetBabyPos(changeTable);
                        showText("");
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

        else if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, contactDistance))
        {
            if (inHand is null)
            {
                if (hit.collider.CompareTag("Baby"))
                {
                    if (babyParent == changeTable)
                    {
                        if (babyScript.hasClothes)
                        {
                            showText("Pick up baby or Remove clothes");

                            if (Input.GetKeyDown(KeyCode.Mouse0))
                            {
                                inHand = baby.gameObject;

                                if (babyScript.isCleaned && babyScript.reasonID == 1) babyScript.sleeps();

                                resetBabyPos(Camera.main.transform);
                                showText("");
                                actioned = true;
                            }
                            else if (Input.GetKeyDown(KeyCode.Mouse1))
                            {
                                babyScript.hasClothes = false;
                                inHand = Instantiate(dirtyClothes, Camera.main.transform);
                                inHand.transform.localPosition = holdPos;
                                inHand.transform.localScale = new Vector3(0.4f, 0.04f, 0.4f);
                                inHand.transform.localRotation = Quaternion.Euler(-45, 0, 0);
                                heldItem.text = "Held item: Dirty Clothes";
                                actioned = true;
                            }
                        }
                        else if (!babyScript.hasClothes && babyScript.hasNappy && !babyScript.isCleaned)
                        {
                            showText("Remove nappy");

                            if (Input.GetKeyDown(KeyCode.Mouse0))
                            {
                                babyScript.hasNappy = false;
                                inHand = Instantiate(dirtyNappy, Camera.main.transform);
                                inHand.transform.localPosition = holdPos;
                                inHand.transform.localScale = new Vector3(0.4f, 0.04f, 0.4f);
                                inHand.transform.localRotation = Quaternion.Euler(-45, 0, 0);
                                heldItem.text = "Held item: Dirty Nappy";
                                actioned = true;
                            }
                        }
                        else if (!babyScript.hasClothes && babyScript.hasNappy && babyScript.isCleaned)
                        {
                            showText("Baby needs clothes!");
                        }
                        else if (!babyScript.hasNappy && babyScript.isCleaned)
                        {
                            showText("Baby needs a nappy!");
                        }
                        else if (!babyScript.hasNappy && !babyScript.isCleaned)
                        {
                            showText("Baby needs a wipe!");
                        }
                        else showText("Baby needs a nappy!");
                    }
                    else
                    {
                        showText("Pick up baby");

                        if (Input.GetKeyDown(KeyCode.Mouse0))
                        {
                            inHand = baby.gameObject;

                            if (babyScript.isSleeping && UnityEngine.Random.Range(0f,1f) < 0.3f) babyScript.wakeUpBaby();

                            resetBabyPos(Camera.main.transform);
                            showText("");
                            actioned = true;
                        }
                    }
                }
                else if (hit.collider.CompareTag("Wipes"))
                {
                    if ((hit.collider.gameObject.name == hit.collider.tag && wipes.value > 0)
                        || hit.collider.gameObject.name != hit.collider.tag)
                    {
                        showText("Grab wipe");

                        if (Input.GetKeyDown(KeyCode.Mouse0))
                        {
                            inHand = wipes.take(hit.collider.gameObject, Camera.main.transform, holdPos);
                            actioned = true;
                        }
                    }
                    else showText("Need more wipes");
                }
                else if (hit.collider.CompareTag("Nappies"))
                {
                    if ((hit.collider.gameObject.name == hit.collider.tag && nappies.value > 0)
                        || hit.collider.gameObject.name != hit.collider.tag)
                    {
                        showText("Grab nappy");

                        if (Input.GetKeyDown(KeyCode.Mouse0))
                        {
                            inHand = nappies.take(hit.collider.gameObject, Camera.main.transform, holdPos);
                            actioned = true;
                        }
                    }
                    else showText("Need more nappies");
                }
                else if (hit.collider.CompareTag("Clothes"))
                {
                    if ((hit.collider.gameObject.name == hit.collider.tag && clothes.value > 0)
                        || hit.collider.gameObject.name != hit.collider.tag)
                    {
                        showText("Grab clothes");

                        if (Input.GetKeyDown(KeyCode.Mouse0))
                        {
                            inHand = clothes.take(hit.collider.gameObject, Camera.main.transform, holdPos);
                            actioned = true;
                        }
                    }
                    else showText("Need more clothes");
                }
                else if (hit.collider.CompareTag("Wipes restock"))
                {
                    if ((hit.collider.gameObject.name == hit.collider.tag && wipesRestock.value >= wipesRestock.withdrawValue)
                        || hit.collider.gameObject.name != hit.collider.tag)
                    {
                        showText("Grab wipes pack");

                        if (Input.GetKeyDown(KeyCode.Mouse0))
                        {
                            inHand = wipesRestock.take(hit.collider.gameObject, Camera.main.transform, holdPos);
                            actioned = true;
                        }
                    }
                    else showText("Need to buy more packs of wipes");
                }
                else if (hit.collider.CompareTag("Nappies restock"))
                {
                    if ((hit.collider.gameObject.name == hit.collider.tag && nappiesRestock.value >= nappiesRestock.withdrawValue)
                        || hit.collider.gameObject.name != hit.collider.tag)
                    {
                        showText("Grab nappies pack");

                        if (Input.GetKeyDown(KeyCode.Mouse0))
                        {
                            inHand = nappiesRestock.take(hit.collider.gameObject, Camera.main.transform, holdPos);
                            actioned = true;
                        }
                    }
                    else showText("Need to buy more packs of nappies");
                }
                else if (hit.collider.CompareTag("Dirty nappy") || hit.collider.CompareTag("Dirty clothes")
                        || hit.collider.CompareTag("Trash") || hit.collider.CompareTag("Clean clothes"))
                {
                    if (hit.collider.CompareTag("Dirty nappy")) showText("Grab dirty nappy");
                    else if (hit.collider.CompareTag("Dirty clothes")) showText("Grab dirty clothes");
                    else if (hit.collider.CompareTag("Trash")) showText("Grab trash");
                    else if (hit.collider.CompareTag("Clean clothes")) showText("Grab clean clothes");

                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        // pickup(hit.collider.gameObject);
                        inHand = hit.collider.gameObject;
                        inHand.transform.SetParent(Camera.main.transform);
                        inHand.transform.localPosition = holdPos;
                        inHand.GetComponent<Rigidbody>().isKinematic = true;
                        inHand.GetComponent<BoxCollider>().isTrigger = true;

                        heldItem.text = $"Held item: {hit.collider.tag}";

                        actioned = true;
                    }
                }
                else if (hit.collider.CompareTag("Wash") && wash.value >= wash.withdrawValue)
                {
                    if (!washing && !washed)
                    {
                        showText("Wash clothes");

                        if (Input.GetKeyDown(KeyCode.Mouse0)) washing = true;
                    }
                    else if (washing && !washed) showText("Washing...");
                    else if (washed)
                    {
                        showText("Take clothes");

                        if (Input.GetKeyDown(KeyCode.Mouse0))
                        {
                            washed = false;
                            washSlider.value = 0;
                            inHand = wash.take(hit.collider.gameObject, Camera.main.transform, holdPos);
                            
                            heldItem.text = "Held item: Clean laundry";
                        
                            actioned = true;
                        }
                    }
                }
                else if (hit.collider.CompareTag("Bin") && bin.value >= bin.withdrawValue)
                {
                    showText("Empty bin");

                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        inHand = bin.take(hit.collider.gameObject, Camera.main.transform, holdPos);
                        
                        heldItem.text = "Held item: Trash";
                    
                        actioned = true;
                    }
                }
                else showText("");
            }
            else //holding something
            {
                if (inHand.tag == "Dirty nappy")
                {
                    if (hit.collider.CompareTag("Baby")) showText("Need to bin dirty nappy");
                    else if (hit.collider.CompareTag("Bin"))
                    {
                        if (bin.value < bin.maxValue) showText("Bin nappy");
                        else showText("Need to empty bin!");

                        if (Input.GetKeyDown(KeyCode.Mouse0) && bin.deposit())
                        {
                            Destroy(inHand); //remove this line if adding the transform self condition
                            inHand = null;

                            if (stinkyRoom) stinkyRoom = false;

                            heldItem.text = "Held item: -";
                        }
                    }
                    else showText("");
                }
                else if (inHand.tag == "Dirty clothes")
                {
                    if (hit.collider.CompareTag("Baby")) showText("Need to put dirty clothes in the wash");
                    else if (hit.collider.CompareTag("Wash"))
                    {
                        if (washed) showText("Remove clean clothes first!");
                        else if (wash.value < wash.maxValue) showText("Wash clothes");
                        else showText("Need to empty machine!");

                        if (Input.GetKeyDown(KeyCode.Mouse0) && wash.deposit())
                        {
                            Destroy(inHand);
                            inHand = null;

                            heldItem.text = "Held item: -";
                        }
                    }
                    else showText("");
                }
                else if (inHand.tag == "Clean clothes")
                {
                    if (hit.collider.CompareTag("Baby")) showText("Need to put clean clothes back in drawer");
                    else if (hit.collider.CompareTag("Clothes") && hit.collider.gameObject.name == hit.collider.tag)
                    {
                        showText("Restock clothes");

                        if (Input.GetKeyDown(KeyCode.Mouse0))
                        {
                            clothes.value = math.min(clothes.value + wash.withdrawValue, clothes.maxValue);
                            print("bug here if holding more than the withdraw amount");
                            clothes.txt.text = (clothes.value == clothes.maxValue) ? "MAX" : $"{clothes.value}";
                            Destroy(inHand);
                            inHand = null;

                            heldItem.text = "Held item: -";
                        }
                    }
                    else showText("");
                }
                else if (inHand.tag == "Nappies restock")
                {
                    if (hit.collider.CompareTag("Baby")) showText("Need to put clean clothes back in drawer");
                    else if (hit.collider.CompareTag("Nappies") && hit.collider.gameObject.name == hit.collider.tag)
                    {
                        showText("Restock nappies");

                        if (Input.GetKeyDown(KeyCode.Mouse0))
                        {
                            nappies.deposit(nappiesRestock.withdrawValue, hit.collider.transform);
                            // nappies.value = math.min(nappies.value + nappiesRestock.withdrawValue, nappies.maxValue);
                            print("bug here if holding more than the withdraw amount");
                            nappies.txt.text = (nappies.value == nappies.maxValue) ? "MAX" : $"{nappies.value}";
                            Destroy(inHand);
                            inHand = null;

                            heldItem.text = "Held item: -";
                        }
                    }
                    else showText("");
                }
                else if (inHand.tag == "Wipes restock")
                {
                    if (hit.collider.CompareTag("Baby")) showText("Need to put clean clothes back in drawer");
                    else if (hit.collider.CompareTag("Wipes") && hit.collider.gameObject.name == hit.collider.tag)
                    {
                        showText("Restock wipes");

                        if (Input.GetKeyDown(KeyCode.Mouse0))
                        {
                            wipes.deposit(wipesRestock.withdrawValue, hit.collider.transform);
                            // wipes.value = math.min(wipes.value + wipesRestock.withdrawValue, wipes.maxValue);
                            print("bug here if holding more than the withdraw amount");
                            wipes.txt.text = (wipes.value == wipes.maxValue) ? "MAX" : $"{wipes.value}";
                            Destroy(inHand);
                            inHand = null;

                            heldItem.text = "Held item: -";
                        }
                    }
                    else showText("");
                }
                else if (inHand.tag == "Trash")
                {
                    if (hit.collider.CompareTag("Baby")) showText("Need to throw trash out (use front door)");
                    else if (hit.collider.CompareTag("Door"))
                    {
                        showText("Throw trash outside");

                        if (Input.GetKeyDown(KeyCode.Mouse0))
                        {
                            Destroy(inHand);
                            inHand = null;

                            heldItem.text = "Held item: -";
                        }
                    }
                    else showText("");
                }
                else if (babyParent == changeTable)
                {
                    if (inHand.tag == "Wipes" && hit.collider.CompareTag("Baby"))
                    {
                        if (babyScript.isCleaned) showText("Baby is clean");
                        else if (!babyScript.hasNappy)
                        {
                            showText("Wipe baby");

                            if (Input.GetKeyDown(KeyCode.Mouse0))
                            {
                                Destroy(inHand);
                                inHand = null;

                                heldItem.text = "Held item: -";

                                babyScript.isCleaned = true;
                            }
                        }
                        else showText("Remove nappy first!");
                    }
                    else if (inHand.tag == "Nappies" && hit.collider.CompareTag("Baby"))
                    {
                        if (!babyScript.hasNappy)
                        {
                            if (!babyScript.isCleaned) showText("Baby needs a wipe!");
                            else
                            {
                                showText("Put on nappy");

                                if (Input.GetKeyDown(KeyCode.Mouse0))
                                {
                                    Destroy(inHand);
                                    inHand = null;

                                    heldItem.text = "Held item: -";

                                    babyScript.hasNappy = true;

                                    if (babyScript.reasonID == 1) babyScript.isSoothing = true;
                                }
                            }
                        }
                        else if (babyScript.isCleaned) showText("Baby has a nappy");
                        else if (babyScript.hasClothes) showText("Remove clothes first!");
                        else if (babyScript.hasNappy) showText("Remove nappy first!");
                    }
                    else if (inHand.tag == "Clothes" && hit.collider.CompareTag("Baby"))
                    {
                        if (!babyScript.hasClothes)
                        {
                            if (babyScript.hasNappy && !babyScript.isCleaned) showText("Baby needs cleaning!");
                            else if (babyScript.hasNappy && babyScript.isCleaned)
                            {
                                showText("Put on clothes");

                                if (Input.GetKeyDown(KeyCode.Mouse0))
                                {
                                    Destroy(inHand);
                                    inHand = null;

                                    heldItem.text = "Held item: -";

                                    babyScript.hasClothes = true;
                                }
                            }
                            else showText("Baby needs a nappy!");
                        }
                        else if (babyScript.isCleaned) showText("Baby is clean");
                        else showText("Remove clothes first!");
                    }
                    else showText("");
                }
                else showText("");
            }
        }
        else showText("");

        if (inHand is not null && Input.GetKeyDown(KeyCode.Mouse0) && !actioned)
        {
            if (inHand.tag == "Dirty nappy" || inHand.tag == "Trash") stinkyRoom = true;
            dropItem();
        }
    }

    void resetBabyPos(Transform parent)
    {
        babyParent = parent;
        baby.eulerAngles = babyParent.eulerAngles + Vector3.forward * 90;
        baby.GetComponent<Rigidbody>().isKinematic = true;
        baby.GetComponent<Collider>().isTrigger = true;

        if (inHand == baby.gameObject)
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

    // void pickup(GameObject item/*, GameObject hitObj, string name*/)
    // {
    //     inHand = item;
    //     inHand.transform.SetParent(Camera.main.transform);
    //     inHand.transform.localPosition = holdPos;
    //     // inHand.transform.localScale = Vector3.one * 0.15f;
    //     // inHand.transform.localRotation = Quaternion.Euler(-45, 0, 0);
    //     inHand.GetComponent<Rigidbody>().isKinematic = true;
    //     inHand.GetComponent<BoxCollider>().isTrigger = true;
    // }

    void dropItem()
    {
        inHand.transform.SetParent(null);
        inHand.GetComponent<Rigidbody>().isKinematic = false;
        inHand.GetComponent<Collider>().isTrigger = false; //changed from boxcollider due to baby
        inHand = null;
        
        heldItem.text = "Held item: -";
        //add condition to destroy old objects if >20 exists
    }

    void showText(string msg)
    {
        if (UI_text.text != msg) UI_text.text = msg;
    }

    // void checkOnBaby()
    // {
    //     checking = true;
    //     baby.localPosition = new Vector3(0, -0.15f, 0.7f);
    //     baby.localEulerAngles = Vector3.zero;
    // // baby.transform.localPosition.LeanMoveY(5,3);
    // // LeanTween(baby.transform.localEulerAngles.z, 0, 3);
    // }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(Camera.main.transform.position, Camera.main.transform.position + (Camera.main.transform.forward*contactDistance));
    }
}
