/*------------------------------------------
Game structure:
Spawn in and baby is crying
To get baby to stop crying, attempt multiple different things which could be the cause
These can include;
    - hungry: feed baby
    - dirty nappy: change baby
    - hot: turn on the ceiling fan
    - cold: wrap in blanket
    - gas: burp baby
    - annoyed (if woken up abruptly): put in swing

When on change table - need to remove clothes, remove nappy, grab wipes, wipe, then reapply nappy and clothes
Limited amount of clothes available, and laundry has a max capacity
Need to wash clothes and dry to restock
Wipes and nappies need to be restocked from change table drawer
Dog needs to be taken outside to relieve herself within a given timeframe otherwise she will relieve herself on the floor in baby room and require cleaning
Thermometer on wall next to fan switch shows temperature
Baby hunger percentage to determine if needs feed (not shown in UI)
Tutorial explains guide to when baby needs each thing
------------------------------------------*/
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Baby : MonoBehaviour
{
    public int reasonID;
    /*
    0 - hungry
    1 - dirty
    2 - annoyed
    */
    public bool isSleeping, isSoothing, hasNappy;
    [SerializeField] TextMeshProUGUI cryUI; //make indep script for handling UI text
    [SerializeField] Slider crySlider;

    [Header("Audio")]
    [SerializeField] public AudioSource audioSource;

    void Awake()
    {
        audioSource.volume = 0;

        isSleeping = false;
        isSoothing = false;
        hasNappy = true;
        reasonID = Random.Range(0,3); //max exclusive
        print("add changing dirty nappy logic");
    }

    void Update()
    {
        if (isSoothing)
        {
            if (audioSource.volume > 0) audioSource.volume -= Time.deltaTime/10;
            else
            {
                isSoothing = false;
                sleeps();
            }
        }
        else if (!isSleeping && !isSoothing)
        {
            if (audioSource.volume < 1) audioSource.volume += Time.deltaTime/5;
        }

        cryUI.text = $"Cry %: {Mathf.Round(audioSource.volume * 100)}";
        crySlider.value = audioSource.volume;
    }

    public void wakeUpBaby()
    {
        isSleeping = false;
        audioSource.Play();
        reasonID = 2;
    }

    public void sleeps()
    {
        isSleeping = true;
        audioSource.Stop();
    }
}
