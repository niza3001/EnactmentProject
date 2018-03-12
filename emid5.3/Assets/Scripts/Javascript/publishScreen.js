#pragma strict
import System; 
import System.IO;

public var customSkin:GUISkin;
public var label2Style:GUIStyle;
public var question:String = "Do you want to name your story now?";
public var bgTexture : Texture2D;
public var videoTexture : Texture2D; 
public var rightArrow:Texture2D;
public var leftArrow:Texture2D;

private var alertBoxOn:boolean;
private var alertBox2On:boolean;
private var stageOn:boolean;
private var storyTitle:String = "";
private var s1 : String = ""; 
private var s2 : String = "";
private var combinedString : String = ""; 
private var videoOn : boolean;
//private var BIGList : List.< List.<String> > = new List.< List.<String> >(); 
private var builtInArray1 : String[]; 
private var selectionGridInt : int = 0; 
private var fakeSelectionGridInt : int = 1; 
private var noOfTabs:int = 19;
function Start () 
{
	
	storyTitle = PlayerPrefs.GetString("storyTitle"); 
	s1 = PlayerPrefs.GetString("storyTeller1"); 
	s2 = PlayerPrefs.GetString("storyTeller2"); 
	if(s1 == "")
		 combinedString = "By "+s2; 
	else if (s2 == "")
		combinedString = "By "+s1; 
	else 
		combinedString = "By "+s1+" and "+s2; 
	
	
	builtInArray1 = PlayerPrefsX.GetStringArray("beginTabArray");
	arrayWithOutBlank = builtInArray1;
	createNewArray();
	stageOn=true;
}
private var arrayWithOutBlank : String[];
private var numberOfFrames : int = 0 ; //NOT BLANK FRAMEs
function FindCorrectVideoNumber(fakeFrameNumber:int)
{
	/*private var videoNumberFake : int = 0; 
	private var totalVideoCount : int = 0;
	for (var i:int = 0; i < fakeFrameNumber; i++ )
	{
		if(arrayWithOutBlank[i]=="VideoFrame")
			totalVideoCount++;
	}
	var count:int = 0; 
	while(count<  )*/
	
}
function createNewArray()
{
	//builtInArray1.Reverse(builtInArray1);
	for(var i:int =0; i<builtInArray1.Length ; i++)
	{
		if(builtInArray1[i]!="")
		{
			arrayWithOutBlank[numberOfFrames++] = builtInArray1[i];
		}
				
	}
	for(var j:int = 0; j<numberOfFrames; j++)
	{
		;//Debug.Log("FINAL"+arrayWithOutBlank[j].ToString());
	}
	
}
private var labelString : String = "End Of Story"; 
private var stageRect = new Rect(Screen.width*0.2,Screen.height*0.2,Screen.width*0.6,Screen.height*0.5);
private var filename : String; 

function OnGUI()
{
	GUI.skin = customSkin;
	if(!videoOn)	
		GUI.DrawTexture(Rect(0,0,Screen.width,Screen.height), bgTexture, ScaleMode.StretchToFill);
	
	if(stageOn)
	{
		GUI.Label(Rect(Screen.width*0.3,0.05*Screen.height,Screen.width*0.4, Screen.height*0.1),storyTitle, label2Style); 
	
		GUI.Label(Rect(Screen.width*0.3,Screen.height*0.1,Screen.width*0.4, Screen.height*0.1), combinedString, label2Style);
	
		if(selectionGridInt == numberOfFrames)
		{
			videoOn = false;
			GUI.Label(stageRect, "The End", GUI.skin.FindStyle("specialText"));
		}
		else if(arrayWithOutBlank[selectionGridInt] == "VideoFrame")
		{
			//GUI.Label(Rect(Screen.width*0.35,Screen.height*0.4,Screen.width*0.3,Screen.height*0.2), videoTexture);
			//showVideo();
			videoOn = true;
				//FindCorrectVideoNumber(selectionGridInt);
				//PlayerPrefs.SetInt("videoFrameNumber", selectionGridInt);
				//PlayerPrefs.SetString("videoFileName", filename);
						
		}
		else 
		{
			videoOn = false; 
			var offset : int = 50;
			GUI.Label(stageRect, arrayWithOutBlank[selectionGridInt], GUI.skin.FindStyle("specialText"));
		}

		if(GUI.Button(Rect(Screen.width*0.35,Screen.height*0.7,Screen.width*0.06, Screen.height*0.06), leftArrow, GUI.skin.FindStyle("leftArrow")))
		{
			if(selectionGridInt != 0 )
			{
				selectionGridInt--; 
			}			
		}
		GUI.Label(Rect(Screen.width*0.45, Screen.height*0.7, Screen.width*0.1, Screen.height*0.06), "Frame "+(selectionGridInt+1).ToString());
	if(GUI.Button(Rect(Screen.width*0.59,Screen.height*0.7,Screen.width*0.06, Screen.height*0.06), rightArrow, GUI.skin.FindStyle("rightArrow")))
	{
			if(selectionGridInt != numberOfFrames)
			{
				selectionGridInt++;
			}
	}
	
	if(GUI.Button(Rect(Screen.width*0.05, Screen.height*0.88, Screen.width*0.1, Screen.height*0.1),"back",GUI.skin.FindStyle("illustrate")))
	{
		Application.LoadLevel(5);
	}
	if(GUI.Button(Rect(Screen.width*0.85, Screen.height*0.88, Screen.width*0.1, Screen.height*0.1),"Publish",GUI.skin.FindStyle("write")))
	{
		
		stageOn = false; 
		alertBoxOn = true; 	
	}
	}
	if(alertBoxOn)
	{
		GUI.Label(Rect(Screen.width*0.2, Screen.height*0.2,Screen.width*0.6,Screen.height*0.4 ), "Are you sure you want to publish your story now?\nYou won't be able to edit your story\n after publishing.", GUI.skin.FindStyle("alert"));
		if(GUI.Button(Rect(Screen.width*0.05, Screen.height*0.88, Screen.width*0.1, Screen.height*0.1),"back",GUI.skin.FindStyle("illustrate")))
		{
		alertBoxOn = false; 
		stageOn = true;
		}
		if(GUI.Button(Rect(Screen.width*0.85, Screen.height*0.88, Screen.width*0.1, Screen.height*0.1),"Yes, publish",GUI.skin.FindStyle("write")))
		{
			//saveInfoForDataAnalysis();
			alertBoxOn = false; 
			alertBox2On = true; 
		}
		
	}
	if(alertBox2On)
	{
		GUI.Label(Rect(Screen.width*0.2, Screen.height*0.2,Screen.width*0.6,Screen.height*0.4 ), "Congratulations on finishing your story!", GUI.skin.FindStyle("alert"));
		if(GUI.Button(Rect(Screen.width*0.85, Screen.height*0.88, Screen.width*0.1, Screen.height*0.1),"Restart",GUI.skin.FindStyle("write")))
		{
			Application.LoadLevel(1);	
		}
	}
}
private var AllData:String = "";
private var txtFileName:String = "";
private var directoryName:String = "";
function saveInfoForDataAnalysis()
{
	var tmp:int;	
	directoryName = "C:\\Users\\Kumar\\Documents\\Dime\\Assets\\Resources\\Videos\\"+s1+s2;
	Directory.CreateDirectory(directoryName);
	for(var j=0; j<numberOfFrames; j++)
	{
		AllData = "";
		
		if(arrayWithOutBlank[j]!="VideoFrame")
		{
			txtFileName = "C:\\Users\\Kumar\\Documents\\Dime\\Assets\\Resources\\Videos\\"+s1+s2+"\\"+"Frame Number"+j.ToString()+".txt";
		//var dt = DateTime.Now;
		
		//AllData += "\n\n\n Home Button was clicked at Time:";
		tmp = j+1; 
		AllData += "\nStory Title:"+storyTitle;
		AllData += "\n Story Authors are "+s1+" and "+s2+" \n";
		AllData += "\n Frame Number is "+tmp.ToString()+"\n"; 
		AllData += "===================================================\n";
		//AllData += "Frame Number is "+j.ToString()+"\n"; 
		AllData += arrayWithOutBlank[j];
		AllData += "\n===================================================\n";
		Debug.Log(AllData);
		File.AppendAllText(txtFileName,AllData);
		}
	}
}

private var myGUITex:GUITexture; 
//var myScript:Fighter; 
private var videoGUItex : GUITexture;  
    //the Movie texture  
public var mTex : MovieTexture;  
    //the AudioSource  
private var movieAS : AudioSource;  


function showVideo()	
{				
		filename = "Videos/"+s1+s2+"/"+selectionGridInt.ToString();
		//filename = "Videos/charliepiperMiddle4";
		videoGUItex = this.GetComponent(GUITexture); 
		movieAS = this.GetComponent(AudioSource);
		mTex = UnityEngine.Resources.Load(filename) as MovieTexture ;
		
		transform.localScale = Vector3 (0,0,0);
		transform.position = Vector3 (0.5,0.5,0);
		
		movieAS.clip = mTex.audioClip; 
		videoGUItex.texture = mTex;  
      	//Plays the movie  
       	//mTex.Play();  
       	//plays the audio from the movie  
      	//movieAS.Play();  		
       	Debug.Log(filename);    	
       
       	if(GUI.Button(Rect(Screen.width*0.05, Screen.height*0.3, Screen.width*0.1, Screen.height*0.1), "Play", GUI.skin.FindStyle("illustrate")))
       	{
       		mTex.Play(); 
       		movieAS.Play();
       	}
       	if(GUI.Button(Rect(Screen.width*0.05, Screen.height*0.5, Screen.width*0.1, Screen.height*0.1), "Pause", GUI.skin.FindStyle("write")))
       	{
       		mTex.Pause(); 
       		movieAS.Pause();
       	}
       	
}