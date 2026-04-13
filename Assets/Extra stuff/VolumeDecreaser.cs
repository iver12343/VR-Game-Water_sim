using UnityEngine;
using System.Collections;

public class VolumeDecreaser : MonoBehaviour
{
    public float targetVolume = 0f;          // Final volume after decrease
    public float decreaseDuration = 5f;      // Time it takes to decrease volume
    public float delayBeforeStart = 5f;      // Time before starting decrease

    private AudioSource audioSource;

    void Start()
    {
        
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(DecreaseVolume());
    }

    IEnumerator DecreaseVolume()
    {
        yield return new WaitForSeconds(delayBeforeStart);
        audioSource.Play();
        float startVolume = audioSource.volume;
        float time = 0f;

        while (time < decreaseDuration)
        {
            time += Time.deltaTime;

            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, time / decreaseDuration);
            yield return null;
        }


        audioSource.volume = targetVolume;
    }
}