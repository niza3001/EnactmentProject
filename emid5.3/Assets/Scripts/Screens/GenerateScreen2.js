	//If ScreenOn is true then ask the names, if both the names are emopty, then alert them. 
//Playerprefs : storyTeller1,storyTeller2

#pragma strict

public var customSkin:GUISkin;
public var label2Style:GUIStyle;
public var defaultButton:GUIStyle; 
public var alertStyle:GUIStyle; 
public var question:String = "What are your names?";
public var bgTexture : Texture2D;

public var bgTextureAlert : Texture2D;

private var storyTeller1:String = ""; 
private var storyTeller2:String = ""; 

private var rectWidth =800;
private var rectHeight = 450;

private var alertBoxWidth = 480; 
private var alertBoxHeight = 270;

private var layoutRectangle : Rect = new Rect((Screen.width/2)-(rectWidth/2), (Screen.height/2)-(rectHeight/2) , rectWidth, rectHeight);


private var alertBoxOn : boolean = false; 
private var ScreenOn : boolean = true; 
private var VerticalSpacing:int = 50;
function OnGUI()
{
	GUI.skin = customSkin; 
	if(ScreenOn)// Display Screen No 2 
	{
	//for label alignment 
	GUI.DrawTexture(Rect(0,0,Screen.width,Screen.height), bgTexture, ScaleMode.StretchToFill);
	GUILayout.BeginArea (layoutRectangle);
	GUILayout.BeginVertical();
	GUILayout.Label (question);
	GUILayout.Space(VerticalSpacing);	
	GUILayout.BeginHorizontal();	
	GUILayout.Label ("Storyteller 1:", label2Style);
	storyTeller1 = GUILayout.TextField (storyTeller1, 48,GUILayout.Width(500));
	GUILayout.EndHorizontal();
	GUILayout.Space(VerticalSpacing);	
	GUILayout.BeginHorizontal();	
	GUILayout.Label ("Storyteller 2:", label2Style);
	storyTeller2 = GUILayout.TextField (storyTeller2, 48,GUILayout.Width(500));
	GUILayout.EndHorizontal();
	GUILayout.Space(VerticalSpacing);		
	GUILayout.BeginHorizontal();	
	GUILayout.Space(300);	
	//GUILayout.skin = customSkin;
	if(GUILayout.Button("Done",GUILayout.Width(250),GUILayout.Height(80) )) 
	{
		if(storyTeller1.Length == 0 && storyTeller2.Length ==0)
		{
		alertBoxOn = true;
		ScreenOn = false;
		}
		else //Player has Entered the name
		{
			PlayerPrefs.SetString("storyTeller1", storyTeller1);
			PlayerPrefs.SetString("storyTeller2", storyTeller2);
			PlayerPrefs.Save();
			Application.LoadLevel(Application.loadedLevel+1);
		}
	}
	GUILayout.EndHorizontal();
	GUILayout.EndVertical();	
	GUILayout.EndArea ();
	}
	
	if(alertBoxOn)
	{
		GUI.DrawTexture(Rect(0,0,Screen.width,Screen.height), bgTexture, ScaleMode.StretchToFill);
		GUI.Label(Rect((Screen.width/2)-(alertBoxWidth/2), (Screen.height/2)-(alertBoxHeight/2),alertBoxWidth,alertBoxHeight),bgTextureAlert);
		
		GUILayout.BeginArea(Rect((Screen.width/2)-(alertBoxWidth/2), (Screen.height/2)-(alertBoxHeight/2),alertBoxWidth,alertBoxHeight));
		GUILayout.BeginVertical();
			GUILayout.Space(50);

			GUILayout.Label("Enter at least one storyteller's name",alertStyle);
			GUILayout.Space(50);
		GUILayout.BeginHorizontal();
		GUILayout.Space(alertBoxWidth*0.33);
		if(GUILayout.Button("OK",GUILayout.Width(160),GUILayout.Height(90)))
		{
			alertBoxOn = false;
			ScreenOn = true;
		}
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
		GUILayout.EndArea ();
		 
	}
}