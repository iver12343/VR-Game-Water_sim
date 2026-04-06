using UnityEngine;

public class DestinationTrigger : MonoBehaviour
{
    [Header("Particle")]
    public ParticleSystem destinationParticle;  // Drag particle here

    [Header("UI")]
    public GameObject uiMessage;                // Drag UI GameObject here

    private void Start()
    {
        // Make sure UI is hidden at start
        if (uiMessage != null) uiMessage.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Turn off particle
            if (destinationParticle != null && destinationParticle.isPlaying)
                destinationParticle.Stop();

            // Turn on UI
            if (uiMessage != null)
                uiMessage.SetActive(true);

            Debug.Log("Player reached destination — particle off, UI on!");
        }
    }
}