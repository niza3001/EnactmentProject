//SImilar to the previous screen, without alert box. 
//Playerprefs storyTitle, beginTabArray, midTabArray, endTabArray, freshStory , dimeFile 
//storyStartTime 

#pragma strict
import System; 
import System.IO; 


public var customSkin:GUISkin;
public var label2Style:GUIStyle;
public var question:String = "Do you want to name your story now?";
public var bgTexture : Texture2D;

/*Total NUMBER OF FRAMES on SCREEN 5*/
public var totalNumberOfFrames:int = 12;

private var beginArr : String[];
private var beginTabArray = new Array(); //These 3 are used in Screen 6 onwards! They contain Vital Text/Picture/Video info
private var midTabArray = new Array();
private var endTabArray = new Array();

private var wholeWindow : Rect = new Rect(0,0, Screen.width, Screen.height);

private var layoutRectangle : Rect = new Rect(Screen.width*0.2, Screen.height*0.3 , Screen.width*0.6, Screen.height);

private var dimeFile :  String; 
private var dimeFolder :  String;
private var timeString : String;
private var s1 : String;
private var s2 : String;
private var storyTitle:String = ""; 
private var freshStory:int = 1; 
private var VerticalSpacing:int = 50;


function Start()
{
	initializeArrays(totalNumberOfFrames); // This integer controls the Maximum number of Frames in a scene.
	//beginTabArray = ["N","N","N","N","N","N","N","N"];
	//beginArr.Push("M");
	//beginArr.Push(10);
	//beginArr.Push("asfhdvgasufyvgjhv vsdhbjhsdvb bjbxcvkjb vskjb ");
/*	for(var i = 0;i<beginTabArray.length;i++)
	{
		Debug.Log(beginTabArray[i]);
		
	}	
	beginArr = ["da","asfkj"];*/
		
	/**************
	Important to convert JS array to BUILTIN array in order to fit into PlayerPrefs!!
	************/
	
	var builtInArray1 : String[] = beginTabArray.ToBuiltin(String) as String[];
	var builtInArray2 : String[] = midTabArray.ToBuiltin(String) as String[];
	var builtInArray3 : String[] = endTabArray.ToBuiltin(String) as String[];
	
	PlayerPrefsX.SetStringArray("beginTabArray",builtInArray1);
	PlayerPrefsX.SetStringArray("midTabArray",builtInArray2);
	PlayerPrefsX.SetStringArray("endTabArray",builtInArray3);
	
	timeString = System.DateTime.Now.Hour.ToString()+System.DateTime.Now.Minute.ToString();



	s1 = PlayerPrefs.GetString ("storyTeller1");
	s2 = PlayerPrefs.GetString ("storyTeller2");
    
	if(!Directory.Exists(Directory.GetParent(Application.dataPath)+"/Stories/"))
	    Directory.CreateDirectory(Directory.GetParent(Application.dataPath)+"/Stories/");

	dimeFile =  Directory.GetParent(Application.dataPath)+"/Stories/"+s1 + s2 +timeString+"/"+s1 + s2 +timeString+".story";
	Debug.Log(timeString);
	

	dimeFolder =  Directory.GetParent(Application.dataPath)+"/Stories/"+s1 + s2 +timeString+"/"; 
	Directory.CreateDirectory(dimeFolder); 
	


    //FileBrowserPath loads the xml think loadgame savegame 
	PlayerPrefs.SetString("FileBrowserPath", "");
	//PlayerPrefs.SetString("storyStartTime",timeString);
	//PlayerPrefs.SetInt("freshStory", freshStory);
	PlayerPrefs.SetString("dimeFolder",dimeFolder);
	PlayerPrefs.SetString("dimeFile",dimeFile);
	PlayerPrefs.Save();
	
}
function initializeArrays( num:int)
{
	for(var i:int =0; i<num;i++)
	{
		beginTabArray.Push("");
		midTabArray.Push("");
		endTabArray.Push("");		
	}
}
private var stateOfStory:String; 

function OnGUI()
{
	GUI.skin = customSkin; 
	GUI.DrawTexture(Rect(0,0,Screen.width,Screen.height), bgTexture, ScaleMode.StretchToFill);
	
	GUI.Label (Rect(0.2*Screen.width, 0.28*Screen.height, 0.6*Screen.width, 0.1*Screen.height),question);
	
	GUI.Label (Rect(0.15*Screen.width, 0.4*Screen.height, 0.15*Screen.width, 0.1*Screen.height), "Story Title: " );
	storyTitle = GUI.TextField (Rect(0.3*Screen.width, 0.4*Screen.height, Screen.width*0.55, Screen.height*0.1), storyTitle, 60);
	
	if(GUI.Button(Rect(Screen.width*0.3,Screen.height*0.55,250,100),"Done") || (GUI.Button(Rect(Screen.width*0.5,Screen.height*0.55, 250,100),"Later"))) 
	{
		Debug.Log(PlayerPrefs.GetString("storyTeller1"));
		PlayerPrefs.SetString("storyTitle", storyTitle);
		stateOfStory = "Beginning";
		PlayerPrefs.SetString("stateOfStory", stateOfStory);
		PlayerPrefs.Save();
		
		Application.LoadLevel(Application.loadedLevel+1);
	}
}

