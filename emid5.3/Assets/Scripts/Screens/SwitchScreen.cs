using UnityEngine;
using System.Collections;

public class SwitchScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnGUI()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            Application.LoadLevel(6);
        }
    }

}
