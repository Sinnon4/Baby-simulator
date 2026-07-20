using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISlider : MonoBehaviour
{
    // Baby babyScript;
    // [SerializeField] Counter energy;
    // // float energy;
    // [SerializeField] TextMeshProUGUI UI_text, energyUI, heldItem;
    // [SerializeField] Slider energySlider;

    // void Awake()
    // {
    //     babyScript = FindAnyObjectByType<Baby>();

    //     energy.value = energy.maxValue;
    // }

    // void Update()
    // {
    //     if (!babyScript.isSleeping && energy.value > 0) energy.value -= 0.1f;
    //     else if (babyScript.isSleeping && energy.value < 100) energy.value += 0.001f;
    //     energyUI.text = $"Energy %: {Mathf.Round(energy.value)}";
    //     energySlider.value = energy.value;

    //     ///calculations different for energy vs cry?
    // }
    void OnCollisionEnter(Collision collision)
    {
        print(collision.gameObject.name);
    }
}
