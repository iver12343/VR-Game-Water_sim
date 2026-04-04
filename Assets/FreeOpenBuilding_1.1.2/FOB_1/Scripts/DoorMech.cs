using UnityEngine;
using BNG;

public class DoorMech : MonoBehaviour
{
    public Vector3 OpenRotation, CloseRotation;
    public float rotSpeed = 3f;
    public bool doorBool;

    void Start()
    {
        doorBool = false;
        CloseRotation = transform.eulerAngles;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player") )
        {
           
            doorBool = true;
            Debug.Log(doorBool);
        }
    }

    //void OnTriggerExit(Collider col)
    //{
    //    if (col.CompareTag("Player"))
    //        doorBool = false;
    //}

    void Update()
    {
        Quaternion targetRotation = doorBool
            ? Quaternion.Euler(OpenRotation)
            : Quaternion.Euler(CloseRotation);

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotSpeed * Time.deltaTime);
    }
}