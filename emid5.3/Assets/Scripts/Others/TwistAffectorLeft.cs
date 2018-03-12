using UnityEngine;
using System.Collections;

public class TwistAffectorLeft : MonoBehaviour {
	private GameObject leftUpperArm;
	Vector3 pos;
	// Use this for initialization
	void Start () {
		leftUpperArm = GameObject.FindGameObjectWithTag("LeftLowerArm"); 
		  

	}
	
	// Update is called once per frame
	void Update () {
		pos = leftUpperArm.GetComponent<Transform> ().position; 
		pos.z = -1.0f; 
		transform.position = pos; 
	
	}
}
