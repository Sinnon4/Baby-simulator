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
using UnityEngine;

public class Baby : MonoBehaviour
{
    public int reasonID;
    /*
    0 - hungry
    1 - dirty
    2 - annoyed
    */
    public bool isSleeping;

    [Header("Audio")]
    public AudioSource audioSource;
    [SerializeField] AudioClip cryingClip;
    [SerializeField] float vol = 1;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = cryingClip;
        audioSource.volume = vol;
        audioSource.loop = true;
        audioSource.Play();

        isSleeping = false;
        reasonID = Random.Range(0,3); //max exclusive
    }

    public void wakeUpBaby()
    {
        audioSource.clip = cryingClip;
        audioSource.loop = true;
        audioSource.Play();

        isSleeping = false;
        // reasonID = 2;
        reasonID = Random.Range(0,3); //TEMP
    }

    public void sleeps()
    {
        isSleeping = true;
        audioSource.Stop();
    }
}
