using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {


    //This script currently just stores the item GameObject's custom scale values for placement in the scene
    public float scale;
    private Vector3[] itemPositions = new Vector3[5];

    // Use this for initialization
    void Start()
    {
        itemPositions[0] = new Vector3(573, 288, 82);
        itemPositions[1] = new Vector3(650, 320, 82);
        itemPositions[2] = new Vector3(570, 234, 82);
        itemPositions[3] = new Vector3(427, 129, 82);
        itemPositions[4] = new Vector3(515, 321, 82);
    }

    public Vector3 getPosition(int i)
    {

        return itemPositions[i];
    }


    public float getScale()
    {

        return scale;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
