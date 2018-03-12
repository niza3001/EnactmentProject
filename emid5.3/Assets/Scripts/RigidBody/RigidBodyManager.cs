using UnityEngine;
using System.Collections;


public class RigidBodyManager : MonoBehaviour {


	public GameObject[] rigidBodyMesh;

	void Awake()
	{
		//DontDestroyOnLoad (this);
	}
	// Use this for initialization
	void Start () {
        //Debug.LogError("Start() being called from RigidBodyManager.cs");
    }
	
	// Update is called once per frame
	void Update () {
        //Debug.LogError("rigidBodyManager.cs has updated.");
    }
}
