using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Baby : MonoBehaviour
{
    public int reasonID;
    /*
    0 - hungry
    1 - dirty
    */
    public bool isSleeping, isSoothing, hasClothes, hasNappy, isCleaned;
    [SerializeField] TextMeshProUGUI cryUI; //make indep script for handling UI text
    [SerializeField] Slider crySlider;

    [Header("Audio")]
    [SerializeField] public AudioSource audioSource;
    [SerializeField] TextMeshProUGUI reason;

    void Awake()
    {
        audioSource.volume = 0;

        isSleeping = false;
        isSoothing = false;
        hasClothes = true;
        hasNappy = true;
        reasonID = Random.Range(0,2); //max exclusive
        if (reasonID == 1) isCleaned = false;
        else isCleaned = true;

        reason.text = $"{reasonID}";
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
        reasonID = 1; //dirty
        isCleaned = false;
    }

    public void sleeps()
    {
        isSleeping = true;
        audioSource.volume = 0;
        audioSource.Stop();
    }
}
