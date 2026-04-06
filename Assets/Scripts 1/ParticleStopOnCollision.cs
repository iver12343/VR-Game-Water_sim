using UnityEngine;

public class ParticleStopOnCollision : MonoBehaviour
{
    private ParticleSystem particle;
    private InventoryFullChecker inventoryChecker; // Reference to reset state

    void Start()
    {
        particle = GetComponent<ParticleSystem>();
        // Find the inventory checker on the player
        inventoryChecker = FindObjectOfType<InventoryFullChecker>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if player collided with particle
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player reached door particle — stopping effect!");
            StopParticleAndReset();
        }
    }

    void StopParticleAndReset()
    {
        // Stop the particle
        if (particle != null && particle.isPlaying)
            particle.Stop();
    }
}