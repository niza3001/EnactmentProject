#pragma strict

private var myGUITex:GUITexture; 
//var myScript:Fighter; 
private var videoGUItex : GUITexture;  
    //the Movie texture  
public var mTex : MovieTexture;  
    //the AudioSource  
private var movieAS : AudioSource;  
private var filename : String;

function Start () {

}

function Update () {
	
	
}
function OnGUI()
{
	showVideo();
}
function showVideo()	
{				
		filename = PlayerPrefs.GetString("videoFileName");
		//filename = "Videos/charliepiperMiddle4";
		videoGUItex = this.GetComponent(GUITexture); 
		movieAS = this.GetComponent(AudioSource);
		mTex = Resources.Load(filename) as MovieTexture ;
		
		transform.localScale = Vector3 (0,0,0);
		transform.position = Vector3 (0.5,0.5,0);
		
		movieAS.clip = mTex.audioClip; 
		videoGUItex.texture = mTex;  
      	//Plays the movie  
       	//mTex.Play();  
       	//plays the audio from the movie  
      	//movieAS.Play();  		
       	Debug.Log(filename);    	
       	if(GUI.Button(Rect(Screen.width*0.3,0,160,90),"Back"))
       		Application.LoadLevel(5);
       	if(GUI.Button(Rect(Screen.width*0.5, 0, 160,90), "Play"))
       	{
       		mTex.Play(); 
       		movieAS.Play();
       	}
       	if(GUI.Button(Rect(Screen.width*0.7, 0, 160,90), "Pause"))
       	{
       		mTex.Pause(); 
       		movieAS.Pause();
       	}
       	
}