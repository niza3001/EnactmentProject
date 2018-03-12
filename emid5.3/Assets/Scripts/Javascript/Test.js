import System.Collections.Generic;
    
    var selGridInt : int = 0;
    var selStrings : String[] = ["Grid 1", "Grid 2", "Grid 3", "Grid 4"];
    public var mySkin:GUISkin; 
var myList : List.<String> = new List.<String>();
private var waveArray : List.< List.<String> > = new List.< List.<String> >();
	var iList; 
var rand = new Random();
private var builtInArray1 : String[]; 

function Start()
{
	for (var i = 0; i<3;i++)
	{
		var  subList : List.<String> = new List.<String>(); 
		for(var j=0; j<5;j++)
		{
			subList.Add(Random.value.ToString());
		}
		waveArray.Add(subList);
	}
	Debug.Log("Haha");
	Debug.Log(waveArray[0][0]);
	//DisplayList();
	
}


function OnGUI () {
    	        selGridInt = GUILayout.SelectionGrid (selGridInt, selStrings, 2);
    	        
    	        
}