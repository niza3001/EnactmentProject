#pragma strict
 @script RequireComponent (GUITexture) 
 @script RequireComponent (AudioSource)

public var customSkin:GUISkin;
private var stateOfStory :String;
private var begin:boolean = false;

public var counterStyle:GUIStyle;

public var characterGrid : int =0; 
private var rigidBodyGrid : int =0; 
private var selectionGrid : int =0; 
private var backgroundGrid :int = 0; 
private var recordingState : int; //if 0->Recording has not started, if 1 it is going On(After Countdown) ; if 2 Recording is complete. 


private var contentArray = new Array(); 
private var builtInArray : String[];

function Start()
{		
	recordingState = 0; 
	
	stateOfStory= PlayerPrefs.GetString("stateOfStory");
	if(stateOfStory == "Beginning")
	{
		begin =true; 
		contentArray = PlayerPrefsX.GetStringArray("beginTabArray");
		
	}	

	//Get the selected RigidBody and character that you need to instantiate things 
	characterGrid = PlayerPrefs.GetInt("characterGrid");
	rigidBodyGrid = PlayerPrefs.GetInt("rigidBodyGrid"); 
	selectionGrid = PlayerPrefs.GetInt("selectionGrid");		
	backgroundGrid = PlayerPrefs.GetInt("backgroundGrid");
	//Debug.Log(characterGrid.ToString());
	
}

private var secondsLeft:float = 2f;
private var secondsLeftInt:int = 2;
private var timerCountdown:boolean = false;

function OnGUI()
{	
	GUI.skin = customSkin;
	if(recordingState == 0) 
		createMenu("Press SpaceBar to Start/Stop recording");
	else if(recordingState == 2)
		createMenu("Click on one of these options");
	 
	createStageState8();
	if(timerCountdown)
	{
		  if( secondsLeft > 0)
		  {
		  	secondsLeftInt = Mathf.Round(secondsLeft);
		  	GUILayout.BeginArea(Rect(Screen.width/2-500,Screen.height*0.1,1000,150));
      		GUILayout.Label("Recording . . . ", counterStyle);
      		GUILayout.EndArea();
      	  }
   		 else
   		 {
      		//Trigger AV Pro here :D 
      		timerCountdown = false;		
      		recordingState = 1; 
      		
      	} 
	}
}

//private var k2:float = k*0.3; 
private var temp:Vector3 ; 
function FixedUpdate()
{
	//temp = GameObject.Find("Dummy").transform.position;
	//if(characterHeadInst && characterBodyInst && characterRightHandInst && characterLeftHandInst && characterRightLegInst)
	//	Debug.Log("ALL Instantiated");
	if(timerCountdown)
	{
		secondsLeft -= Time.deltaTime;
	}
	if(recordingState <= 1) 
	{
				
		//ROTATION FIX
		//characterTorsoInst.transform.localEulerAngles.y = 0;
		if(Input.GetKeyDown("space") && spaceBarPressedFirst)
		{
			recordingState = 2; 
			spaceBarPressedSecond = true;
		}
		
	}
}
private var MenuPercent: float = 0.1;
private var tellYourStoryPrecent = 1.0;

function createMenu(myString : String)
{
	 
GUILayout.BeginArea(Rect(0,0,Screen.width,MenuPercent*Screen.height));
	GUILayout.BeginHorizontal();
		GUILayout.Label(myString, counterStyle, GUILayout.Width(tellYourStoryPrecent*Screen.width), GUILayout.Height(Screen.height*MenuPercent));
	GUILayout.EndHorizontal();
GUILayout.EndArea();	
}


private var spaceBarPressedFirst:boolean ;
private var spaceBarPressedSecond:boolean;


private var s1 : String ; 
private var s2 : String ;
public var BGPlaneTexture: Texture;
var videoFileName : String ;


private var BGPlane : GameObject; 
public var bgPic : Texture2D;
function createStageState8()
{
	BGPlane = GameObject.Find("BGPlane");
	if(recordingState == 0)
	{
		if(!spaceBarPressedFirst)
		{
			if(GUI.Button(Rect(Screen.width-240,Screen.height-90,120,60),"back", GUI.skin.FindStyle("back")))
			{
				Application.LoadLevel(5);
			}
		}
		if(Input.GetKeyDown("space") && !spaceBarPressedFirst )
		{
			spaceBarPressedFirst = true; 
			timerCountdown = true; 
		}	
	}
	else if(recordingState == 2 && spaceBarPressedSecond) 
	{
			//GUI.Label(Rect(0,0,Screen.width,Screen.height),bgPic);
			s1  = PlayerPrefs.GetString("storyTeller1"); 
			s2 = PlayerPrefs.GetString("storyTeller2");
			//Playback the recorded video 
			//videoFileName = Application.dataPath+"/Resources/Videos/"+s1+s2+"/"+selectionGrid.ToString();	

			//Debug.Log("FILE NAME IS :"+videoFileName);
			

		if(GUI.Button(Rect(0.5*Screen.width-150,Screen.height*0.9,120,60),"", GUI.skin.FindStyle("review")))
		{
			//var vp : AVProWindowsMediaManager = GetComponent("AVProWindowsMediaManager");
			
			
		}
		if(GUI.Button(Rect(Screen.width/2,Screen.height*0.9,120,60),"", GUI.skin.FindStyle("redo")))
		{
			//BGPlane.renderer.material.mainTexture = BGPlaneTexture;
			//recordingState = 0 ;  
			//assetRefreshDone = false; 
			recordingState = 0; 
			spaceBarPressedFirst = false; 
			spaceBarPressedSecond = false; 
			Application.LoadLevel(7);
		}
		if(GUI.Button(Rect(0.5*Screen.width+150,Screen.height*0.9,120,60),"Done", GUI.skin.FindStyle("done")))
		{
			
			Application.LoadLevel(5);
			
		}		
		
	}
}