using UnityEngine;
using System.Collections;

public class GenerateLoadGameSaveGame : MonoBehaviour {
    
    //PUBLIC Variables 
    public GUIStyle customStoryThemeStyle;
    public Texture2D bgTexture;   
    public GUISkin customSkin;

	// Use this for initialization
	void Start () {
               
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //ONGUI 
    void OnGUI()
    {
        GUI.skin = customSkin;
        //GUI.Label(layoutRectangle, "abc");
        //GUI.Label(Rect(0,0,Screen.width,Screen.height), bgTexture);
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), bgTexture, ScaleMode.StretchToFill);
        GUILayout.BeginArea(new Rect(0f,0f,Screen.width, Screen.height));
        GUILayout.BeginVertical();
        //GUILayout.Label("What do you want to do?");
        //GUILayout.Space(50);
        //GUILayout.Label(todayTheme, customStoryThemeStyle);
        GUILayout.Space(Screen.height*0.4f);
        GUILayout.BeginHorizontal();
        GUILayout.Space(Screen.width*0.25f);
        if (GUILayout.Button("New Story", GUILayout.Width(Screen.width*0.2f), GUILayout.Height(Screen.height*0.2f)))
        {
            //Take them to the Next Screen as usual. 
            Application.LoadLevel(2);
        }
        GUILayout.Space(Screen.width * 0.1f);
        if (GUILayout.Button("Load Story", GUILayout.Width(Screen.width * 0.2f), GUILayout.Height(Screen.height*0.2f)))
        {
            //Take them to FileBrowser Screen where you select a file
            Application.LoadLevel(Application.loadedLevel+1);
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.EndArea();
    }

}
