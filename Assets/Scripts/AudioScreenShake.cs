using UnityEngine;

public class AudioScreenShake : MonoBehaviour
{
    public AudioSource audioSource;
    public float shakeMultiplier = 5f, tweenTime;
    public float n;
    Vector3 originalPosition;
    float[] audioSamples = new float[64];

    void Start()
    {
        originalPosition = transform.localPosition;
    }

    void Update()
    {
        if (audioSource.isPlaying)
        {
            audioSource.GetOutputData(audioSamples, 0);
            float sum = 0;

            foreach (var sample in audioSamples)
            {
                sum += sample * sample;
            }

            float rmsValue = Mathf.Sqrt(sum / audioSamples.Length);
            float currentIntensity = rmsValue * shakeMultiplier;
            if (rmsValue > n) n = rmsValue;
            Vector3 randomOffset = Random.insideUnitSphere * currentIntensity;
            Vector3 pos = new Vector3(originalPosition.x + randomOffset.x, originalPosition.y + randomOffset.y, originalPosition.z);
            // transform.localPosition = originalPosition + randomOffset;
            //FREEZE Z
            transform.LeanMoveLocal(pos, tweenTime);
        }
        else
        {
            // transform.localPosition = originalPosition;
            transform.LeanMoveLocal(originalPosition, tweenTime);
        }
    }
}
