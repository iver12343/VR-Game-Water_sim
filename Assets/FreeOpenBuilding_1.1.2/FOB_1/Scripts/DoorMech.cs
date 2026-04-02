using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using BNG;

public class DoorMech : MonoBehaviour 
{

	public Vector3 OpenRotation, CloseRotation;

	public float rotSpeed = 1f;

	public bool doorBool;


	void Start()
	{
		doorBool = false;
	}
		
	void OnTriggerStay(Collider col)
	{
        Debug.Log("Something entered: " + col.name);

        if (col.gameObject.tag == ("Player"))
		{
			doorBool = true;
			Debug.Log("door open");
		}
		else
			doorBool = false;
		}
	

	void Update()
	{
		if (doorBool)
			transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.Euler (OpenRotation), rotSpeed * Time.deltaTime);
		else
			transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.Euler (CloseRotation), rotSpeed * Time.deltaTime);	
	}

}

