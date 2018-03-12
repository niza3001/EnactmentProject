using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.IO; 

public enum Screen8State
{
	Start, 
	RecordingInProgress, 
	RecordingFinished, 
	ReviewInProgress
}

public class Screen8UI : MonoBehaviour {

	public GUISkin customSkin; 
	public Texture2D videoPlayerTexture;
	private GameObject rigidBodyObject; 
	private AVProWindowsMediaMovie wmm; 
	private AVProWindowsMediaPlayVideoDemo wmpvd;
	private int spaceBarCounter = 0;
	private String[] tempArray = new String[10];
	private int selectionGridInt ;
	//private Screen5 myScreen5;
	public bool testingPhase; 

	private List<string> BIGList;
	private GameObject myGameManager ;
	private int noOfTabs;
	private Screen8State currentState;
    private float startTime;
    private GameObject myVideoPlayer; 

    private bool  gotArrayAlready; 
    
	// Use this for initialization
	void Start () 
	{
        
		spaceBarCounter = 0;
		currentState = Screen8State.Start;

		rigidBodyObject = (GameObject)GameObject.Find("RigidBodyObject");
		if(rigidBodyObject != null)
            rigidBodyObject.SetActive (true);

		 
		selectionGridInt = PlayerPrefs.GetInt("selectionGrid");

		myGameManager = GameObject.FindGameObjectWithTag("GameManager") as GameObject;
        myVideoPlayer = GameObject.FindGameObjectWithTag("VideoPlayer") as GameObject;
        wmm = GetComponent<AVProWindowsMediaMovie>();
        wmpvd = GetComponent<AVProWindowsMediaPlayVideoDemo>();
        wmm.enabled = false;
        wmpvd.enabled = false; 
        //myVideoPlayer.SetActive(false); 
        tempArray = PlayerPrefsX.GetStringArray("beginTabArray");
        if (Application.isEditor == false)
            testingPhase = false; 
		if(testingPhase) //Create GameManager variables in Scene itself 
		{
			BIGList = new List<string> ();
			for(int i =0; i <16; i++)
			{
				BIGList.Add("");
			}
			noOfTabs = 16;
			
		}
		else 
		{
			noOfTabs =(int)myGameManager.GetComponent<GameManager> ().dd.numberOfTabs;
			BIGList = myGameManager.GetComponent<GameManager> ().dd.BIGList;
		}
	}
    private bool tookScreenShot;
    private bool loadedMovie = false;   
	void OnGUI()
	{
		GUI.skin = customSkin;
		//Debug.Log (spaceBarCounter);
		if(spaceBarCounter == 0)//Recording Not started Yet 
		{
            tookScreenShot = false; 
			if(GUI.Button(new Rect(Screen.width*0.1f,Screen.height-90.0f,120.0f,60.0f),"", GUI.skin.FindStyle("back")))
			{
				Application.LoadLevel(5);
			}
            string topText = "Press SpaceBar to Start/Stop recording";
            if(myGameManager.GetComponent<RUISInputManager>().enableKinect2 && myGameManager.GetComponent<RUISInputManager>().enablePSMove)
            {
                topText += "\nPress C to recalibrate Kinect and Move"; 
            }

            GUI.Label(new Rect(0.2f * Screen.width, Screen.height*0.05f, Screen.width * 0.6f, Screen.height * 0.15f), topText, GUI.skin.FindStyle("counterStyle"));
            
		}
		else if(spaceBarCounter == 1)//Recording in Progress
		{
			currentState = Screen8State.RecordingInProgress;
            GUI.Label(new Rect(0.2f * Screen.width, Screen.height * 0.05f, Screen.width * 0.6f, Screen.height * 0.15f), "Recording . . . ", GUI.skin.FindStyle("counterStyle"));
            if (!tookScreenShot)
            {
                //TAKE SCREENSHOT OF THE SCREEN 
                
                string _frameFolder = PlayerPrefs.GetString("frameFolder");
               
                ScreenCapture sc = new ScreenCapture();
                sc.SaveScreenshot(CaptureMethod.RenderToTex_Synch, _frameFolder +selectionGridInt.ToString()+ ".png");
                tookScreenShot = true; 
            }
		}
		else // Review redo Screen  
		{
			//wmm.LoadMovie (true);

            if (!gotArrayAlready)
            {
                tempArray = PlayerPrefsX.GetStringArray("beginTabArray");
                gotArrayAlready = true; 
            }
            BIGList[selectionGridInt] = tempArray[selectionGridInt];

			//GUI.Label(new Rect(0.0f,0.0f,Screen.width,Screen.height*0.15f),"Click on one of these options",GUI.skin.FindStyle("counterStyle"));
			if(GUI.Button(new Rect(0.5f*Screen.width-250.0f,Screen.height*0.9f,120.0f,60.0f),"", GUI.skin.FindStyle("review")))
			{
                //myVideoPlayer.SetActive(true);
                if (!loadedMovie)
                {

                    wmm.enabled = true;
                    wmpvd.enabled = true; 
                    //UI for the Video with all the controls 
                    currentState = Screen8State.ReviewInProgress;
                    
                    wmm._filename = tempArray[selectionGridInt];
                    wmm._loadOnStart = true;
                    wmm._playOnStart = true;
                    wmm.LoadMovie(true);
                    wmm._loop = true;
                    wmm.enabled = true;
                    wmpvd.enabled = true;
                    
               
                    loadedMovie = true; 
                }               
				 
				//Enable the UI 
				//wmpvd.enabled = true; 
				GUI.Label(new Rect(0f,0f,Screen.width,Screen.height),videoPlayerTexture);
								
			}
			if(GUI.Button(new Rect(Screen.width*0.5f,Screen.height*0.9f,120.0f,60.0f),"", GUI.skin.FindStyle("redo")))
			{
				//BGPlane.renderer.material.mainTexture = BGPlaneTexture;
				//recordingState = 0 ;  
				//assetRefreshDone = false; 
				//spaceBarCounter = 0;

				Application.LoadLevel(Application.loadedLevel);
			}
			if(GUI.Button(new Rect(0.5f*Screen.width+250.0f,Screen.height*0.9f,120.0f,60.0f),"Done", GUI.skin.FindStyle("done")))
			{
                //SAVE the Video location on BigList so that it can be accessed in Screen 5 
               if(!testingPhase)
                    myGameManager.GetComponent<GameManager>().dd.BIGList = BIGList; 
                

              //  StartCoroutine(WaitAndPrint(0.1f)); //AND END FUNCTION 
                Application.LoadLevel(5);
				
			}		
		}
	}
    IEnumerator WaitAndPrint(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        print("WaitAndPrint " + Time.time);
        
    }
    
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			spaceBarCounter++;
            startTime = Time.time;
		}
        if(Input.GetKeyDown(KeyCode.C))
        {
            Application.LoadLevel(11);
        }
   
	}
}
