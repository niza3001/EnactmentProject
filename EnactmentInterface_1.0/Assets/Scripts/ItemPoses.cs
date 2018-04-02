using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPoses : MonoBehaviour {


    public Vector3 pose0;
    public Vector3 pose1;
    public Vector3 pose2;
    public Vector3 pose3;
    public Vector3 pose4;

    public Vector3 groundPose;
    
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public Vector3 getItemPos(int ind, bool ground)
    {
        if (!ground) {
            switch (ind)
            {
                case 0:
                    return pose0;
                case 1:
                    return pose1;
                case 2:
                    return pose2;
                case 3:
                    return pose3;
                case 4:
                    return pose4;
                default:
                    return pose0;

            }
        }
        else { return groundPose; }
    }
}
