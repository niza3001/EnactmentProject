//Finds the GameObject Screen0 and uses Fade.js to Fade in and out . 

#pragma strict

private var logo : GUITexture;

function Start()
{
	Debug.Log(Screen.currentResolution);
	//screenState0 = true; 
	 logo = GameObject.FindWithTag("Screen0").GetComponent.<GUITexture>();
	yield Fade.use.Alpha(logo, 0.0, 1.0, 1.0, EaseType.In);
	yield WaitForSeconds(2.5);
	yield Fade.use.Alpha(logo, 1.0, 0.0, 1.0, EaseType.Out);
	Application.LoadLevel(9);
	
}
function Update()
{
	
	if(Input.GetMouseButtonDown(0))
	{
	//Take them to Load Game New Game Screen 
	Application.LoadLevel(9);
	}
}


