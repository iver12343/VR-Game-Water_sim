using UnityEngine;
using BNG;
using WrightAngle.Waypoint;

public class InventoryFullChecker : MonoBehaviour
{
    [Header("All 3 Snap Zones (Holsters)")]
    public SnapZone snapZone1;
    public SnapZone snapZone2;
    public SnapZone snapZone3;

    [Header("Items - Link Each Item Directly")]
    public Grabbable item1;   // Drag the actual item GameObject here
    public Grabbable item2;
    public Grabbable item3;

    [Header("Particle Effect Near Door")]
    public ParticleSystem doorParticle;

    [Header("Waypoint System")]
    public WaypointUIManager waypointManager;
    public WaypointTarget doorWaypoint;
    public WaypointTarget itemWaypoint1;
    public WaypointTarget itemWaypoint2;
    public WaypointTarget itemWaypoint3;

    private bool wasFull = false;
    private bool doorWaypointShown = false;
    private bool doorWaypointPermanentlyHidden = false;

    private bool item1Hidden = false;
    private bool item2Hidden = false;
    private bool item3Hidden = false;

    void Update()
    {
        // --- Check each item directly, not snapzone ---
        // Waypoint hides only when ITS specific item is picked up
        CheckItemWaypoint(item1, itemWaypoint1, ref item1Hidden);
        CheckItemWaypoint(item2, itemWaypoint2, ref item2Hidden);
        CheckItemWaypoint(item3, itemWaypoint3, ref item3Hidden);

        bool isFull = IsInventoryFull();

        // --- When inventory becomes full ---
        if (isFull && !wasFull)
        {
            Debug.Log("Inventory Full!");
            StartParticle();

            if (!doorWaypointShown &&
                !doorWaypointPermanentlyHidden &&
                waypointManager != null &&
                doorWaypoint != null)
            {
                waypointManager.Register(doorWaypoint);
                doorWaypointShown = true;
                Debug.Log("Door waypoint activated!");
            }
        }

        // --- When inventory loses an item ---
        if (!isFull && wasFull)
        {
            Debug.Log("Inventory has space again.");
            StopParticle();

            if (doorWaypointShown && !doorWaypointPermanentlyHidden && doorWaypoint != null)
            {
                waypointManager.Unregister(doorWaypoint);
                doorWaypointShown = false;
                Debug.Log("Door waypoint hidden — inventory no longer full.");
            }
        }

        wasFull = isFull;

        CheckDoorWaypointReached();
    }

    /// <summary>
    /// Checks item's grabbed state directly
    /// Hides waypoint when item is held/snapped, restores when dropped
    /// </summary>
    void CheckItemWaypoint(Grabbable item, WaypointTarget waypoint, ref bool isHidden)
    {
        if (item == null || waypoint == null || waypointManager == null) return;

        // Item is being held or snapped into a zone
        bool itemPickedUp = item.BeingHeld || IsItemInAnySnapZone(item);

        if (itemPickedUp && !isHidden)
        {
            // Item picked up — hide its waypoint
            waypointManager.Unregister(waypoint);
            isHidden = true;
            Debug.Log($"Item picked up — hiding waypoint: {waypoint.DisplayName}");
        }
        else if (!itemPickedUp && isHidden)
        {
            // Item dropped — restore its waypoint
            waypointManager.Register(waypoint);
            isHidden = false;
            Debug.Log($"Item dropped — restoring waypoint: {waypoint.DisplayName}");
        }
    }

    /// <summary>
    /// Checks if a specific item is currently snapped in any of the 3 snapzones
    /// </summary>
    bool IsItemInAnySnapZone(Grabbable item)
    {
        if (snapZone1 != null && snapZone1.HeldItem == item.gameObject) return true;
        if (snapZone2 != null && snapZone2.HeldItem == item.gameObject) return true;
        if (snapZone3 != null && snapZone3.HeldItem == item.gameObject) return true;
        return false;
    }

    public bool IsInventoryFull()
    {
        bool slot1 = snapZone1 != null && snapZone1.HeldItem != null;
        bool slot2 = snapZone2 != null && snapZone2.HeldItem != null;
        bool slot3 = snapZone3 != null && snapZone3.HeldItem != null;

        return slot1 && slot2 && slot3;
    }

    void CheckDoorWaypointReached()
    {
        if (doorWaypointPermanentlyHidden || !doorWaypointShown || doorWaypoint == null) return;

        float distance = Vector3.Distance(transform.position, doorWaypoint.TargetPosition);

        if (distance <= 1f)
        {
            waypointManager.Unregister(doorWaypoint);
            doorWaypoint.enabled = false;
            doorWaypointPermanentlyHidden = true;
            doorWaypointShown = false;
            Debug.Log("Player reached door — waypoint permanently hidden!");
        }
    }

    void StartParticle()
    {
        if (doorParticle != null && !doorParticle.isPlaying)
            doorParticle.Play();
    }

    void StopParticle()
    {
        if (doorParticle != null && doorParticle.isPlaying)
            doorParticle.Stop();
    }
}