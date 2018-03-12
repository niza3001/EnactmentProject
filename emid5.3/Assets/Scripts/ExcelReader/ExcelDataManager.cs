using UnityEngine;
using System.Collections;
using System; 
using System.Data; 
using System.Data.Odbc; 

public class ExcelDataManager : MonoBehaviour {
	
	// Variables 
	
	//Column indices based on the Motive xls output for one rigid body
	public Vector3 origin = Vector3.zero;
	public float scale = 1.0f; 
	public Vector3 rotationOffset = Vector3.zero;
	//public int RigidBodyId = 0; 
	private static ExcelDataManager instance;

	public static ExcelDataManager Instance
	{
		get { return instance; } 
	}


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
	private string excelFilePath = "Resources/Excel/LeftRightTest.xls"; 
	//public Vector3 position; 
	public bool readExcelData; 
	// Use this for initialization
	void Start () {
		excelFilePath = Application.dataPath + "/" + excelFilePath;
		Debug.Log ("File read = " + excelFilePath); 
		if(readExcelData)
			readXLS (excelFilePath);
		Debug.Log ("Row Count = " + dtYourData.Rows.Count);
	}
	
	// Update is called once per frame
	void Update () {
		//int currentFrameNumber = Time.frameCount; 
		//Vector3 pos = getPosition(currentFrameNumber);
		//Quaternion ori = getOrientation (currentFrameNumber);
		//Debug.Log ("pos :" + pos);
		//this.transform.position = pos; 
		//this.transform.rotation = ori;
		Quaternion q = getOrientation (6);
		Vector3 vec = getPosition (12);
	}
	
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


	public Quaternion getOrientation(int rigidBodyId)
	{
		int frameNumber = 1; 
		rigidBodyId--;
		if(dtYourData == null)
		{
			Debug.Log("Excel Data is NULL");
			return Quaternion.identity;
		}

		if(frameNumber < dtYourData.Rows.Count)
		{
			float rx,ry,rz,rw = 0.0f;

			Debug.Log(((int)Column.Rw).ToString());
			rx = float.Parse(dtYourData.Rows [frameNumber] [dtYourData.Columns [(int)Column.Rx+(rigidBodyId*7)+2].ColumnName].ToString());
			ry = float.Parse(dtYourData.Rows [frameNumber] [dtYourData.Columns [(int)Column.Ry+(rigidBodyId*7)+2].ColumnName].ToString());
			rz = float.Parse(dtYourData.Rows [frameNumber] [dtYourData.Columns [(int)Column.Rz+(rigidBodyId*7)+2].ColumnName].ToString());
			rw = float.Parse(dtYourData.Rows [frameNumber] [dtYourData.Columns [(int)Column.Rw+(rigidBodyId*7)+2].ColumnName].ToString());

			Quaternion ori = new Quaternion(rx,ry,rz,rw);
			Debug.Log(rx.ToString()+"ry"+ry.ToString()+"rw"+rw.ToString());
			//ori = ori*Quaternion.Euler(rotationOffset);
			return ori;
		}
		else 
		{
			Debug.Log("All the frames Read");

			return Quaternion.identity;
		}
	}

	public Vector3 getPosition(int rigidBodyId)
	{
		int frameNumber = 1; 
		rigidBodyId--;
		if(dtYourData == null)
		{
			Debug.Log("Excel Data is NULL");
			return Vector3.zero;
		}
		
		if(frameNumber < dtYourData.Rows.Count)
		{
			float px,py,pz = 0.0f;
			
			//Adding +2 to ignore Tme and SimTime columns 

			px = float.Parse(dtYourData.Rows [frameNumber] [dtYourData.Columns [(int)Column.Tx+(rigidBodyId*7)+2].ColumnName].ToString());
			py = float.Parse(dtYourData.Rows [frameNumber] [dtYourData.Columns [(int)Column.Ty+(rigidBodyId*7)+2].ColumnName].ToString());
			pz = float.Parse(dtYourData.Rows [frameNumber] [dtYourData.Columns [(int)Column.Tz+(rigidBodyId*7)+2].ColumnName].ToString());
			
			Vector3 pos = new Vector3(px,py,pz) ; 

			Debug.Log(px.ToString()+"py"+py.ToString()+"pz"+pz.ToString());
			//ori = ori*Quaternion.Euler(rotationOffset);
			return pos;
		}
		else 
		{
			Debug.Log("All the frames Read");
			
			return Vector3.zero;
		}
	}
	
	
}
