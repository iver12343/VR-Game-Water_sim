using UnityEngine;
using BNG;

public class FloodStageTrigger : MonoBehaviour
{
    [Header("References")]
    public MoveToWaypoint moveToWaypoint;

    [Header("Settings")]
    [Tooltip("Tag of the object that should activate this trigger")]
    public string playerTag = "Player";

    private void Awake()
    {
        // Make sure the collider on this object is set to trigger
        Collider col = GetComponent<Collider>();
        if (col != null)
            col.isTrigger = true;
        else
            Debug.LogWarning("FloodStageTrigger: No Collider found on this GameObject. Please add one.");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (FloodController.Instance != null)
            {
                if (FloodController.Instance.IsStage(0))
                {
                    FloodController.Instance.GoToNextStage();
                    Debug.Log("FloodStageTrigger: Was stage 1, moved to stage 2.");
                }
                else
                {
                    Debug.Log($"FloodStageTrigger: Current stage is {FloodController.Instance.CurrentStageNumber}, no stage change.");
                }
            }
            else
            {
                Debug.LogWarning("FloodStageTrigger: FloodController instance not found.");
            }

            // MoveToWaypoint Logic
            if (moveToWaypoint != null)
            {
                moveToWaypoint.reachedDelay = true;
                Debug.Log("FloodStageTrigger: MoveToWaypoint activated.");
            }
            else
            {
                Debug.LogWarning("FloodStageTrigger: MoveToWaypoint reference not assigned.");
            }
        }

        // Flood Stage Logic

    }
}