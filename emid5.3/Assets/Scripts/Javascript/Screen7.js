//For now, I have assumed that the tabs will not be visible. 
//Playerpref : CharChosen
#pragma strict
public var customSkin:GUISkin;
public var myImages:Texture2D[];
public var bgTexture : Texture2D;
public var mySelectionStyle:GUIStyle;

private var wholeWindow : Rect = new Rect(0,0, 1920, 1080);
private var stateOfStory :String;
private var begin:boolean = false;
private var mid:boolean = false;
private var end:boolean = false;

public var menuLabel1Style:GUIStyle;
public var menuLabel2Style:GUIStyle; 

function Start()
{	
	
	stateOfStory= PlayerPrefs.GetString("stateOfStory");
	if(stateOfStory == "Beginning")
		begin =true; 
	else if(stateOfStory == "Middle")
		mid = true;
	else  
		end = true;

}

function OnGUI()
{
	GUI.skin = customSkin;
	GUI.Box(wholeWindow, bgTexture);
	
	createMenu();
	//createTabs();
	createSimpleMargin();

		createStageState7();

}

function createMenu()
{
GUILayout.BeginArea(Rect(0,0,1920,180));
GUILayout.BeginVertical();
	GUILayout.BeginHorizontal();
		GUILayout.Label("Tell your story now:", menuLabel1Style, GUILayout.Width(960), GUILayout.Height(60));
		GUILayout.Label(stateOfStory, menuLabel2Style,  GUILayout.Width(960), GUILayout.Height(60));
	GUILayout.EndHorizontal();
	GUILayout.Label("Choose a character", GUILayout.Width(960), GUILayout.Height(60));
	GUILayout.EndVertical();
	
GUILayout.EndArea();	
}


function createSimpleMargin()
{
	//GUI.Box(Rect(0,0,300,1080),marginTexture); BOX is an EPIC Fail! 
	GUILayout.BeginArea(Rect(0,240,300,840));
	GUILayout.BeginVertical();
	GUILayout.Space(50);
	GUILayout.Button("Enact", GUILayout.Width(250), GUILayout.Height(100));
	GUILayout.Space(150);
	if(GUILayout.Button("Home", GUILayout.Width(150), GUILayout.Height( 60)))
	
	GUILayout.EndVertical();
	GUILayout.EndArea();
}


private var myStrings:String[] = ["m1","m2","m3","m4","m5","m6"];

private var selGridInt : int = 0;
private var selGridInt2 : int = 2;

private var tabHeight:int = Screen.height *0.15;
private var tabWidth:int = Screen.width*0.2; 
private var noOfTabs:int = 8; 
private var Rect1 : Rect = new Rect(0, 0.05*Screen.height, 0.8*Screen.width, 0.17*Screen.height);
private var Rect2 : Rect = new Rect(0, 0.05*Screen.height, noOfTabs*tabWidth, 0.15*Screen.height);
private var scrollPosition :Vector2 = Vector2.zero; 

function createStageState7()
{
	selGridInt = GUI.SelectionGrid (Rect (400, 200, 256, 512), selGridInt, myImages, 2,GUI.skin.GetStyle("Selected Item"));
	//Debug.Log("Grid entered");
	if(GUI.Button(Rect(Screen.width-160,Screen.height-90,120,60),"Done"))
	{
		PlayerPrefs.SetInt("charChosen",selGridInt);
		PlayerPrefs.Save();
		Application.LoadLevel(7);
	}
	scrollPosition = GUI.BeginScrollView(Rect1,scrollPosition,Rect2);
	//selGridInt2 = GUI.SelectionGrid (Rect (0,0.05*Screen.height , noOfTabs*tabWidth, tabHeight), selGridInt2, myImages, 8,GUI.skin.GetStyle("Selected Item"));
	GUI.EndScrollView();
	
	//Debug.Log("Grid entered");
	if(GUI.Button(Rect(Screen.width-160,Screen.height-90,120,60),"Done"))
	{
		PlayerPrefs.SetInt("charChosen",selGridInt);
		PlayerPrefs.Save();
		Application.LoadLevel(7);
	}
	
}