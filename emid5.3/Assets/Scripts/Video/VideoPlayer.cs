using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VideoPlayer : MonoBehaviour {

    private AVProWindowsMediaMovie wmm;
    private AVProWindowsMediaPlayVideoDemo wmpvd;
    private int selectionGridInt;

    private List<string> BIGList;
    private GameObject myGameManager;
    private int noOfTabs;
    public GUISkin customSkin; 

	// Use this for initialization
	void Start () 
    {
        
         wmm = GetComponent<AVProWindowsMediaMovie>();
        //UI for the Video with all the controls 
        wmpvd = GetComponent<AVProWindowsMediaPlayVideoDemo>();
        wmpvd.enabled = false;


        selectionGridInt = 1; 
        selectionGridInt = PlayerPrefs.GetInt("selectionGrid");
        
        myGameManager = GameObject.FindGameObjectWithTag("GameManager") as GameObject;
        noOfTabs = (int)myGameManager.GetComponent<GameManager>().dd.numberOfTabs;
        BIGList = myGameManager.GetComponent<GameManager>().dd.BIGList;
        
        wmm.enabled = true;
        wmm._filename = BIGList[selectionGridInt];
       // wmm._filename = "C:\\Users\\Kumar\\Documents\\Dime\\Assets\\MasterGameData\\s1859\\Videos\\0.avi";

        wmm._loadOnStart = true;
        wmm._playOnStart = true;
        wmm.LoadMovie(true);
        //Enable the UI 
        wmpvd.enabled = true; 

	}
    void OnGUI()
    {
        //GUI.skin = customSkin;
        if (GUI.Button(new Rect(Screen.width * 0.05f, Screen.height * 0.88f, Screen.width * 0.1f, Screen.height * 0.1f), "back"))
        {
              Application.LoadLevel(5);
        }
        
        //GUI.Label(new Rect(0f, 0f, Screen.width, Screen.height),"");
    }
	// Update is called once per frame
	void Update () 
    {
	
	}
}
