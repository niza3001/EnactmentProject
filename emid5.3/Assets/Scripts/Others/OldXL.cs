using UnityEngine;
using System.Collections;
using System; 
using System.Data; 
using System.Data.Odbc; 

public class EXCELREADER : MonoBehaviour {


//	int linesToIgnore = 1;
	//Column names based on the Motive output for one rigid body
	enum Column
	{
		Frame =0,
		SimTime,
		Rx,
		Ry,
		Rz,
		Rw,
		Tx,
		Ty, 
		Tz
	};

	DataTable dtYourData;

	// Use this for initialization
	void Start () {
		Debug.Log (Application.dataPath + "/MotiveSmall.xls");
		readXLS(Application.dataPath + "/MotiveSmall.xls");
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log ("dATA"+dtYourData.Columns[1].ColumnName + " : " + dtYourData.Rows[Time.frameCount][dtYourData.Columns[1].ColumnName].ToString());
		Debug.Log (Time.frameCount);
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
		
		if(dtYourData.Rows.Count > 0) 
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
		} 

		//Debug.Log ("dATA"+dtYourData.Rows[7][dtYourData.Columns[2].ColumnName].ToString());

	}
	void OnGUI()
	{
		if (Time.time < 2.0f) {
						GUILayout.Label ("Frame Num:" + Time.frameCount);
				}
	}
}