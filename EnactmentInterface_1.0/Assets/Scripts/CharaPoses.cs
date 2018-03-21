using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaPoses : MonoBehaviour {

    //this class currently just stores the sprites for all the character poses for a character
    public Sprite[] poses = new Sprite[5];
    private Vector3[] itemPositions = new Vector3[5];
    
	// Use this for initialization
	void Start () {
        itemPositions[0] = new Vector3(0, 0, 0);
        itemPositions[1] = new Vector3(0, 0, 0);
        itemPositions[2] = new Vector3(0, 0, 0);
        itemPositions[3] = new Vector3(0, 0, 0);
        itemPositions[4] = new Vector3(0, 0, 0);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
