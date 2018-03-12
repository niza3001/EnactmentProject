using UnityEngine;
using System.Collections;

public class Screen1 : MonoBehaviour {


    public GUISkin customSkin;
    public GUIStyle customStoryThemeStyle;
    //public GUIStyle customButtonThemeStyle;
    public Texture2D bgTexture;
    private GameObject myGameManager;

	// Use this for initialization
	void Start ()
    {
        myGameManager = GameObject.FindGameObjectWithTag("GameManager") as GameObject;        
    }
	void OnGUI()
    {
        GUI.skin = customSkin;
        GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), bgTexture, ScaleMode.StretchToFill);
        GUI.Label(new Rect(Screen.width*0.4f, Screen.height*0.3f, Screen.width*0.6f, Screen.height* 0.1f), "The story theme for today is \n"+ myGameManager.GetComponent<RUISInputManager>().StoryTheme, customStoryThemeStyle);
        if (GUI.Button(new Rect(Screen.width * 0.4f, Screen.height * 0.5f, Screen.width * 0.2f, Screen.height * 0.1f), "OK")) 
        {
            Application.LoadLevel(Application.loadedLevel + 1);
        }
        
    }
	// Update is called once per frame
	void Update () {
	
	}
}
