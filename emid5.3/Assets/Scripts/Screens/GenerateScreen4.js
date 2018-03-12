//Aligning the 3 boxes in all resolutions will be a CHALLENGE!  saveMyStrings function saves all the playerprefs at once. 
//Playerprefs : summaryBegin, summaryMid, summaryEnd, stateOfStory 

#pragma strict
import System.IO;
import System; 

public var customSkin:GUISkin;
public var label2Style:GUIStyle;
public var label3Style:GUIStyle;
public var question:String = "What is your story about?";
public var bgTexture : Texture2D;

private var menuBar:boolean = true;
private var stage:boolean = true;

private var questionWindow : Rect = new Rect(0,0, 800,200);

private var storyTitle:String = "";

private var summaryBegin:String = "";
private var summaryMid:String = "";
private var summaryEnd:String = "";
private var buttonClicked:String = "";

private var verticalSpacing:float = Screen.height*0.1;
private var horizontalSpacing:int = Screen.width/10;


private var boxHeight:int = Screen.height/4;
private var boxWidth:int = Screen.width/4;
private var freshStory:int ;

private var scrollPositionBegin : Vector2 = Vector2.zero;
private var scrollPositionMid : Vector2 = Vector2.zero;
private var scrollPositionEnd : Vector2 = Vector2.zero;

private var storyTeller1:String = ""; 
private var storyTeller2:String = "";
private var fileName:String = ""; 

function Start()
{
	storyTitle = PlayerPrefs.GetString("storyTitle");
	storyTeller1 = PlayerPrefs.GetString("storyTeller1");
	storyTeller2 = PlayerPrefs.GetString("storyTeller2");
	fileName = storyTeller1+storyTeller2; 
	
	freshStory = PlayerPrefs.GetInt("freshStory",freshStory);
	if(!freshStory)
	{
	summaryBegin = PlayerPrefs.GetString("summaryBegin",summaryBegin);
	summaryMid = PlayerPrefs.GetString("summaryMid",summaryMid);
	summaryEnd = PlayerPrefs.GetString("summaryEnd",summaryEnd);
	}
	saveInfoForDataAnalysis();
}

private var builtInArray1 : String[]; 
private var builtInArray2 : String[];
private var builtInArray3 : String[];

function saveInfoForDataAnalysis()
{
	//AppendAllText(C:\Users\Kumar\Documents\Dime\Assets);
	//This function takes in SUmaryBegin, mid, End, AND storyTitle, beginTabArray, midTabArray, endTabArray  
	var dt = DateTime.Now;
	var AllData:String = ""; 
	AllData += "\n\n\n Home Button was clicked at Time:";
	AllData += dt.ToString(); 
	AllData += "\nStory Title:"+storyTitle;
	AllData += "\n Story Authors are"+storyTeller1+"and"+storyTeller2+"\n";
	
	AllData += "\nBeginning Summary:"+summaryBegin+"\nMiddle Summary:"+summaryMid+"\nEnding Summary:"+summaryEnd+"\n\n"; 
	
	builtInArray1 = PlayerPrefsX.GetStringArray("beginTabArray");
	builtInArray2 = PlayerPrefsX.GetStringArray("midTabArray");
	builtInArray3 = PlayerPrefsX.GetStringArray("endTabArray"); 
	
	for(var i = 0;i < builtInArray1.length; i++)
	{	
		//parse through all the array elements and print it to the alldata
		if(builtInArray1[i] != "BLANK" && builtInArray1[i] != "")
		{
			AllData += "\nBeginning Frame Number"+i.ToString()+":"+builtInArray1[i];
		}
	}
	
	for( i = 0;i < builtInArray2.length; i++)
	{	
		if(builtInArray2[i] != "BLANK" && builtInArray2[i] != "")
		{
			AllData += "\nMiddle Frame Number"+i.ToString()+":"+builtInArray2[i];
		}
	}
	
	for( i = 0;i < builtInArray3.length; i++)
	{	
		//parse through all the array elements and print it to the alldata
		if(builtInArray2[i] != "BLANK" && builtInArray2[i] != "")
		{
			AllData += "\nEnding Frame Number"+i.ToString()+":"+builtInArray3[i];
		}
	}
	
	Debug.Log(AllData);
	File.AppendAllText(fileName,AllData);
	
	
}
function saveMyStrings() 	//Saves all the needed strings to PlayerPrefabs and saves it to hard disk
{
			if(menuBar)
			{
				PlayerPrefs.SetString("storyTitle",storyTitle);
				PlayerPrefs.Save(); 
			}
			if(stage)
			{
				PlayerPrefs.SetString("summaryBegin",summaryBegin);
				PlayerPrefs.SetString("summaryMid",summaryMid);
				PlayerPrefs.SetString("summaryEnd",summaryEnd);	
				PlayerPrefs.SetString("stateOfStory", buttonClicked);
				PlayerPrefs.SetInt("freshStory",freshStory);
				PlayerPrefs.Save();
			}
}


function OnGUI()
{
	
	GUI.skin = customSkin; //for label alignment 
	GUI.DrawTexture(Rect(0,0,Screen.width,Screen.height), bgTexture, ScaleMode.StretchToFill);

	if(menuBar)
	{
		GUILayout.BeginHorizontal();
		//GUILayout.Label("Home", GUILayout.Width(300));
		GUILayout.Space(Screen.width-600); 
		GUILayout.Label("Story Title:",GUILayout.Width(220),GUILayout.Height(50));
		//storyTitle = PlayerPrefs.GetString("storyTitle");
		storyTitle = GUILayout.TextField (storyTitle, 36,GUI.skin.FindStyle("menuText"), GUILayout.Width(300),GUILayout.Height(60));
		saveMyStrings();
		GUILayout.EndHorizontal();
	}
	if(stage)
	{
		GUILayout.BeginVertical();
		GUILayout.Space(verticalSpacing*0.6);
		GUILayout.Label(question,GUI.skin.FindStyle("screen4LabelCenter")); 
		//GUILayout.Space(horizontalSpacing/4);
		GUILayout.Label("Write a summary of the beginning middle and end of your story",GUI.skin.FindStyle("screen4LabelCenter2"));
		GUILayout.EndVertical();
		
		GUILayout.BeginHorizontal();
		
		GUILayout.BeginArea(Rect(horizontalSpacing,2*verticalSpacing,boxWidth, 3*boxHeight));
		
		GUILayout.BeginVertical();
		GUILayout.Space(verticalSpacing);
		GUILayout.Label("Beginning",label3Style);
	//	scrollPositionBegin =  GUILayout.BeginScrollView(scrollPositionBegin, false, true ,GUILayout.Width(1.2*boxWidth),GUILayout.Height(1.5*boxHeight));
		summaryBegin = GUILayout.TextArea(summaryBegin, 200,GUILayout.Width(boxWidth),GUILayout.Height(1.5*boxHeight));
	//	GUILayout.EndScrollView();
		if(GUILayout.Button("Create story",GUILayout.Width(boxWidth*0.96),GUILayout.Height(80)))
		{
			freshStory = 0;
			buttonClicked = "Beginning";
			saveMyStrings();
			Application.LoadLevel(5);
		}
	
		GUILayout.EndVertical();
		GUILayout.EndArea();
		
				
		GUILayout.BeginArea(Rect(20+boxWidth+horizontalSpacing,2*verticalSpacing,boxWidth,3*boxHeight));
		GUILayout.BeginVertical();
		
		GUILayout.Space(verticalSpacing);
		GUILayout.Label("Middle",label3Style);
	//	scrollPositionMid =  GUILayout.BeginScrollView(scrollPositionMid, false, true ,GUILayout.Height(1.5*boxHeight));
		summaryMid = GUILayout.TextArea(summaryMid, 200,GUILayout.Width(boxWidth),GUILayout.Height(1.5*boxHeight));
	//	GUILayout.EndScrollView();
		if(GUILayout.Button("Create story",GUILayout.Width(boxWidth*0.96),GUILayout.Height(80)))
		{
			freshStory = 0;
			buttonClicked = "Middle";
			saveMyStrings();
			Application.LoadLevel(5);
		}
		
		
		GUILayout.EndVertical();
		GUILayout.EndArea();
		
		GUILayout.BeginArea(Rect(20+(2*boxWidth)+horizontalSpacing,2*verticalSpacing,boxWidth,3*boxHeight));
		GUILayout.BeginVertical();
		
		GUILayout.Space(verticalSpacing);
		GUILayout.Label("Ending",label3Style);
	//	scrollPositionEnd =  GUILayout.BeginScrollView(scrollPositionEnd, true,true ,GUILayout.Height(1.5*boxHeight));
		summaryEnd = GUILayout.TextArea(summaryEnd, 200,GUILayout.Width(boxWidth),GUILayout.Height(1.5*boxHeight));	
	//	GUILayout.EndScrollView();
		if(GUILayout.Button("Create a story",GUILayout.Width(boxWidth*0.96),GUILayout.Height(80)))
		{
			freshStory = 0;
			buttonClicked = "Ending";
			saveMyStrings();
			Application.LoadLevel(5);
		}
		
		GUILayout.EndVertical();
		GUILayout.EndArea();
		
		GUILayout.EndHorizontal();	
	}
}
