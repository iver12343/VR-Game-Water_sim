using UnityEngine;

public class HelicopterController : MonoBehaviour
{
    [Header("Blade Settings")]
    public Transform mainBlade;     // Assign main rotor
    public Transform tailBlade;     // Assign tail rotor
    public float mainBladeSpeed = 1000f;
    public float tailBladeSpeed = 1500f;

    [Header("Audio Settings")]
    public AudioSource helicopterAudio;

    void Start()
    {
        // Play sound when game starts
        if (helicopterAudio != null)
        {
            helicopterAudio.loop = true;
            helicopterAudio.Play();
        }
    }

    void Update()
    {
        // Rotate main blade (usually Y-axis)
        if (mainBlade != null)
        {
            mainBlade.Rotate(Vector3.up * mainBladeSpeed * Time.deltaTime);
        }

        // Rotate tail blade (usually X-axis)
        if (tailBlade != null)
        {
            tailBlade.Rotate(Vector3.right * tailBladeSpeed * Time.deltaTime);
        }
    }
}