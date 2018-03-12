
using UnityEngine;
using System;
using System.Collections;
using OptitrackManagement;
using System.Data; 
using System.Data.Odbc;
public enum TransformType
{
    Optitrack = 0,
    Excel,
    Kinect
}


public class TransformManager : MonoBehaviour 
{
    
    //public bool optiTrackOn;
    public TransformType currentTransform; 
    public string myName;
	public float scale = 1.0f;
	private static TransformManager instance;
	public Vector3 origin = Vector3.zero; // set this to wherever you want the center to be in your scene

	private Quaternion tempOrientation; 
	private Vector3 tempPosition; 

	//Excel variables 
	private enum Column
	{
		
		Rx = 0,
		Ry = 1,
		Rz,
		Rw,
		Tx,
		Ty, 
		Tz
	};
	private DataTable dtYourData; //Stores Entire Data 
	private string excelFilePath = "/Resources/Excel/LeftRightTest.xls";
    private GameObject myGameManager; 


    public static TransformManager Instance
	{
		get { return instance; } 
	}

	void Awake()
	{
		instance = this;
	}

	~TransformManager ()
	{      
		Debug.Log("OptitrackManager: Destruct");
		OptitrackManagement.DirectMulticastSocketClient.Close();
	}
    int sceneStartCount; 
	void Start () 
	{
        //Debug.LogError("Start() func was called from TransformManager.cs .");
        sceneStartCount = Time.frameCount;
		tempPosition = Vector3.zero; 
		tempOrientation = Quaternion.identity;
        myGameManager = GameObject.FindGameObjectWithTag("GameManager") as GameObject;
        if (myGameManager.GetComponent<RUISInputManager>().enableKinect2 && myGameManager.GetComponent<RUISInputManager>().enablePSMove)
        {
            currentTransform = TransformType.Kinect; 
        }

        if (currentTransform == TransformType.Optitrack) 
		{
			Debug.Log (myName + ": Initializing");
			OptitrackManagement.DirectMulticastSocketClient.Start ();
			Application.runInBackground = true;
		}
        else if (currentTransform == TransformType.Excel) // USE EXCEL DATA 
		{
			excelFilePath = Application.dataPath + "/" + excelFilePath;
			Debug.Log ("File read = " + excelFilePath); 
			readXLS (excelFilePath);
			Debug.Log ("Row Count = " + dtYourData.Rows.Count);
		}
        else if (currentTransform == TransformType.Kinect)
        {
            /*
			GameObject kinect = GameObject.FindWithTag("Kinect");
            CubemanController cc = (CubemanController)kinect.GetComponent<CubemanController>();
            cc.enabled = true;
			*/
		}

	}
	// Update is called once per frame
	void Update () 
	{
		
	}

#region OPTITRACK FUNCTIONS 
	public OptiTrackRigidBody getOptiTrackRigidBody(int index)
	{
		// only do this if you want the raw data
		if(OptitrackManagement.DirectMulticastSocketClient.IsInit())
		{
			DataStream networkData = OptitrackManagement.DirectMulticastSocketClient.GetDataStream();
            //NZ 3/3/18
            //Debug.Log(networkData.getRigidbody(1));
            //Debug.Log("My dummy log");
			return networkData.getRigidbody(index);
		}
		else
		{
			OptitrackManagement.DirectMulticastSocketClient.Start();
			return getOptiTrackRigidBody(index);
		}
	}

	public Vector3 getPosition(int rigidbodyIndex)
	{
		if(OptitrackManagement.DirectMulticastSocketClient.IsInit())
		{
            
			DataStream networkData = OptitrackManagement.DirectMulticastSocketClient.GetDataStream();
            // OO* 1/29/2018
            
            
            Vector3 pos = origin + networkData.getRigidbody(rigidbodyIndex).position * scale;
            // OO* 1/29/2018
            //Debug.LogError(pos);
            //pos.x = -pos.x; // not really sure if this is the best way to do it
            //pos.y = pos.y; // these may change depending on your configuration and calibration
            //pos.z = -pos.z;
            //Debug.Log("Position is "+networkData.getRigidbody(rigidbodyIndex).position.ToString());
            return pos;


		}
		else
		{
			return Vector3.zero;
		}
	}

	public Quaternion getOrientation(int rigidbodyIndex)
	{
		// should add a way to filter it
		if(OptitrackManagement.DirectMulticastSocketClient.IsInit())
		{
			DataStream networkData = OptitrackManagement.DirectMulticastSocketClient.GetDataStream();
			Quaternion rot = networkData.getRigidbody(rigidbodyIndex).orientation;

			// change the handedness from motive
			//rot = new Quaternion(rot.z, rot.y, rot.x, rot.w); // depending on calibration
			// Invert pitch and yaw
			//Vector3 euler = rot.eulerAngles;
			//rot.eulerAngles = new Vector3(euler.x, -euler.y, euler.z); // these may change depending on your calibration

			return rot;
		}
		else
		{
			return Quaternion.identity;
		}
	}

	public void DeInitialize()
	{
		OptitrackManagement.DirectMulticastSocketClient.Close();
	}
#endregion

#region EXCEL FUNCTIONS 
	void readXLS( string filetoread)
	{
		// Must be saved as excel 2003 workbook, not 2007, mono issue really
		string con = "Driver={Microsoft Excel Driver (*.xls)}; DriverId=790; Dbq="+filetoread+";";
		Debug.Log(con);
		string yourQuery = "SELECT * FROM [Sheet1$]"; 
		// our odbc connector 
		OdbcConnection oCon = new OdbcConnection(con); 
		// our command object 
		OdbcCommand oCmd = new OdbcCommand(yourQuery, oCon);
		// table to hold the data 
		dtYourData = new DataTable("YourData"); 
		// open the connection 
		oCon.Open(); 
		// lets use a datareader to fill that table! 
		OdbcDataReader rData = oCmd.ExecuteReader(); 
		// now lets blast that into the table by sheer man power! 
		dtYourData.Load(rData); 
		// close that reader! 
		rData.Close(); 
		// close your connection to the spreadsheet! 
		oCon.Close(); 
		// wow look at us go now! we are on a roll!!!!! 
		// lets now see if our table has the spreadsheet data in it, shall we? 
		
		/*if(dtYourData.Rows.Count > 0) 
		{ 
			// do something with the data here 
			// but how do I do this you ask??? good question! 
			for (int i = 0; i < dtYourData.Rows.Count; i++) 
			{ 
				// for giggles, lets see the column name then the data for that column! 
				Debug.Log(dtYourData.Columns[0].ColumnName + " : " + dtYourData.Rows[i][dtYourData.Columns[0].ColumnName].ToString() + 
				          "  |  " + dtYourData.Columns[1].ColumnName + " : " + dtYourData.Rows[i][dtYourData.Columns[1].ColumnName].ToString() + 
				          "  |  " + dtYourData.Columns[2].ColumnName + " : " + dtYourData.Rows[i][dtYourData.Columns[2].ColumnName].ToString()); 
			} 
		} */
		
		//Debug.Log ("dATA"+dtYourData.Rows[7][dtYourData.Columns[2].ColumnName].ToString());
		
	}
	
	
	public Quaternion getOrientationExcel(int rigidBodyId)
	{
        int frameNumber = Time.frameCount - sceneStartCount; 
		rigidBodyId--;
		if(dtYourData == null)
		{
			//Debug.Log("Excel Data is NULL");
			return Quaternion.identity;
		}
		
		if(frameNumber < dtYourData.Rows.Count)
		{
			float rx,ry,rz,rw = 0.0f;
			Quaternion ori; 
			//Debug.Log(((int)Column.Rw).ToString());
			rx = float.Parse(dtYourData.Rows [frameNumber] [dtYourData.Columns [(int)Column.Rx+(rigidBodyId*7)+2].ColumnName].ToString());
			ry = float.Parse(dtYourData.Rows [frameNumber] [dtYourData.Columns [(int)Column.Ry+(rigidBodyId*7)+2].ColumnName].ToString());
			rz = float.Parse(dtYourData.Rows [frameNumber] [dtYourData.Columns [(int)Column.Rz+(rigidBodyId*7)+2].ColumnName].ToString());
			rw = float.Parse(dtYourData.Rows [frameNumber] [dtYourData.Columns [(int)Column.Rw+(rigidBodyId*7)+2].ColumnName].ToString());

			float lowerLimit = 0.0001f; 
			if(rx < lowerLimit && ry < lowerLimit && rz < lowerLimit && rx > -lowerLimit && ry > -lowerLimit && rz > -lowerLimit)
				ori = tempOrientation; 
			else
			{
				ori = new Quaternion(rx,ry,rz,rw);
				Vector3 euler = ori.eulerAngles;
				ori.eulerAngles = new Vector3(euler.x, -euler.y, euler.z);

				tempOrientation = ori; 
			}
			//Debug.Log(rx.ToString()+"ry"+ry.ToString()+"rw"+rw.ToString());
			//ori = ori*Quaternion.Euler(rotationOffset);

			return ori;
		}
		else 
		{
			//Debug.Log("All the frames Read");
			
			return Quaternion.identity;
		}
	}
	
	public Vector3 getPositionExcel(int rigidBodyId)
	{
        int frameNumber = Time.frameCount - sceneStartCount; 
		rigidBodyId--;
		if(dtYourData == null)
		{
			Debug.Log("Excel Data is NULL");
			return Vector3.zero;
		}
		
		if(frameNumber < dtYourData.Rows.Count)
		{
			float px,py,pz = 0.0f;
			Vector3 pos ; 
			//Adding +2 to ignore Tme and SimTime columns 
			
			px = -float.Parse(dtYourData.Rows [frameNumber] [dtYourData.Columns [(int)Column.Tx+(rigidBodyId*7)+2].ColumnName].ToString());
			py = float.Parse(dtYourData.Rows [frameNumber] [dtYourData.Columns [(int)Column.Ty+(rigidBodyId*7)+2].ColumnName].ToString());
			pz = float.Parse(dtYourData.Rows [frameNumber] [dtYourData.Columns [(int)Column.Tz+(rigidBodyId*7)+2].ColumnName].ToString());
			float lowerLimit = 0.0001f; 
			if(px < lowerLimit && py < lowerLimit && pz < lowerLimit && px > -lowerLimit && py > -lowerLimit && pz > -lowerLimit)
				pos = tempPosition ; 
			else 
			{
				pos = new Vector3(px,py,pz);
				tempPosition = pos; 
			}
			//Debug.Log(px.ToString()+"py"+py.ToString()+"pz"+pz.ToString());
			//ori = ori*Quaternion.Euler(rotationOffset);
			return pos;
		}
		else 
		{
			Debug.Log("All the frames Read");
			
			return Vector3.zero;
		}
    }
#endregion

}