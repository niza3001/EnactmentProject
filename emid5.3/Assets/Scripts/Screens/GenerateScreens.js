//layoutRectangle defines where the rectangle containing the label and the box appears. Screen.width and height are NOT working! And hence hard coding rectangle to be 640,360,400,200 in UNity


#pragma strict

public var customSkin:GUISkin;
private var themeJungle:String = "An adventure in the Jungle";
private var themeCave:String = "An adventure in the Cave";
private var todayTheme: String = "";
public var customStoryThemeStyle : GUIStyle;
public var bgTexture : Texture2D;

private var rectWidth =800;
private var rectHeight = 450;
private var layoutRectangle : Rect = new Rect((Screen.width/2)-(rectWidth/2), (Screen.height/2)-(rectHeight/2) , rectWidth, rectHeight);

private var storyThemeText :String = "The story theme for today is "; 
public var CaveOn:boolean; 
public function Start()
{
	if(CaveOn)
	{
		todayTheme = themeCave;
	}
	else 
	{
		todayTheme = themeJungle;	
	}
}
public function OnGUI()
{
	GUI.skin = customSkin; 
	//GUI.Label(layoutRectangle, "abc");
	//GUI.Label(Rect(0,0,Screen.width,Screen.height), bgTexture);
	GUI.DrawTexture(Rect(0,0,Screen.width,Screen.height), bgTexture, ScaleMode.StretchToFill);
	GUILayout.BeginArea(layoutRectangle);
	GUILayout.BeginVertical();
	GUILayout.Label(storyThemeText);
	GUILayout.Space(50);
	GUILayout.Label(todayTheme,customStoryThemeStyle);
	GUILayout.Space(50);
	GUILayout.BeginHorizontal();
	GUILayout.Space(rectWidth/3);
	if(GUILayout.Button("OK",GUILayout.Width(rectWidth/3),GUILayout.Height(80)))
	{
		Application.LoadLevel(Application.loadedLevel+1);
	}
	GUILayout.Space(rectWidth/3);
	GUILayout.EndHorizontal();
	GUILayout.EndVertical();
	GUILayout.EndArea();
}
