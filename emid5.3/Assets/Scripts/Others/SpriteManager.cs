using UnityEngine;
using System.Collections;

public class SpriteManager  : MonoBehaviour{
		
	public int count ; 

	//VARIABLES 
	public static int selGridInt ; 
	public static int[] counterArray = {0,0,0};

	private string[] selGridText = {"Face","Clothes","Hair"};

	// Use this for initialization
	void Start () {
		//selGridInt  = 0;

        //counterArray[0] = counterArray[1] = counterArray[2] = 0;
	}
	
	// Update is called once per frame
	void Update () {
        
	}

	void OnGUI()
	{

		selGridInt = GUI.SelectionGrid(new Rect(Screen.width*0.4f,Screen.height*0.85f,Screen.width*0.3f,Screen.height*0.1f),selGridInt,selGridText,3);
		//Using Buttons instead of Arrow keys 
		if(GUI.Button(new Rect(Screen.width*0.3f,Screen.height*0.85f,Screen.width*0.1f,Screen.height*0.1f),"<"))
		{			
			if(counterArray[selGridInt]>0)
				counterArray[selGridInt]--;
		}
		if(GUI.Button(new Rect(Screen.width*0.7f,Screen.height*0.85f,Screen.width*0.1f,Screen.height*0.1f),">"))
		{
			counterArray[selGridInt]++;
		}
	
	}
}
