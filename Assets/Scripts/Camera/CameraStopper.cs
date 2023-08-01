using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStopper : MonoBehaviour {

	public GameObject otherObject;

	void OnTriggerEnter(Collider other)
	{
		Debug.Log(other.tag);
		if (other.tag == "Camera_Midpoint") {
			otherObject = other.transform.parent.gameObject;
			CameraFollow.isFollowing = false; //This variable is set to static, so we can access it from within other scripts without using getComponent.
			gameObject.SetActive (false);
		} else
			return;
	}

}