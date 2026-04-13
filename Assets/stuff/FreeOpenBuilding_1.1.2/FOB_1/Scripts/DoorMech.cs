using UnityEngine;
using BNG;

public class DoorMech : MonoBehaviour
{
    public Vector3 OpenRotation, CloseRotation;
    public float rotSpeed = 3f;
    public bool doorBool;

    [Header("Inventory Check")]
    public InventoryFullChecker inventoryChecker; // Drag player object here

    void Start()
    {
        doorBool = false;
        CloseRotation = transform.eulerAngles;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            // Only open if inventory is full
            if (inventoryChecker != null && inventoryChecker.IsInventoryFull())
            {
                doorBool = true;
                Debug.Log("Inventory full — Door opening!");
            }
            else
            {
                Debug.Log("Inventory not full — Door locked!");
            }
        }
    }

    void Update()
    {
        Quaternion targetRotation = doorBool
            ? Quaternion.Euler(OpenRotation)
            : Quaternion.Euler(CloseRotation);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotSpeed * Time.deltaTime);
    }
}