using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] Transform baby, babyParent, crib, changeTable;
    [SerializeField] float contactDistance = 2;
    Baby babyScript;
    bool checking, feeding, stinkyRoom, actioned;
    public GameObject inHand, dirtyNappy, dirtyClothes;
    //[SerializeField] GameObject actionDial;
    [SerializeField] Counter energy;
    [SerializeField] TextMeshProUGUI UI_text, energyUI, heldItem;
    [SerializeField] Slider energySlider;

    [SerializeField] Counter nappies, wipes, clothes, wash, bin;
/*
things wanting to add:
1. have to remove clothes and put in wash, then get clean clothes from drawer.
Wash has capacity and clean clothes has counter which relies on getting clean laundry to restock
2. when changing dirty nappy, number of wipes required to clean baby is random number between 1 and 4 - ADD LATER
3. ability to restock wipes and nappies piles from grabbing new ones from under change table
Include capacity for under change table - Have to buy more from shops when low
4. add wondering dog - add in requirement to take dog out (walk up to front door and click) or dog shits on ground (have to clean up shit or room stinky and baby wakes up)
5. add temperature on wall mounted thermometer which reads either hot, cold, or nice
6. figure out the cry vs energy system (goal of game is to get back into bed before energy depletes?)
7. change feeding option to getting a bottle and warming it up then cleaning it properly
8. add in settings to turn off UI prompts

NEXT THING TO ADD SHOULD BE FEEDING VIA BOTTLE OR RESTOCKING ITEMS

GAME DESIGN DOC SO FAR
You are a dad babysitting your newborn baby while your wife is away.
You need to get the baby back to sleep before you run out of energy.
Baby may want feed (you need to burp baby afterward or else), change (you need to bin dirty nappy or else), or fix temp.
You also need to keep cleaning the laundry, emptying the nappy bin, and restocking the clothes, nappies, and wipes
Every time you get the baby back to sleep you spend the free time (1 token) on upgrading the max capacity of your items.
*/
    void Awake()
    {
        babyScript = FindAnyObjectByType<Baby>();
        resetBabyPos(crib);

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

        if (actioned) actioned = false; //reset each frame

        // if (hasBaby)
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
                        // hasBaby = false;
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
                                // hasBaby = true;
                                inHand = baby.gameObject;

                                if (babyScript.isCleaned && babyScript.reasonID == 1) babyScript.sleeps();

                                resetBabyPos(Camera.main.transform);
                                showText("");
                                actioned = true;
                            }
                            else if (Input.GetKeyDown(KeyCode.Mouse1))
                            {
                                babyScript.hasClothes = false;
                                inHand = Instantiate(dirtyClothes, Camera.main.transform); //change this to dirtyy clothes
                                inHand.transform.localPosition = new Vector3(0, -0.2f, 0.45f);
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
                                inHand.transform.localPosition = new Vector3(0, -0.2f, 0.45f);
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
                            // hasBaby = true;
                            inHand = baby.gameObject;

                            if (babyScript.isSleeping && Random.Range(0f,1f) < 0.3f) babyScript.wakeUpBaby();

                            resetBabyPos(Camera.main.transform);
                            showText("");
                            actioned = true;
                        }
                    }
                }
                else if (hit.collider.CompareTag("Wipes"))
                {
                    if ((hit.collider.gameObject.name == "Wipes" && wipes.value > 0)
                        || hit.collider.gameObject.name != "Wipes")
                    {
                        showText("Grab wipe");

                        if (Input.GetKeyDown(KeyCode.Mouse0))
                        {
                            inHand = wipes.take(hit.collider.gameObject, Camera.main.transform, new Vector3(0, -0.2f, 0.45f));
                            actioned = true;
                        }
                    }
                    else showText("Need more wipes");
                }
                else if (hit.collider.CompareTag("Nappies"))
                {
                    if ((hit.collider.gameObject.name == "Nappies" && nappies.value > 0)
                        || hit.collider.gameObject.name != "Nappies")
                    {
                        showText("Grab nappy");

                        if (Input.GetKeyDown(KeyCode.Mouse0))
                        {
                            inHand = nappies.take(hit.collider.gameObject, Camera.main.transform, new Vector3(0, -0.2f, 0.45f));
                            actioned = true;
                        }
                    }
                    else showText("Need more nappies");
                }
                else if (hit.collider.CompareTag("Clothes"))
                {
                    if ((hit.collider.gameObject.name == "Clothes" && clothes.value > 0)
                        || hit.collider.gameObject.name != "Clothes")
                    {
                        showText("Grab clothes");

                        if (Input.GetKeyDown(KeyCode.Mouse0))
                        {
                            inHand = clothes.take(hit.collider.gameObject, Camera.main.transform, new Vector3(0, -0.2f, 0.45f));
                            actioned = true;
                        }
                    }
                    else showText("Need more clothes");
                }
                else if (hit.collider.CompareTag("Dirty nappy") || hit.collider.CompareTag("Dirty clothes"))
                {
                    if (hit.collider.CompareTag("Dirty nappy")) showText("Pickup dirty nappy");
                    else if (hit.collider.CompareTag("Dirty clothes")) showText("Pickup dirty clothes");

                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        inHand = hit.collider.gameObject;
                        inHand.transform.SetParent(Camera.main.transform);
                        inHand.transform.localPosition = new Vector3(0, -0.2f, 0.45f);
                        inHand.transform.localScale = new Vector3(0.4f, 0.04f, 0.4f);
                        print("may want to vary size depending on object");
                        inHand.transform.localRotation = Quaternion.Euler(-45, 0, 0);
                        inHand.GetComponent<Rigidbody>().isKinematic = true;
                        inHand.GetComponent<BoxCollider>().isTrigger = true;

                        if (hit.collider.CompareTag("Dirty nappy")) heldItem.text = "Held item: Dirty Nappy";
                        else if (hit.collider.CompareTag("Dirty clothes")) heldItem.text = "Held item: Dirty Clothes";
                    
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
                        showText("Bin nappy");

                        if (Input.GetKeyDown(KeyCode.Mouse0) && bin.deposit(inHand.transform))
                        {
                            Destroy(inHand);
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
                        showText("Wash clothes");

                        if (Input.GetKeyDown(KeyCode.Mouse0) && wash.deposit(inHand.transform))
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

                                    // if (babyScript.reasonID == 1) babyScript.isSoothing = true;
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
            if (inHand.tag == "Dirty nappy") stinkyRoom = true;
            dropItem();
        }
        // }




    //     else if (babyParent == crib)
    //     {
    //         if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, contactDistance))
    //         {
    //             if (inHand is null)
    //             {
    //                 if (hit.collider.CompareTag("Baby"))
    //                 {
    //                     showText("Pick up baby");

    //                     if (Input.GetKeyDown(KeyCode.Mouse0))
    //                     {
    //                         hasBaby = true;

    //                         if (babyScript.isSleeping && Random.Range(0f,1f) < 0.3f) babyScript.wakeUpBaby();

    //                         resetBabyPos(Camera.main.transform);
    //                         showText("");
    //                     }
    //                 }
    //                 else if (hit.collider.CompareTag("Wipes"))
    //                 {
    //                     if ((hit.collider.gameObject.name == "Wipes" && wipes.value > 0)
    //                         || hit.collider.gameObject.name != "Wipes")
    //                     {
    //                         showText("Grab wipe");

    //                         if (Input.GetKeyDown(KeyCode.Mouse0))
    //                         {
    //                             inHand = wipes.take(hit.collider.gameObject, Camera.main.transform, new Vector3(0, -0.2f, 0.45f));
    //                         }
    //                     }
    //                     else showText("Need more wipes");
    //                 }
    //                 else if (hit.collider.CompareTag("Nappies"))
    //                 {
    //                     if ((hit.collider.gameObject.name == "Nappies" && nappies.value > 0)
    //                         || hit.collider.gameObject.name != "Nappies")
    //                     {
    //                         showText("Grab nappy");

    //                         if (Input.GetKeyDown(KeyCode.Mouse0))
    //                         {
    //                             inHand = nappies.take(hit.collider.gameObject, Camera.main.transform, new Vector3(0, -0.2f, 0.45f));
    //                         }
    //                     }
    //                     else showText("Need more nappies");
    //                 }
    //                 // else if (hit.collider.CompareTag("Clothes"))
    //                 // {
    //                 //     if ((hit.collider.gameObject.name == "Clothes" && clothes.value > 0)
    //                 //         || hit.collider.gameObject.name != "Clothes")
    //                 //     {
    //                 //         showText("Grab clothes");

    //                 //         if (Input.GetKeyDown(KeyCode.Mouse0))
    //                 //         {
    //                 //             inHand = clothes.take(hit.collider.gameObject, Camera.main.transform, new Vector3(0, -0.2f, 0.45f));
    //                 //         }
    //                 //     }
    //                 //     else showText("Need more clothes");
    //                 // }
    //                 else showText("");
    //             }
    //             else if (inHand is not null && Input.GetKeyDown(KeyCode.Mouse0))
    //             {
    //                 dropItem();
    //             }
    //             else showText("");
    //         }
    //         else if (inHand is not null && Input.GetKeyDown(KeyCode.Mouse0))
    //         {
    //             dropItem();
    //         }
    //         else showText("");
    //     }

    //     else if (babyParent == changeTable)
    //     {
    //         if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, contactDistance))
    //         {
    //             if (inHand is not null)
    //             {
    //                 if (inHand.tag == "Wipes" && hit.collider.CompareTag("Baby"))
    //                 {
    //                     showText("Wipe baby");

    //                     if (Input.GetKeyDown(KeyCode.Mouse0))
    //                     {
    //                         Destroy(inHand);
    //                         inHand = null;

    //                         heldItem.text = "Held item: -";

    //                         print("cleaned bum");
    //                         cleaned = true;
    //                     }
    //                 }
    //                 else if (!babyScript.hasNappy && inHand.tag == "Nappies" && hit.collider.CompareTag("Baby"))
    //                 {
    //                     if (!cleaned) showText("Baby needs a wipe!");
    //                     else
    //                     {
    //                         showText("Put on nappy");

    //                         if (Input.GetKeyDown(KeyCode.Mouse0))
    //                         {
    //                             Destroy(inHand);
    //                             inHand = null;

    //                             heldItem.text = "Held item: -";

    //                             print("new nappy");
    //                             babyScript.hasNappy = true;
    //                         }
    //                     }
    //                 }
    //                 else if (inHand.tag == "Dirty nappy")
    //                 {
    //                     if (hit.collider.CompareTag("Baby")) showText("Need to bin dirty nappy");
    //                     else if (hit.collider.CompareTag("Bin"))
    //                     {
    //                         showText("Bin nappy");

    //                         if (Input.GetKeyDown(KeyCode.Mouse0))
    //                         {
    //                             Destroy(inHand);
    //                             inHand = null;

    //                             heldItem.text = "Held item: -";
    //                         }
    //                     }
    //                     else showText("");
    //                 }
    //                 else if (Input.GetKeyDown(KeyCode.Mouse0))
    //                 {
    //                     if (inHand.tag == "Dirty nappy") stinkyRoom = true;
    //                     dropItem();
    //                 }
    //                 else showText("");
    //             }
    //             else //inHand is null
    //             {
    //                 if (hit.collider.CompareTag("Baby"))
    //                 {
    //                     if (babyScript.hasNappy)
    //                     {
    //                         showText("Pick up baby (LC) or remove nappy (RC)");

    //                         if (Input.GetKeyDown(KeyCode.Mouse0))
    //                         {
    //                             hasBaby = true;

    //                             if (cleaned && babyScript.reasonID == 1) { babyScript.sleeps(); cleaned = false; }

    //                             resetBabyPos(Camera.main.transform);
    //                             showText("");
    //                         }
    //                         else if (Input.GetKeyDown(KeyCode.Mouse1))
    //                         {
    //                             babyScript.hasNappy = false;
    //                             inHand = Instantiate(dirtyNappy, Camera.main.transform);
    //                             inHand.transform.localPosition = new Vector3(0, -0.2f, 0.45f);
    //                             inHand.transform.localScale = new Vector3(0.4f, 0.04f, 0.4f);
    //                             inHand.transform.localRotation = Quaternion.Euler(-45, 0, 0);
    //                             heldItem.text = "Held item: Dirty Nappy";
    //                         }
    //                     }
    //                     else if (!babyScript.hasNappy && !cleaned)
    //                     {
    //                         showText("Baby needs a wipe!");
    //                     }
    //                     else showText("Baby needs a nappy!");
    //                 }
                    
    //                 else if (hit.collider.CompareTag("Wipes"))
    //                 {
    //                     if ((hit.collider.gameObject.name == "Wipes" && wipes.value > 0)
    //                         || hit.collider.gameObject.name != "Wipes")
    //                     {
    //                         showText("Grab wipe");

    //                         if (Input.GetKeyDown(KeyCode.Mouse0))
    //                         {
    //                             inHand = wipes.take(hit.collider.gameObject, Camera.main.transform, new Vector3(0, -0.2f, 0.45f));
    //                         }
    //                     }
    //                     else showText("Need more wipes");
    //                 }
    //                 else if (hit.collider.CompareTag("Nappies"))
    //                 {
    //                     if ((hit.collider.gameObject.name == "Nappies" && nappies.value > 0)
    //                         || hit.collider.gameObject.name != "Nappies")
    //                     {
    //                         showText("Grab nappy");

    //                         if (Input.GetKeyDown(KeyCode.Mouse0))
    //                         {
    //                             inHand = nappies.take(hit.collider.gameObject, Camera.main.transform, new Vector3(0, -0.2f, 0.45f));
    //                         }
    //                     }
    //                     else showText("Need more nappies");
    //                 }
    //                 else showText("");
    //             }
    //         }
    //         else if (inHand is not null && Input.GetKeyDown(KeyCode.Mouse0))
    //         {
    //             dropItem();
    //         }
    //         else showText("");
        // }

        // else if (babyParent == swing)
        // {
        //     if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, contactDistance))
        //     {
        //         if (inHand is null)
        //         {
        //             if (hit.collider.CompareTag("Baby"))
        //             {
        //                 showText("Pick up baby");

        //                 if (Input.GetKeyDown(KeyCode.Mouse0))
        //                 {
        //                     hasBaby = true;

                            // babyScript.isSoothing = false; //if picking up from swing

        //                     resetBabyPos(Camera.main.transform);
        //                     showText("");
        //                 }
        //             }
        //             else if (hit.collider.CompareTag("Wipes"))
        //             {
        //                 if ((hit.collider.gameObject.name == "Wipes" && wipes.value > 0)
        //                     || hit.collider.gameObject.name != "Wipes")
        //                 {
        //                     showText("Grab wipe");

        //                     if (Input.GetKeyDown(KeyCode.Mouse0))
        //                     {
        //                         inHand = wipes.take(hit.collider.gameObject, Camera.main.transform, new Vector3(0, -0.2f, 0.45f));
        //                     }
        //                 }
        //                 else showText("Need more wipes");
        //             }
        //             else if (hit.collider.CompareTag("Nappies"))
        //             {
        //                 if ((hit.collider.gameObject.name == "Nappies" && nappies.value > 0)
        //                     || hit.collider.gameObject.name != "Nappies")
        //                 {
        //                     showText("Grab nappy");

        //                     if (Input.GetKeyDown(KeyCode.Mouse0))
        //                     {
        //                         inHand = nappies.take(hit.collider.gameObject, Camera.main.transform, new Vector3(0, -0.2f, 0.45f));
        //                     }
        //                 }
        //                 else showText("Need more nappies");
        //             }
        //             else showText("");
        //         }
        //     }
        //     else if (inHand is not null && Input.GetKeyDown(KeyCode.Mouse0))
        //     {
        //         dropItem();
        //     }
        //     else showText("");
        // }
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
        inHand.GetComponent<Collider>().isTrigger = false; //changed from boxcollider due to baby
        inHand = null;
        
        heldItem.text = "Held item: -";
        //add condition to destroy old objects if >20 exists
    }

    // public void resetRound() //public?
    // {
    //     print("add more here  for round reset (location respawn)");
    //     cleaned = false;
    //     // babyScript.isSleeping = false;
    // }

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
