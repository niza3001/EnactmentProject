//2 new Playerprefs introduced characterGrid rigidBodyGrid of type int It's used to track which character and rigid body is chosen by next scene. 

#pragma strict

public var customSkin:GUISkin;
public var bgTexture : Texture2D;
public var dimeLogo : Texture2D;
public var blankTabTexture:Texture2D;
public var textFilledTabTexture: Texture2D;
public var videoTabTexture : Texture2D; 


public var marginTexture:Texture2D; 

public var emptyTabTexture:Texture2D; 

private var stateOfStory :String;
private var stateOfStoryInt:int;

private var beginCount:int = 0; //These variables indicate the number of tabs already used
private var midCount:int =0;
private var endCount:int =0;


private var builtInArray1 : String[]; 
private var builtInArray2 : String[];
private var builtInArray3 : String[];
private var beginTabArray = new Array(); //These 3 are used in Screen 6 onwards! They contain Vital Text/Picture/Video info
private var midTabArray = new Array();
private var endTabArray = new Array();
private var BIGList : List.< List.<String> > = new List.< List.<String> >(); //2d linked list containing all screens. 

private var beginTabContents : GUIContent[]; 
private var midTabContents : GUIContent[]; 
private var endTabContents : GUIContent[]; 

//private var totalNumberOfTabs:int = 8; //This is the maximum number of tabs which can be changed 

private var wholeWindow : Rect = new Rect(0,0, 1920, 1080);
private var question:String = "Do you want to Write or Enact? (Choose on the left)";
private var stageState:int = 5;  

private var tempString:String;

private var isWriteButtonOn:boolean;
private var isVideoButtonOn:boolean;
private	var isPictureButtonOn:boolean; 
private	var isEnactButtonOn:boolean;
private	var isHomeButtonOn:boolean;
private	var alertBoxOn:boolean;
private var userTypingFlag:boolean = true;
private var currentString:String = "";
private var isViewMyStoryOn:boolean; 

private enum StageState 
{
//Normal,
//WritingText, 
SelectingCharacter, 
SelectingObject, 
SelectingBackGround, 
PickingPhysicalObject, 
//PlayingVideo
}
private var currentStageState:StageState ;


function anyFrameFilled()
{
	for(var i:int =0; i<BIGList[stateOfStoryInt].Count; i++)
	{
		if(BIGList[stateOfStoryInt][i]!="" && BIGList[stateOfStoryInt][i]!="BLANK")
		{
			isViewMyStoryOn = true; 
			return; 
		}
	}
	isViewMyStoryOn = false; 
}
function Awake()
{
	
	//DontDestroyOnLoad(this);
}
function Start()
{
	currentStageState = StageState.SelectingCharacter;
	alertBoxOn = false; 
	//var newBool :boolean = PlayerPrefsX.GetBool("abcd");	
	stateOfStory= PlayerPrefs.GetString("stateOfStory");
	if(stateOfStory == "Beginning")
		stateOfStoryInt = 0;
	else if(stateOfStory == "Middle")
		stateOfStoryInt = 1;
	else  
		stateOfStoryInt =2;
		
	isWriteButtonOn = true;
	isPictureButtonOn=true; 
	isEnactButtonOn = true;
	isViewMyStoryOn = true;
	
	builtInArray1 = PlayerPrefsX.GetStringArray("beginTabArray");
	builtInArray2 = PlayerPrefsX.GetStringArray("midTabArray");
	builtInArray3 = PlayerPrefsX.GetStringArray("endTabArray");
	
	storyTitle = PlayerPrefs.GetString("storyTitle"); 
	s1  = PlayerPrefs.GetString("storyTeller1"); 
	s2 = PlayerPrefs.GetString("storyTeller2");
	//beginTabArray = new Array(builtInArray1);
	//midTabArray = new Array(builtInArray2);
	//endTabArray = new Array(builtInArray3);
	
	makeBigList();//Makes a list of all the text, picture, enact tab contents 
	makeTabContents();
	selGridInt = countWhichTabIsBlank();
	
	//TESTING if we really need it  
	if(BIGList[stateOfStoryInt][selGridInt] == "BLANK" || BIGList[stateOfStoryInt][selGridInt] == "")
	{
		stageState = 5;
	}
	else 
	{
		isEnactButtonOn = isPictureButtonOn = isHomeButtonOn = false;
		stageState = 6;
	}
	/*pickingObject = false; 
	selectingRigidBody  = false; 
	selectingCharacter = true;*/ 
}
function makeTabContents()
{
	beginTabContents = new GUIContent[noOfTabs];
	
		
	
		for(var j=0;j<BIGList[0].Count;j++)
		{
			if(BIGList[0][j] == "BLANK" || BIGList[0][j] == "" )
				beginTabContents[j] = GUIContent("",blankTabTexture,"Frame "+(j+1).ToString()+" Blank");	
			else if(BIGList[0][j] == "")
				beginTabContents[j] = GUIContent("",videoTabTexture,"Frame "+(j+1).ToString()+" Video");
			else 
				beginTabContents[j] = GUIContent(BIGList[0][j], textFilledTabTexture, "Frame "+(j+1).ToString()+" Text");
		}
		
		
		
	
}
function makeBigList()
{
	var j:int = 0;
	BIGList.Clear();
	for (var i = 0; i<3;i++)
	{
	var  subList : List.<String> = new List.<String>(); 
	if(i==0)
	{
		subList.Clear();
		for( j=0; j<builtInArray1.Length;j++)
		{
			subList.Add(builtInArray1[j]);
		}
		BIGList.Insert(i,subList);
	}	
}

}
function OnGUI()
{
	//Debug.Log("The video number is "+findVideoNumber(selGridInt).ToString());
	//makeBigList();
	//makeTabContents();
	GUI.skin = customSkin;
	//GUI.DrawTexture(Rect(0,0,Screen.width,Screen.height), bgTexture, ScaleMode.StretchToFill);	
	createMenu();
	/*if(BIGList[stateOfStoryInt][selGridInt] == "VideoFrame") 
	{
		stageState = 8;
		//Debug.Log(selGridInt.ToString());
	}
	else
		stageState =5; */
	createTabs();
	//Debug.Log(selGridInt.ToString());
	
		
				
	createMargin();
	if(stageState == 5 ) //NORMAL Stage 
		createStageState5();
		
	else if(stageState == 6)	//User is Writing text 
		createStageState6();
	else if(stageState == 7) //User is selecting stuff for Enactment 
	{
		createStageState7();
	}
	else if(stageState == 9 )
	{
		
		
		//Playback the recorded video 
		filename = "Videos/"+s1+s2+"/"+selGridInt.ToString();
		//Debug.Log("FILE NAME IN SCREEN 5 IS "+filename);
		PlayerPrefs.SetString("videoFileName", filename);
		PlayerPrefs.SetString("storyTitle",storyTitle);
		//PlayerPrefs.SetInt("videoCreationIndex",findVideoNumber(selGridInt));
		PlayerPrefs.Save();
		Application.LoadLevel(8);
	}
					//createStage(); //Screen 6 where text is entered
			
	if(!selectingRigidBody && !pickingObject ) 
		selectingCharacter = true;
	
	
}
function findVideoNumber(selGrid:int):int
{
	//Takes in a SelectionGridInteger and return eshtane video number for that video 
	var count : int = 0;
	for(var i:int = 0 ; i<=selGrid; i++)
	{
		if(BIGList[stateOfStoryInt][i] == "VideoFrame")
			count++; 
	}
	return count;
}

private var MenuPercent: float = 0.05;
private var tellYourStoryPrecent = 0.45;
private var storyPercent = 0.5;
private var storyTitle:String = "";

function createMenu()
{
	 var frameNumberInt: int = selGridInt+1; 
	 var frameNumberString:String = stateOfStory+" frame "+frameNumberInt.ToString();
	 GUI.Label(Rect(0, 0, Screen.width*0.1 ,Screen.height*MenuPercent),dimeLogo);
	 GUI.Label(Rect(Screen.width*0.2, 0, Screen.width*0.15 ,Screen.height*MenuPercent),"Story Title", GUI.skin.FindStyle("storyTitle"));
	 
	storyTitle = GUI.TextField(Rect(Screen.width*0.35, 0, Screen.width*0.5 ,Screen.height*MenuPercent), storyTitle, 48);
	
}

private var tabHeight:int = Screen.height *0.13;
private var tabWidth:int = Screen.width*0.18; 
private var noOfTabs:int = 20; 
private var Rect1 : Rect = new Rect(0, 0.07*Screen.height, 0.9*Screen.width, 0.17*Screen.height);
private var Rect2 : Rect = new Rect(0, 0.05*Screen.height, noOfTabs*tabWidth, 0.13*Screen.height);
private var scrollPosition :Vector2 = Vector2.zero; 
private var selGridInt : int = 0;
private var currentIndex: int = 0;
public var rightArrow:Texture2D;//Array holding all textures 
public var filmStrip:Texture2D; 
private var tempString1:String;
private var toolTipRect:Rect = new Rect(0.9*Screen.width,0.04*Screen.height, 0.1*Screen.width,0.2*Screen.height );
private var filmStripRect:Rect = new Rect(0, 0, 0.9*Screen.width, 0.27*Screen.height);
function createTabs()
{
	//GUI.Label (toolTipRect, GUI.tooltip);
	tempString1 = "";
	//TRASH BUTTON 
	GUI.Label(filmStripRect, filmStrip);
	/*
	if(GUI.Button (Rect (Screen.width*0.925,0.05*Screen.height,tabWidth*0.3,tabHeight/2), GUIContent("",""),GUI.skin.FindStyle("trash")))// 2nd argument is tooltip Text
	{
		BIGList[stateOfStoryInt].RemoveAt(selGridInt);
		BIGList[stateOfStoryInt].Add("");
		currentString = BIGList[stateOfStoryInt][selGridInt];
		saveMyArray(selGridInt);		
	}
	//LEFT Button 
	if(GUI.Button (Rect (Screen.width*0.92,0.05*Screen.height+tabHeight/2,tabWidth*0.15,tabHeight/2), GUIContent("",""),GUI.skin.FindStyle("leftArrow")))
	{
		if(selGridInt != 0)
		{
			tempString1 = BIGList[stateOfStoryInt][selGridInt];
			BIGList[stateOfStoryInt][selGridInt] = BIGList[stateOfStoryInt][selGridInt-1]; 
			BIGList[stateOfStoryInt][selGridInt-1] = tempString1; 
			
			currentString = BIGList[stateOfStoryInt][selGridInt];
			saveMyArray(selGridInt);
			currentString = BIGList[stateOfStoryInt][selGridInt-1];
			saveMyArray(selGridInt-1);
			
			selGridInt--; 
			
		}
	}
	//RIGHT Button
	if(GUI.Button (Rect (Screen.width*0.93+tabWidth*0.15,(0.05*Screen.height+tabHeight*0.5),tabWidth*0.15,tabHeight/2), GUIContent("",""),GUI.skin.FindStyle("rightArrow")))
	{
		if(selGridInt != noOfTabs-1)
		{
			tempString1 = BIGList[stateOfStoryInt][selGridInt];
			BIGList[stateOfStoryInt][selGridInt] = BIGList[stateOfStoryInt][selGridInt+1]; 
			BIGList[stateOfStoryInt][selGridInt+1] = tempString1; 
			
			currentString = BIGList[stateOfStoryInt][selGridInt];
			saveMyArray(selGridInt);
			currentString = BIGList[stateOfStoryInt][selGridInt+1];
			saveMyArray(selGridInt+1);
			
			selGridInt++; 
			
		}
	}
	*/
	GUI.Box (toolTipRect, GUI.tooltip,GUI.skin.FindStyle("ToolTipStyle"));
	//ACTUAL TABS - different tabs for begin mid end 
	if(stateOfStoryInt == 0)
	{
		//noOfTabs = BIGList[0].Count;
		scrollPosition = GUI.BeginScrollView(Rect1,scrollPosition,Rect2);
		selGridInt = GUI.SelectionGrid (Rect (0,0.05*Screen.height , noOfTabs*tabWidth, tabHeight), selGridInt, beginTabContents, noOfTabs, GUI.skin.GetStyle("item"));
		GUI.EndScrollView();
	}
	/*
	else if(stateOfStoryInt == 1)
	{
		scrollPosition = GUI.BeginScrollView(Rect1,scrollPosition,Rect2);
		selGridInt = GUI.SelectionGrid (Rect (0,0.05*Screen.height , noOfTabs*tabWidth, tabHeight), selGridInt, midTabContents, noOfTabs,GUI.skin.GetStyle("item"));
		GUI.EndScrollView();
	}
	else if(stateOfStoryInt == 2)
	{
		scrollPosition = GUI.BeginScrollView(Rect1,scrollPosition,Rect2);
		selGridInt = GUI.SelectionGrid (Rect (0,0.05*Screen.height , noOfTabs*tabWidth, tabHeight), selGridInt, endTabContents, noOfTabs,GUI.skin.GetStyle("item"));
		GUI.EndScrollView();
	}*/
	//THE BOX IN WHICH TOOLTIP is displayed. 
	GUI.Box (toolTipRect, GUI.tooltip,GUI.skin.FindStyle("ToolTipStyle"));
	/*
	if(GUI.Button (Rect (Screen.width*0.8,0.05*Screen.height,tabWidth/2,tabHeight/2), "",GUI.skin.FindStyle("leftArrow")))
	{
	}
	if(GUI.Button (Rect (Screen.width*0.8,(0.05*Screen.height+tabHeight*0.5),tabWidth/2,tabHeight/2), "",GUI.skin.FindStyle("rightArrow")))
	{
	}*/
}
private var beginPosX : float = 0.02;
private var beginPosY : float = 0.4;
private var buttonWidth:float = 0.16;
private var buttonHeight:float = 0.09; 
private var offsetY:float = 0.12; 
private var myGUITex:GUITexture; 
//var myScript:Fighter; 
private var videoGUItex : GUITexture;  

private var s1 : String;
private var s2 : String; 
private var filename : String;
/*
bool isTextFrame(String videoPath)
	{
		Match match = Regex.Match(videoPath, @"C:*.avi$",
		                          RegexOptions.IgnoreCase);
		if(match.Success)
		{
			return false;
		}
		else
		{
			return true;
		}
	}	*/

function createMargin()
{
	if(BIGList[stateOfStoryInt][selGridInt] == "" && stageState == 5)
	{
		isEnactButtonOn	= isPictureButtonOn = isHomeButtonOn = true;
		isVideoButtonOn = false;
		//isHomeButtonOn = false;
	}
	else if(BIGList[stateOfStoryInt][selGridInt] == "VideoFrame" && stageState == 5)
	{
		isEnactButtonOn	= isPictureButtonOn = isWriteButtonOn = isVideoButtonOn  = false;
		isVideoButtonOn = isHomeButtonOn = true;
	}
	else if (stageState == 6)
		isEnactButtonOn	= isPictureButtonOn = isViewMyStoryOn = isHomeButtonOn = false;
	else if ( stageState == 7) 
	 	isPictureButtonOn = isWriteButtonOn = isHomeButtonOn = false;
	
	if(BIGList[stateOfStoryInt][selGridInt] != "" && BIGList[stateOfStoryInt][selGridInt] != "PPP" && BIGList[stateOfStoryInt][selGridInt] != "VideoFrame")
	{
		//Already data entered. So disable pic and enact 
		isEnactButtonOn	= isPictureButtonOn = false;
	}
	if( isVideoButtonOn && BIGList[stateOfStoryInt][selGridInt] == "VideoFrame" )
	{
		if(GUI.Button(Rect(beginPosX*Screen.width, beginPosY*Screen.height, buttonWidth*Screen.width, buttonHeight*Screen.height), "View video", GUI.skin.FindStyle("write")))
		{
			
			stageState = 9; 
			currentIndex = selGridInt;
			currentString = BIGList[stateOfStoryInt][selGridInt];
			
		}
	}
	
	if(isWriteButtonOn)
	{
		if(GUI.Button(Rect(beginPosX*Screen.width, beginPosY*Screen.height, buttonWidth*Screen.width, buttonHeight*Screen.height), "Write", GUI.skin.FindStyle("write")))
		{
			
			stageState = 6; 
			currentIndex = selGridInt;
			currentString = BIGList[stateOfStoryInt][selGridInt];
			
		}
	}
	if(isEnactButtonOn)
	{
		if(GUI.Button(Rect(beginPosX*Screen.width, (beginPosY*Screen.height)+(offsetY*Screen.height), buttonWidth*Screen.width, buttonHeight*Screen.height), "Enact", GUI.skin.FindStyle("enact")))
		{
			stageState = 7; 
			currentIndex = selGridInt;
			
			//Application.LoadLevel(6);
			
		}
	}
	if(isPictureButtonOn)
	{
		/*if(GUI.Button(Rect(beginPosX*Screen.width, (beginPosY*Screen.height)+(offsetY*2*Screen.height), buttonWidth*Screen.width, buttonHeight*Screen.height), "Illustrate", GUI.skin.FindStyle("illustrate")))
		{
			
		}*/
		;
	}
	anyFrameFilled();
	if(isViewMyStoryOn && isHomeButtonOn)
	{
		if(GUI.Button(Rect(beginPosX*Screen.width, (0.8*Screen.height), 1.1*buttonWidth*Screen.width, 1.3*buttonHeight*Screen.height), "View my story", GUI.skin.FindStyle("home")))
		{
			//Save all needed info to the Disc 
			PlayerPrefs.SetString("storyTitle",storyTitle);
			PlayerPrefs.Save();
			Application.LoadLevel(7);
		}
	}
	
}
private var beginStagePosX : float = 0.2;
private var beginStagePosY : float = 0.25;
private var stageWidth:float = 0.65;
private var stageHeight:float = 0.6; 

private var stageRect : Rect = new Rect(Screen.width*beginStagePosX, Screen.height*beginStagePosY, Screen.width*stageWidth, Screen.height*stageHeight);
//public var stageBG:Texture2D;
function createStageState5()//Just display the Question in Screen 5 
{
	isEnactButtonOn = isPictureButtonOn = isViewMyStoryOn = isWriteButtonOn = true; 
	if(BIGList[stateOfStoryInt][selGridInt] == "BLANK" || BIGList[stateOfStoryInt][selGridInt] == "")
		GUI.Label(stageRect,question,GUI.skin.GetStyle("Screen5StageStyle"));
	else if(BIGList[stateOfStoryInt][selGridInt] == "VideoFrame")
		GUI.Label(stageRect,"Click View button to play the video",GUI.skin.GetStyle("Screen5StageStyle"));
	else 
		GUI.Label(stageRect,"Click Write button to edit the text",GUI.skin.GetStyle("Screen5StageStyle"));
}

private var alertBoxWidth = 480; 
private var alertBoxHeight = 270;
public var bgTextureAlert : Texture2D;

function createStageState6()
{
	isViewMyStoryOn = false; 
	//COMPARE selGridInt and currentIndex ?? 
	 if(currentIndex != selGridInt)//  user clicked another tab in between WRITING!! currentIndex is set when the ENACT/Picture/Write button is pressed 
	 {
	 	alertBoxOn = true; 
	 }	 
	 if(alertBoxOn)
	 {
	 	GUI.DrawTexture(Rect(0,0,Screen.width,Screen.height), bgTexture, ScaleMode.StretchToFill);
		GUI.Label(Rect((Screen.width/2)-(alertBoxWidth/2), (Screen.height/2)-(alertBoxHeight/2),alertBoxWidth,alertBoxHeight),bgTextureAlert);
		
		GUI.Label(Rect(Screen.width*0.2,Screen.height*0.3,Screen.width*0.6, Screen.height*0.2),"You must click Done before clicking another Frame",GUI.skin.FindStyle("alertStyle"));
		
		if(GUI.Button(Rect(Screen.width*0.4,Screen.height*0.5,Screen.width*0.1, Screen.height*0.1),"OK", GUI.skin.FindStyle("home")))
		{
			alertBoxOn = false;
			selGridInt = currentIndex;
		}
		
	 }
	 
	if(userTypingFlag)//If you dont use the userTypingFlag the content of currentString will always be the SAME while typing the text !! 
	{
	if(BIGList[stateOfStoryInt][selGridInt] != "")
	{
		currentString = BIGList[stateOfStoryInt][selGridInt];
		userTypingFlag = false;
	}
	}
	currentString = GUI.TextArea(stageRect, currentString, 249); 
	if(GUI.Button(Rect(Screen.width*0.8, Screen.height*0.9,buttonWidth*Screen.width, buttonHeight*Screen.height), "Done", GUI.skin.FindStyle("done")) && alertBoxOn == false)
	{
		//Save all the strings to disk based on stateOfStory and selectiongrid 
		
		saveMyArray(selGridInt);	
		selGridInt++; 
		isPictureButtonOn = true;
		isEnactButtonOn = true;
		stageState =5;
		makeTabContents();
	}
}


//There are three subscreens in this Stage 7. 
	//Chose character, RigidBody, Pick up object.
private var selectingRigidBody : boolean;
private var selectingCharacter : boolean; 
private var pickingObject : boolean ; 
private var characterGrid : int = 0;
private var rigidBodyGrid : int = 0; 
private var backgroundGrid : int = 0; 
public var characterImages:Texture2D[]; 
public var rigidBodyImages : Texture2D[]; 
public var backgroundImages : Texture2D[]; 
public var physicalObjectImages : Texture2D[]; 

private var selectionRect : Rect = Rect(stageRect.x,1.3*stageRect.y,stageRect.width*0.8,0.8*stageRect.height); // Rectangle for selection grid 
private var labelRect : Rect = Rect(stageRect.x, stageRect.y - stageRect.height*0.05,selectionRect.width, stageRect.height*0.2); //Rectangle for Label 
private var miniStageRect : Rect = Rect(stageRect.xMax-Screen.width*0.05, selectionRect.yMin, Screen.width*0.2,selectionRect.height);//Rectangle for the mini stage displaying character 

private var scrollPositionRigidBody :Vector2 = Vector2.zero; 
private var wideRect : Rect = Rect(selectionRect.x, selectionRect.y, selectionRect.width*1.1 ,selectionRect.height);
private var longRect : Rect = Rect(selectionRect.x, selectionRect.y, selectionRect.width, selectionRect.height*2.5 );
public var purpleBox : Texture2D; //PurpleBox with white bg 
private var physicalObjectId:int; //stores which physical object needs to be picked from the BOX 

function createStageState7()
{ 
	//creates characterGrid rigidBodyGrid 
	if(currentIndex != selGridInt)//  user clicked another tab in between WRITING!! currentIndex is set when the ENACT/Picture/Write button is pressed 
	 {
	 	//set the BIGList from "VideoFrame" to"" and go to satge 5 state. 
		BIGList[stateOfStoryInt][selGridInt] = ""; 
		stageState = 5;
	 }	 
	//************Selecting Character ****************
	if(currentStageState == StageState.SelectingCharacter) 
	{ 
		//GUI.Label(stageRect, purpleBox);
		GUI.Label(labelRect, "Choose a character");
		characterGrid = GUI.SelectionGrid (selectionRect , characterGrid, characterImages, 2, GUI.skin.FindStyle("PropBox"));
		GUI.Label(miniStageRect, characterImages[characterGrid]);
		
		if(GUI.Button(Rect(stageRect.xMin,stageRect.yMax , 160, 90),"", GUI.skin.FindStyle("back")))
		{
			//set the BIGList from "VideoFrame" to"" and go to satge 5 state. 
			BIGList[stateOfStoryInt][selGridInt] = ""; 
			stageState = 5; 
			characterGrid = rigidBodyGrid = backgroundGrid = 0;
		}
		if(GUI.Button(Rect(stageRect.xMax,stageRect.yMax, 160, 90),"Select", GUI.skin.FindStyle("done")))
		{
			//selectingCharacter = false; 
			//selectingRigidBody = true; 
			currentStageState = StageState.SelectingObject;			
		}
	}
	//************Selecting Rigid Body Object  ****************
	if(currentStageState == StageState.SelectingObject)
	{
		GUI.Label(labelRect, "Choose an object");		
		scrollPositionRigidBody = GUI.BeginScrollView(wideRect,scrollPositionRigidBody, longRect); 
		rigidBodyGrid = GUI.SelectionGrid (longRect, rigidBodyGrid, rigidBodyImages, 4, GUI.skin.FindStyle("PropBox"));  
		GUI.EndScrollView();
		GUI.Label(miniStageRect, characterImages[characterGrid]); 
		GUI.Label(Rect(miniStageRect.x + miniStageRect.width*0.5 , miniStageRect.y + miniStageRect.height*0.5, miniStageRect.width*0.3, miniStageRect.height*0.3), rigidBodyImages[rigidBodyGrid]); 
		if(GUI.Button(Rect(stageRect.xMin,stageRect.yMax , 160, 90),"", GUI.skin.FindStyle("back")))
		{
			//selectingCharacter = true; 
			//selectingRigidBody = false; 
			currentStageState = StageState.SelectingCharacter; 
		}
		if(GUI.Button(Rect(stageRect.xMax,stageRect.yMax, 160, 90),"Select", GUI.skin.FindStyle("done")))
		{
			 	
			/*if(rigidBodyGrid == 0)
			{
				//BIGList[stateOfStoryInt][selGridInt] = "VideoFrame";
				PlayerPrefs.SetString("storyTitle",storyTitle);
				PlayerPrefs.SetInt("rigidBodyGrid", rigidBodyGrid); 
				PlayerPrefs.SetInt("selectionGrid", selGridInt);
				PlayerPrefs.SetInt("characterGrid", characterGrid); 
				PlayerPrefs.Save();
				Application.LoadLevel(7);
				return;
			}	*/	
			//pickingObject = true; 
			// selectingRigidBody = false;
			
			currentStageState = StageState.SelectingBackGround;
		}
	}
	//************Selecting Background ****************
	if(currentStageState == StageState.SelectingBackGround)
	{
		GUI.Label(labelRect, "Choose a background");
		
		scrollPositionRigidBody = GUI.BeginScrollView(wideRect,scrollPositionRigidBody, longRect); 
		backgroundGrid = GUI.SelectionGrid (longRect, backgroundGrid, backgroundImages, 2, GUI.skin.FindStyle("PropBox"));  
		GUI.EndScrollView();
		//GUI.Label(miniStageRect, characterImages[characterGrid]); 
		//GUI.Label(Rect(miniStageRect.x + miniStageRect.width*0.5 , miniStageRect.y + miniStageRect.height*0.5, miniStageRect.width*0.3, miniStageRect.height*0.3), rigidBodyImages[rigidBodyGrid]); 
		if(GUI.Button(Rect(stageRect.xMin,stageRect.yMax , 160, 90),"", GUI.skin.FindStyle("back")))
		{
			//selectingCharacter = true; 
			//selectingRigidBody = false; 
			currentStageState = StageState.SelectingObject;
		}
		if(GUI.Button(Rect(stageRect.xMax,stageRect.yMax, 160, 90),"Select", GUI.skin.FindStyle("done")))
		{
			if(rigidBodyGrid != 0) 	
				currentStageState = StageState.PickingPhysicalObject;	
			else 
			{
				PlayerPrefs.SetString("storyTitle",storyTitle);
				PlayerPrefs.SetInt("rigidBodyGrid", rigidBodyGrid); 
				PlayerPrefs.SetInt("selectionGrid", selGridInt);
				PlayerPrefs.SetInt("characterGrid", characterGrid); 
				
				PlayerPrefs.SetInt("backgroundGrid",backgroundGrid);
				
				PlayerPrefs.Save();
				Application.LoadLevel(7);
			}
			
		}
	}
	
	//************ Picking Physical Object ****************
	if(currentStageState == StageState.PickingPhysicalObject)
	{
		GUI.Label(labelRect, "Pick up this object from the toolbox.\n Click 'Done' when you have the object in hand."); 
		 physicalObjectId = whichPhysicalObject(rigidBodyGrid);
		GUI.Label(selectionRect, physicalObjectImages[physicalObjectId]); 
		
		if(GUI.Button(Rect(stageRect.xMin,stageRect.yMax , 160, 90),"", GUI.skin.FindStyle("back")))
		{
			pickingObject = false; 
			selectingRigidBody = true;
		}
		if(GUI.Button(Rect(stageRect.xMax,stageRect.yMax, 160, 90),"Done", GUI.skin.FindStyle("done")))
		{
			//Add rigidBody and hcaracter selected to playerprefs and call scene 7 
			//PlayerPrefs.SetInt("CS", characterGrid);
			PlayerPrefs.SetString("storyTitle",storyTitle);
			PlayerPrefs.SetInt("rigidBodyGrid", rigidBodyGrid); 
			PlayerPrefs.SetInt("selectionGrid", selGridInt);
			PlayerPrefs.SetInt("characterGrid", characterGrid); 
			
			PlayerPrefs.SetInt("backgroundGrid",backgroundGrid);
			
			PlayerPrefs.Save();
			Application.LoadLevel(6); 
		}
	}
}

function whichPhysicalObject(rigidId : int )
{
	if(rigidId >= 1 && rigidId <=8) 
		return 0; 
	else if (rigidId >=9 && rigidId <=13)
		return 1; 
	else if (rigidId >= 14 && rigidId <=18)
		return 2; 
	else if(rigidId >=19 && rigidId <= 25)
		return 3; 
	else if (rigidId >= 26 && rigidId <=36)
		return 4; 
	else 
		return 5; 
}
function saveMyArray(index:int) 
{
	BIGList[stateOfStoryInt][index] = currentString; 
		
		//	beginTabArray[currentIndex] = currentString; 
		//	builtInArray1  = beginTabArray.ToBuiltin(String) as String[];
		switch(stateOfStoryInt)
		{
		
			case 0: builtInArray1 = BIGList[stateOfStoryInt].ToArray();
					PlayerPrefsX.SetStringArray("beginTabArray",builtInArray1);	
					break; 
			case 1: builtInArray2 = BIGList[stateOfStoryInt].ToArray();
					PlayerPrefsX.SetStringArray("midTabArray",builtInArray2);	
					break; 			
			case 2: builtInArray3 = BIGList[stateOfStoryInt].ToArray();
					PlayerPrefsX.SetStringArray("endTabArray",builtInArray3);	
					break; 
		}
		
}

function countWhichTabIsBlank()
{
	for(var i:int =0; i<BIGList[stateOfStoryInt].Count; i++)
	{
		if(BIGList[stateOfStoryInt][i]=="")
		{
			return i;
		}
	}
}
