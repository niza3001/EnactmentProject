using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic; 
using System.IO;
using System.Xml;
using System.Xml.Serialization; 

public class Screen5Modular : MonoBehaviour {
	//GUI Elements and Textures  
	public bool testingPhase; 
	public GUISkin customSkin;
	public Texture2D bgTexture ;
	public Texture2D dimeLogo ;
	public Texture2D blankTabTexture;
	public Texture2D textFilledTabTexture;
	public Texture2D videoTabTexture ; 
	public Texture2D marginTexture; 
	public Texture2D emptyTabTexture; 
	
	//Arrays 
    private GameManager gm; 
	private GameObject myGameManager ;
	private List<string> BIGList; 
	private GUIContent[] tabContents; 
	private int noOfTabs = 0;
	//private var totalNumberOfTabs:int = 8; //This is the maximum number of tabs which can be changed 
	
	private StageState currentStageState;  
	private string currentString = "";
	private string tempString;

	//Switch Margin Buttons On or Off 
	private bool isWriteButtonOn;
	private bool isVideoButtonOn;
	private	bool isPictureButtonOn; 
	private	bool isEnactButtonOn;
	private	bool alertBoxOn;
	private bool userTypingFlag;
	private bool isViewMyStoryOn; 
	
	//PlayerPref Variables 
	private string s1;
	private string s2;  
	private string storyTitle = "";
    private string dimeFile; 
    private string _frameFolder;
    private string FileBrowserPath; 

	private int selGridInt = 0; 

	private enum StageState 
	{
		Normal,
		WritingText, 
		SelectingCharacter, 
        CustomizingCharacter,
		SelectingObject, 
		SelectingBackGround, 
		PickingPhysicalObject, 
		PlayingVideo
	}
    void SerializeGameManagerToXml()
    {
        try
        {
            myGameManager.GetComponent<GameManager>().dd.s1 = PlayerPrefs.GetString("storyTeller1");
            myGameManager.GetComponent<GameManager>().dd.s2 = PlayerPrefs.GetString("storyTeller2");
            myGameManager.GetComponent<GameManager>().dd.storyTitle = PlayerPrefs.GetString("storyTitle");
            myGameManager.GetComponent<GameManager>().dd.dimeFile = PlayerPrefs.GetString("dimeFile");
            myGameManager.GetComponent<GameManager>().dd.dimeFolder = PlayerPrefs.GetString("dimeFolder"); 


            XmlSerializer serializer = new XmlSerializer(typeof(DimeData));
            FileStream fileStream = File.Open(dimeFile, FileMode.OpenOrCreate);
            serializer.Serialize(fileStream, (DimeData)myGameManager.GetComponent<GameManager>().dd);            
            fileStream.Close();
        }
        catch (Exception ex)
        {
            Debug.Log("Exception during Serializing File:"+dimeFile);
            Debug.Log(ex); 
        }
    }
    private void DeserializeXml(string FileBrowserPath)
    {
        try
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(DimeData));
            FileStream fileStream = File.Open(FileBrowserPath, FileMode.Open);
            myGameManager.GetComponent<GameManager>().dd = (DimeData)deserializer.Deserialize(fileStream);
            fileStream.Close();

            PlayerPrefs.SetString("storyTeller1", myGameManager.GetComponent<GameManager>().dd.s1);
            PlayerPrefs.SetString("storyTeller2", myGameManager.GetComponent<GameManager>().dd.s2);
            PlayerPrefs.SetString("storyTitle", myGameManager.GetComponent<GameManager>().dd.storyTitle);
            PlayerPrefs.SetString("dimeFile", myGameManager.GetComponent<GameManager>().dd.dimeFile);
            PlayerPrefs.SetString("dimeFolder", myGameManager.GetComponent<GameManager>().dd.dimeFolder); 

            //You don't wanna Deserialize EVERY Time so set FileBrowserPath to "" 
            PlayerPrefs.SetString("FileBrowserPath", ""); 
        }
        catch (Exception ex)
        {
            Debug.Log("Exception during DESerializing File:" + dimeFile);
            Debug.Log(ex);
        }
    }
    #region Start 
    // Use this for initialization
	void Start () 
	{
        //myGameManager.GetComponent<GameManager>().xmlDataList.rigidBodyList[0].
        currentStageState = StageState.Normal; 
		alertBoxOn = false; 

		isWriteButtonOn = true;
		isPictureButtonOn=true; 
		isEnactButtonOn = true;
		isViewMyStoryOn = false;

        //PlayerPrefs STrings from previous screens 
        storyTitle = PlayerPrefs.GetString("storyTitle");
        s1 = PlayerPrefs.GetString("storyTeller1");
        s2 = PlayerPrefs.GetString("storyTeller2");
        dimeFile = PlayerPrefs.GetString("dimeFile");
        _frameFolder = PlayerPrefs.GetString("frameFolder");
        FileBrowserPath = PlayerPrefs.GetString("FileBrowserPath"); 


		myGameManager = GameObject.FindGameObjectWithTag("GameManager") as GameObject;
       // gm = myGameManager.GetComponent<GameManager>();

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
            if (FileBrowserPath != "")
            {
                DeserializeXml(FileBrowserPath);
            }
            noOfTabs = (int)myGameManager.GetComponent<GameManager>().dd.numberOfTabs;
            BIGList = myGameManager.GetComponent<GameManager>().dd.BIGList;
            
		}
		

		selGridInt = countWhichTabIsBlank();	 

		if(BIGList[selGridInt]=="")
		{
			currentStageState = StageState.Normal;
		}
		else 
		{
			currentStageState = StageState.WritingText; 
		}

		makeTabContents ();
        //StartCoroutine(WaitAndPrint(1.0f));
		//Initializing some Rectangles 
		stageRect = new Rect (Screen.width * beginStagePosX, Screen.height * beginStagePosY, Screen.width * stageWidth, Screen.height * stageHeight);
        SerializeGameManagerToXml();
	}


    #endregion


    // Update is called once per frame
	void Update () {
		
	}

	void OnGUI()
	{
		GUI.skin = customSkin; 

		createMenu (); 
		createTabs (); 
		createMargin (); 

		switch(currentStageState)
		{
			case StageState.Normal : createStageNormal(); 
				break; 
			case StageState.WritingText : createStageWritingText(); 
				break; 
			case StageState.SelectingCharacter : createStageSelecting();
				break;
            case StageState.CustomizingCharacter: createStageSelecting();
                break;
			case StageState.SelectingObject : createStageSelecting();
				break;
			case StageState.SelectingBackGround : createStageSelecting();
				break;
			case StageState.PickingPhysicalObject : createStageSelecting();
				break;
	
		}
	}

    #region OnGUI Helper Functions 
    /******************OnGUI Helper Functions ****************/ 
    private float MenuPercent = 0.05f;


	void createMenu() 
	{
		GUI.Label(new Rect(0, 0, Screen.width*0.1f ,Screen.height*MenuPercent),dimeLogo);
		GUI.Label(new Rect(Screen.width*0.2f, 0, Screen.width*0.15f ,Screen.height*MenuPercent),"Story Title", GUI.skin.FindStyle("storyTitle"));		
		storyTitle = GUI.TextField(new Rect(Screen.width*0.35f, 0, Screen.width*0.5f ,Screen.height*MenuPercent), storyTitle, 48);
	}

	
	private float tabHeight = Screen.height *0.13f;
	private float tabWidth = Screen.width*0.18f; 

	//private float newWidth = 0.0f;
	//HARD Coding 3.6 * Screen.width 
	private Rect Rect1 = new Rect(0, 0.07f*Screen.height, 0.9f*Screen.width, 0.17f*Screen.height);
	private Rect Rect2 = new Rect(0, 0.05f*Screen.height, Screen.width*3.6f , 0.13f*Screen.height);
	private Vector2 scrollPosition = Vector2.zero; 
	private int currentIndex = 0;
	public Texture2D rightArrow;//Array holding all textures 
	public Texture2D filmStrip; 
	private string tempString1 = "";
	private Rect toolTipRect = new Rect(0.9f*Screen.width,0.14f*Screen.height, 0.1f*Screen.width,0.2f*Screen.height);
	private Rect filmStripRect = new Rect(0, 0, 0.9f*Screen.width, 0.27f*Screen.height);

	void createTabs()
	{

		//TRASH BUTTON 

			
		if(GUI.Button (new Rect (Screen.width*0.925f,0.05f*Screen.height,tabWidth*0.3f,tabHeight*0.5f), new GUIContent("",""),GUI.skin.FindStyle("trash")))// 2nd argument is tooltip Text
		{
            if (isVideoTab(BIGList[selGridInt]))
            {
                HandleVideoDelete();
            }
			BIGList.RemoveAt(selGridInt);
			BIGList.Add("");//add a blank frame at the end 
			currentString = BIGList[selGridInt];
			makeTabContents();
					
		}
		//LEFT Button 
		if(GUI.Button (new Rect (Screen.width*0.92f,0.05f*Screen.height+tabHeight*0.5f,tabWidth*0.15f,tabHeight*0.5f),new GUIContent("",""),GUI.skin.FindStyle("leftArrow")))
		{
			if(selGridInt != 0)
			{
                if (isVideoTab(BIGList[selGridInt]) && isVideoTab(BIGList[selGridInt+1]))
                {
                    
                }
				//Swap Two Tabs 
				tempString1 = BIGList[selGridInt];
				BIGList[selGridInt] = BIGList[selGridInt-1]; 
				BIGList[selGridInt-1] = tempString1; 
				
				currentString = BIGList[selGridInt];
				currentString = BIGList[selGridInt-1];
								
				selGridInt--; 
				makeTabContents();
				
			}
		}

		//RIGHT Button
		if(GUI.Button (new Rect (Screen.width*0.93f+tabWidth*0.15f,(0.05f*Screen.height+tabHeight*0.5f),tabWidth*0.15f,tabHeight*0.5f), new GUIContent("",""),GUI.skin.FindStyle("rightArrow")))
		{
			if(selGridInt != noOfTabs-1)
			{
				tempString1 = BIGList[selGridInt];
				BIGList[selGridInt] = BIGList[selGridInt+1]; 
				BIGList[selGridInt+1] = tempString1; 
				
				currentString = BIGList[selGridInt];
				currentString = BIGList[selGridInt+1];

				selGridInt++; 
				makeTabContents();
				
			}
		}

		GUI.Box (toolTipRect, GUI.tooltip,GUI.skin.FindStyle("ToolTipStyle"));

		//ACTUAL TABS 
		GUI.Label(filmStripRect, filmStrip);
		scrollPosition = GUI.BeginScrollView(Rect1,scrollPosition,Rect2);
		selGridInt = GUI.SelectionGrid (new Rect (0.0f,0.05f*Screen.height , Screen.width*3.6f, tabHeight), selGridInt, tabContents, noOfTabs, GUI.skin.GetStyle("item"));
		GUI.EndScrollView();
			

		//THE BOX IN WHICH TOOLTIP is displayed. 
		GUI.Box (toolTipRect, GUI.tooltip,GUI.skin.FindStyle("ToolTipStyle"));

	}
	private float beginPosX = 0.02f;
	private float beginPosY = 0.4f;
	private float buttonWidth = 0.16f;
	private float buttonHeight = 0.09f; 
	private float offsetY = 0.12f; 

	private string filename;

	void createMargin()
	{

		if(currentStageState == StageState.Normal)
		{
            if (BIGList[selGridInt] == "")
            {
                //BLANK FRAME 
                isEnactButtonOn = isPictureButtonOn = isWriteButtonOn = true;
                isVideoButtonOn = false;
                //isHomeButtonOn = false;
            }
            else if(isVideoTab(BIGList[selGridInt]))
            {
                //VIDEO FRAME 
                isEnactButtonOn	= isPictureButtonOn = isWriteButtonOn = false;
			    isVideoButtonOn  = true;
            }
            else
            {
                //TEXT FRAME 
                isWriteButtonOn = true;
                isEnactButtonOn = isPictureButtonOn = isVideoButtonOn = false;

            }

		}
        
		else if (currentStageState == StageState.WritingText)
		{
            isEnactButtonOn = isPictureButtonOn = isViewMyStoryOn = isVideoButtonOn = false;
			 
		}
		else //if ( currentStageState == StageState.PickingPhysicalObject || currentStageState == StageState.SelectingCharacter || currentStageState == StageState.SelectingBackGround || currentStageState == StageState.SelectingObject ) 
		{
            isPictureButtonOn = isWriteButtonOn = isViewMyStoryOn = isVideoButtonOn = false;
		}/*
		if(BIGList[selGridInt] != "" && BIGList[selGridInt] != "PPP" && BIGList[selGridInt] != "VideoFrame")
		{
			//Already data entered. So disable pic and enact 
			isEnactButtonOn	= isPictureButtonOn = false;
		}*/

		//
		if(isVideoButtonOn)
		{
			if(GUI.Button(new Rect(beginPosX*Screen.width, beginPosY*Screen.height, buttonWidth*Screen.width, buttonHeight*Screen.height), "View video", GUI.skin.FindStyle("write")))
			{				
				currentStageState = StageState.PlayingVideo;
                SaveMyParameters();
                Application.LoadLevel(8);
				//currentIndex = selGridInt;
				//currentString = BIGList[selGridInt];				
			}
		}	
		if(isWriteButtonOn)
		{
			if(GUI.Button(new Rect(beginPosX*Screen.width, beginPosY*Screen.height, buttonWidth*Screen.width, buttonHeight*Screen.height), "Write", GUI.skin.FindStyle("write")))
			{
				
				currentStageState = StageState.WritingText; 
				currentIndex = selGridInt;
				currentString = BIGList[selGridInt];
				
			}
		}

		if(isEnactButtonOn)
		{
			if(GUI.Button(new Rect(beginPosX*Screen.width, (beginPosY*Screen.height)+(offsetY*Screen.height), buttonWidth*Screen.width, buttonHeight*Screen.height), "Enact", GUI.skin.FindStyle("enact")))
			{
				currentStageState = StageState.SelectingCharacter; 
				currentIndex = selGridInt;

			}
		}

		anyFrameFilled();
		if(isViewMyStoryOn && currentStageState == StageState.Normal)
		{
			if(GUI.Button(new Rect(beginPosX*Screen.width, (0.8f*Screen.height), 1.1f*buttonWidth*Screen.width, 1.3f*buttonHeight*Screen.height), "View my story", GUI.skin.FindStyle("home")))
			{
				//Save all needed info to the Disc 
				if(!testingPhase)
				myGameManager.GetComponent<GameManager> ().dd.BIGList = BIGList; 
				PlayerPrefs.SetString("storyTitle",storyTitle);
				PlayerPrefs.Save();
				Application.LoadLevel(7);
			}
		}
	}
	/*************Stage Creating GUI Functions***************/ 
	
	private float beginStagePosX  = 0.2f;
	private float beginStagePosY  = 0.25f;
	private float stageWidth = 0.65f;
	private float stageHeight = 0.6f; 

	private Rect stageRect ;
	private string question = "Do you want to Write or Enact? (Choose on the left)";


	void createStageNormal()//Just display the Question in Screen 5 
	{

		isEnactButtonOn = isPictureButtonOn = isWriteButtonOn = true; 
		if(BIGList[selGridInt] == "" )
			GUI.Label(stageRect,question,GUI.skin.GetStyle("Screen5StageStyle"));
        else if (isVideoTab(BIGList[selGridInt]))
			GUI.Label(stageRect,"Click View button to play the video",GUI.skin.GetStyle("Screen5StageStyle"));
		else 
			GUI.Label(stageRect,"Click Write button to edit the text",GUI.skin.GetStyle("Screen5StageStyle"));

        GUILayout.BeginArea(new Rect(0f, Screen.height * 0.91f, Screen.width*0.8f, Screen.height * 0.09f));
        GUILayout.BeginHorizontal();
        GUILayout.Label("Storyteller 1", GUI.skin.GetStyle("storyTitle"));
        s1 = GUILayout.TextField(s1, 25, GUILayout.Width(Screen.width*0.2f));
        GUILayout.Label("Storyteller 2", GUI.skin.GetStyle("storyTitle"));
        s2 = GUILayout.TextField(s2, 25, GUILayout.Width(Screen.width * 0.2f));
        GUILayout.EndHorizontal();
        GUILayout.EndArea();


	}


	private float alertBoxWidth = 480.0f; 
	private float alertBoxHeight = 270.0f;
	public Texture2D bgTextureAlert ;
	void createStageWritingText()
	{

		//COMPARE selGridInt and currentIndex ?? 
		if(currentIndex != selGridInt)//  user clicked another tab in between WRITING!! currentIndex is set when the ENACT/Picture/Write button is pressed 
		{
			alertBoxOn = true; 
		}	 
		if(alertBoxOn)
		{
			GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height), bgTexture, ScaleMode.StretchToFill);
			GUI.Label(new Rect((Screen.width*0.5f)-(alertBoxWidth*0.5f), (Screen.height*0.5f)-(alertBoxHeight*0.5f),alertBoxWidth,alertBoxHeight),bgTextureAlert);
			
			GUI.Label(new Rect(Screen.width*0.2f,Screen.height*0.3f,Screen.width*0.6f, Screen.height*0.2f),"You must click Done before clicking another Frame",GUI.skin.FindStyle("alertStyle"));
			
			if(GUI.Button(new Rect(Screen.width*0.4f,Screen.height*0.5f,Screen.width*0.1f, Screen.height*0.1f),"OK", GUI.skin.FindStyle("home")))
			{
				alertBoxOn = false;
				selGridInt = currentIndex;
			}
			
		}
		
		if(userTypingFlag)//If you dont use the userTypingFlag the content of currentString will always be the SAME while typing the text !! 
		{
			if(BIGList[selGridInt] != "")
			{
				currentString = BIGList[selGridInt];
				userTypingFlag = false;
			}
		}
		currentString = GUI.TextArea(stageRect, currentString, 255); 
		if(GUI.Button(new Rect(Screen.width*0.8f, Screen.height*0.9f,buttonWidth*Screen.width, buttonHeight*Screen.height), "Done", GUI.skin.FindStyle("done")) && alertBoxOn == false)
		{
			//Save all the strings to disk based on stateOfStory and selectiongrid 
			BIGList[selGridInt] = currentString; 
			//saveMyArray(selGridInt);	
			selGridInt++; 
			isPictureButtonOn = true;
			isEnactButtonOn = true;
			currentStageState = StageState.Normal;
			makeTabContents();
		}
	}


	/*private var selectingRigidBody : boolean;
	private var selectingCharacter : boolean; 
	private var pickingObject : boolean ; */
	int characterGrid = 0;
	int rigidBodyGrid  = 0; 
	int backgroundGrid = 0; 

	public Texture2D[] characterImages; 
	//public Texture2D[] rigidBodyImages ; 
	//public Texture2D[] backgroundImages ; 
	public Texture2D[] physicalObjectImages ; 
	
	public Texture2D purpleBox; //PurpleBox with white bg 
	int physicalObjectId ; //stores which physical object needs to be picked from the BOX 
	Vector2 scrollPositionRigidBody = Vector2.zero;
    public GameObject KenneyCharacter;
    private bool KenneyAlive; 

	void createStageSelecting()
	{
		Rect selectionRect = new Rect(stageRect.x,1.3f*stageRect.y,stageRect.width*0.8f,0.8f*stageRect.height); // Rectangle for selection grid 
		Rect labelRect = new Rect(stageRect.x, stageRect.y - stageRect.height*0.05f,selectionRect.width, stageRect.height*0.2f); //Rectangle for Label 
		Rect miniStageRect = new Rect(stageRect.xMax-Screen.width*0.05f, selectionRect.yMin, Screen.width*0.2f,selectionRect.height);//Rectangle for the mini stage displaying character 
		

		Rect wideRect = new Rect(selectionRect.x, selectionRect.y, selectionRect.width*1.1f ,selectionRect.height);
		Rect longRect = new Rect(selectionRect.x, selectionRect.y, selectionRect.width, selectionRect.height*2.5f );


		//creates characterGrid rigidBodyGrid 
		if(currentIndex != selGridInt)//  user clicked another tab in between WRITING!! currentIndex is set when the ENACT/Picture/Write button is pressed 
		{
			//set the BIGList from "VideoFrame" to"" and go to satge 5 state. 
			BIGList[selGridInt] = ""; 
			currentStageState = StageState.Normal;
		}	 

		//************Selecting Character ****************
		if(currentStageState == StageState.SelectingCharacter) 
		{ 
			//GUI.Label(stageRect, purpleBox);
			GUI.Label(labelRect, "Choose a character");
            
            characterGrid = GUI.SelectionGrid(selectionRect, characterGrid, characterImages, characterImages.Length, GUI.skin.FindStyle("PropBox"));
			GUI.Label(miniStageRect, characterImages[characterGrid]);
			
			if(GUI.Button(new Rect(stageRect.xMin,stageRect.yMax , 160, 90),"", GUI.skin.FindStyle("back")))
			{
				//set the BIGList from "VideoFrame" to"" and go to satgestate Normal. 
				BIGList[selGridInt] = ""; 
				currentStageState = StageState.Normal; 
				characterGrid = rigidBodyGrid = backgroundGrid = 0;
			}
			if(GUI.Button(new Rect(stageRect.xMax,stageRect.yMax, 160.0f, 90.0f),"Select", GUI.skin.FindStyle("done")))
			{
                if (characterGrid == characterImages.Length - 1)
                    currentStageState = StageState.CustomizingCharacter; 
                else 
                    currentStageState = StageState.SelectingObject;			
			}
		}
        //**************************************Customizing Character*******************
        else if(currentStageState == StageState.CustomizingCharacter)
        {
            if (!KenneyAlive)
            {
                KenneyCharacter = (GameObject)Instantiate(KenneyCharacter, new Vector3(0, 0, 0), Quaternion.identity);
                KenneyAlive = true;
            }
            Debug.Log("Child Count"+transform.childCount); 
            if(GUI.Button(new Rect(stageRect.xMin,stageRect.yMax , 160, 90),"", GUI.skin.FindStyle("back")))
			{

                DestroyKenney();
				//set the BIGList from "VideoFrame" to"" and go to satgestate Normal. 
				BIGList[selGridInt] = ""; 
				currentStageState = StageState.SelectingCharacter; 
				characterGrid = rigidBodyGrid = backgroundGrid = 0;
			}
            if (GUI.Button(new Rect(stageRect.xMax, stageRect.yMax, 160.0f, 90.0f), "Select", GUI.skin.FindStyle("done")))
            {
                DestroyKenney();
                currentStageState = StageState.SelectingObject;
            }
        }
		//************Selecting Rigid Body Object  ****************
		else if(currentStageState == StageState.SelectingObject)
		{
			GUI.Label(labelRect, "Choose an object");		
			scrollPositionRigidBody = GUI.BeginScrollView(wideRect,scrollPositionRigidBody, longRect); 
			rigidBodyGrid = GUI.SelectionGrid (longRect, rigidBodyGrid, myGameManager.GetComponent<GameManager>().rbdTexture, 4, GUI.skin.FindStyle("PropBox"));  
			GUI.EndScrollView();
			GUI.Label(miniStageRect, characterImages[characterGrid]); 
			GUI.Label(new Rect(miniStageRect.x + miniStageRect.width*0.5f , miniStageRect.y + miniStageRect.height*0.5f, miniStageRect.width*0.3f, miniStageRect.height*0.3f), myGameManager.GetComponent<GameManager>().rbdTexture[rigidBodyGrid]); 
			if(GUI.Button(new Rect(stageRect.xMin,stageRect.yMax , 160.0f, 90.0f),"", GUI.skin.FindStyle("back")))
			{
				currentStageState = StageState.SelectingCharacter; 
			}
			if(GUI.Button(new Rect(stageRect.xMax,stageRect.yMax, 160.0f, 90.0f),"Select", GUI.skin.FindStyle("done")))
			{
				//pickingObject = true; 
				// selectingRigidBody = false;
				
				currentStageState = StageState.SelectingBackGround;
			}
		}
		//************Selecting Background ****************
		else if(currentStageState == StageState.SelectingBackGround)
		{
			GUI.Label(labelRect, "Choose a background");
			
			scrollPositionRigidBody = GUI.BeginScrollView(wideRect,scrollPositionRigidBody, longRect); 
			backgroundGrid = GUI.SelectionGrid (longRect, backgroundGrid, myGameManager.GetComponent<GameManager>().bgTexture, 2, GUI.skin.FindStyle("PropBox"));  
			GUI.EndScrollView();
			//GUI.Label(miniStageRect, characterImages[characterGrid]); 
			//GUI.Label(Rect(miniStageRect.x + miniStageRect.width*0.5 , miniStageRect.y + miniStageRect.height*0.5, miniStageRect.width*0.3, miniStageRect.height*0.3), rigidBodyImages[rigidBodyGrid]); 
			if(GUI.Button(new Rect(stageRect.xMin,stageRect.yMax , 160.0f, 90.0f),"", GUI.skin.FindStyle("back")))
			{ 
				currentStageState = StageState.SelectingObject;
			}
			if(GUI.Button(new Rect(stageRect.xMax,stageRect.yMax, 160.0f, 90.0f),"Select", GUI.skin.FindStyle("done")))
			{
				//currentStageState = StageState.PickingPhysicalObject;

				if(rigidBodyGrid != 0) 	
					currentStageState = StageState.PickingPhysicalObject;	
				else // No Prop was chosen 
				{
                    SaveMyParameters();

					Application.LoadLevel(6);

				}
				
			}
		}
		
		//************ Picking Physical Object ****************
        else if (currentStageState == StageState.PickingPhysicalObject)
        {
            if (rigidBodyGrid != 0)
            {
                GUI.Label(labelRect, "Pick up this object from the toolbox.\n Click 'Done' when you have the object in hand.");
                physicalObjectId = whichPhysicalObject(rigidBodyGrid);
                GUI.Label(selectionRect, physicalObjectImages[physicalObjectId]);

                if (GUI.Button(new Rect(stageRect.xMin, stageRect.yMax, 160.0f, 90.0f), "", GUI.skin.FindStyle("back")))
                {
                    currentStageState = StageState.SelectingBackGround;
                }
                if (GUI.Button(new Rect(stageRect.xMax, stageRect.yMax, 160.0f, 90.0f), "Done", GUI.skin.FindStyle("done")))
                {
                    //Add rigidBody and hcaracter selected to playerprefs and call scene 7 
                    //PlayerPrefs.SetInt("CS", characterGrid);

                    SaveMyParameters();

                    Application.LoadLevel(6);
                }
            }
        }


	}
    private int[] KenneyArray;
    private void DestroyKenney()
    {
        GameObject go = GameObject.FindGameObjectWithTag("Player");
        if (!testingPhase)
            myGameManager.GetComponent<GameManager>().KenneyArray = SpriteManager.counterArray;
        else
        {
            Debug.Log("Character Customiz wont work!! ");
            KenneyArray = SpriteManager.counterArray;
        }
        GameObject.Destroy(go);
        KenneyAlive = false; 
    }
    #endregion

    #region Utility Functions
    ///////////////////UTILITY FUNCTIONS //
	int whichPhysicalObject(int rigidId )
	{
        if (rigidId >= 1 && rigidId <= 3)
            return 0;
        else if (rigidId > 3 && rigidId <= 6)
            return 1;
        else if (rigidId > 6 && rigidId <= 9)
            return 2;
        else if (rigidId > 9 && rigidId <= 12)
            return 3;
        else if (rigidId > 12 && rigidId <= 15)
            return 4;
        else if (rigidId > 15 && rigidId <= 30)
            return myGameManager.GetComponent<GameManager>().xmlDataList.rigidBodyList[rigidId - 16].physicalObjectId;
        else
        {
            Debug.Log("WRONG RIGIDID");
            return 5;
        }
	}

	int countWhichTabIsBlank()
	{
		for(int i =0; i<BIGList.Count;i++)
		{
			if(BIGList[i]=="")
				return i; 
		}
		return 0;
	}

	//if at least one frame is filled, then viewMyStory is switched On 
	void anyFrameFilled()
	{
		foreach(string element in BIGList)
		{
			if(element != "")
			{
				isViewMyStoryOn = true; 
				return ; 
			}
		}
	}
   
    //private bool isTextureLoaded = false;

   
    public Texture2D texture;
    private Texture2D[] textures;
    //string _frameFolder;
	//Decides which Textures Should go on GUI of different Tabs 
	void makeTabContents()
	{
        textures = new Texture2D[noOfTabs];
		tabContents = new GUIContent[noOfTabs];
        
        
        //int videoCounter = 0;
        
        for (int j = 0; j < BIGList.Count; j++)
        {
            if (BIGList[j] == "")
                tabContents[j] = new GUIContent("", blankTabTexture, "Frame " + (j + 1).ToString() + " Blank");
            else if (isVideoTab(BIGList[j]))
            {
                string imageLocation = BIGList[j].Substring(0, BIGList[j].Length - 4) + ".png";

                if (File.Exists(imageLocation))
                {
                    Texture2D tex = LoadPNG(imageLocation);
                    tabContents[j] = new GUIContent("", tex, "Frame " + (j + 1).ToString() + " Video");
                }
                else
                    tabContents[j] = new GUIContent("", videoTabTexture, "Frame " + (j + 1).ToString() + " Video");
            }
            else
                tabContents[j] = new GUIContent(BIGList[j], textFilledTabTexture, "Frame " + (j + 1).ToString() + " Text");
        }        
		
	}
	int findVideoNumber(int selGrid)
	{
		//Takes in a SelectionGridInteger and return eshtane video number for that video 
		int count = 0;
		for( int i = 0 ; i<=selGrid; i++)
		{
			if(isVideoTab(BIGList[i]))
				count++; 
		}
		return count;
	}
	//Takes in a String and Evaluates if that's a Valid File 
	bool isVideoTab(string tabContent)
	{
		//
		if(File.Exists(tabContent))
		{
			return true;
		}
		else 
			return false; 
	}

    /// <summary>
    /// Saves All the List data and PlayerPrefs data, Call this before Application.Loadlevel    
    /// </summary>
    void SaveMyParameters()
    {
        if (!testingPhase)
            myGameManager.GetComponent<GameManager>().dd.BIGList = BIGList;

        PlayerPrefs.SetString("storyTitle", storyTitle);
        PlayerPrefs.SetInt("rigidBodyGrid", rigidBodyGrid);
        PlayerPrefs.SetInt("selectionGrid", selGridInt);
        PlayerPrefs.SetInt("characterGrid", characterGrid);
        PlayerPrefs.SetInt("backgroundGrid", backgroundGrid);
        PlayerPrefs.SetString("storyTeller1", s1);
        PlayerPrefs.SetString("storyTeller2", s2); 
        PlayerPrefs.Save();
    }
    /// <summary>
    /// Returns a Texture2D and takes in FilePath for PNG , uses System.IO
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    Texture2D LoadPNG(string filePath)
    {
        Texture2D tex = new Texture2D(2, 2);
        byte[] data = File.ReadAllBytes(filePath);
        if (data == null)
            Debug.Log("BAD FILE PATH");
        else
        {

            tex.LoadImage(data);
        }
        return tex;
    }

    /// <summary>
    /// Deletes the video and ScreenShot of the video 
    /// </summary>
    void HandleVideoDelete()
    {
        //Delete ScreenShot 
        string screenShotFile = BIGList[selGridInt].Substring(0, BIGList[selGridInt].Length - 4); // remove ".avi"
        screenShotFile += ".png";
        if (File.Exists(screenShotFile))
        {
            File.Delete(screenShotFile); 
            Debug.Log("FILE deleted at " + screenShotFile);
        }
       //Delete Video 
        if (File.Exists(BIGList[selGridInt]))
        {
            File.Delete(BIGList[selGridInt]);
            Debug.Log("FILE deleted at " + BIGList[selGridInt].ToString());
        }

    }

    //IEnumerator WaitAndPrint(float waitTime)
    //{
    //    yield return new WaitForSeconds(waitTime);
    //    print("WaitAndPrint " + Time.time);
    //    isTextureLoaded = false;
    //}
    #endregion

}
