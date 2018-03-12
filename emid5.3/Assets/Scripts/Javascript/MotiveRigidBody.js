
public class MotiveRigidBody
{
var id: String; 
var filePath : String;

var tx : float;
var ty : float;
var tz : float;

var rx : float;
var ry : float;
var rz : float; 
var rw : float; //Since it's a Quaternion, we need w as well! 

var positionVector : Vector3; 
var startQuat : Quaternion; 
var targetQuat : Quaternion; 

public function MotiveRigidBody(i:String, fp:String) //Takes rigidBodyID and Location of FilePath of mocap.txt 
{
	id = i;
	filePath = fp; 
	
	getTransformUpdates();
	startQuat = new Quaternion(0.0,0.0,0.0,1.0);
	targetQuat = new Quaternion(rx,ry,rz,rw);
	positionVector = new Vector3(tx,ty,tz);
}
public function getTransformUpdates()
{
	var sr = new File.OpenText(filePath);
	var returnChar = "X"[0];
    var commaChar = ","[0];
    
    
	startQuat.Set(rx,ry,rz,rw); //Store the Old Quaternion 
    
    if(sr != null)
    {
    	var buildDataPairs = new ArrayList();
     	var input : String = "";
     	
         var inputLine = sr.ReadLine();
         sr.Close();
        
		var dataLines = inputLine.Split(returnChar);
		 
		 for (var dataLine in dataLines) 
		 {
       		  var dataPair = dataLine.Split(commaChar);
       		  //Debug.Log(dataLine);
       		if(dataPair[0] == id )
       		{			
       		 
       		   rx = float.Parse(dataPair[1]);
       		  ry = float.Parse(dataPair[2]);
       		  rz = float.Parse(dataPair[3]);
       		  rw = float.Parse(dataPair[4]);
       		  
       		  tx = float.Parse(dataPair[5]);
       		  ty = float.Parse(dataPair[6]);
       		  tz = float.Parse(dataPair[7]);

       		 }//end of if        	  
    	 }//end for 
		
 	 }//close streamReader
 	 
	 targetQuat.Set(rx,ry,rz,rw); 
	 positionVector.Set(tx,ty,tz);
	
}
}