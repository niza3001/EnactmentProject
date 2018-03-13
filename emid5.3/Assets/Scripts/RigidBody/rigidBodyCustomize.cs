using UnityEngine;
using System.Collections;

enum BigBoxObject 
{
	Barrel, 
	Basket, 
	Box, 
	HandBag, 
	ParamedicBag, 
	Present,
	Treasure
}


public class rigidBodyCustomize : MonoBehaviour {
    public bool switchingOn; 
	public int _rigidBodyIndex;
    private RigidBodyData xmlRBDData; 
	private RigidBodyManager rbm ;
    private OptitrackStreamingClient streamClient; // Streaming Client
    private GameObject[] objectsRead;
    public Material standardMaterial;
    public Material transparentMaterial;
    public float magicScale = 1.0f; // Comes from inputConfig.Xml set by the user 
    private GameObject mesh, myGameManager;
    public bool enableOptitrack=false; // Replacement flag for RUIS

	//Use this for initialization
	void Start () 
	{
        //Debug.LogError("Start() being called from rigidBodyCustomize.cs");
        //objectsRead = new GameObject[1]; 
        myGameManager = GameObject.FindGameObjectWithTag("GameManager") as GameObject;
        
        // AddComponent<TransformManager>();
        _rigidBodyIndex = PlayerPrefs.GetInt ("rigidBodyGrid");

        //OO* 1/29/2018
        //Debug.LogError("Value of rigidBodyIndex:");
        //Debug.LogError(_rigidBodyIndex);
        // OO* 1/29/2018
        //NZ 3/3/18
        //_rigidBodyIndex = 1;
        if (_rigidBodyIndex > 0 && _rigidBodyIndex <=15)
		{
            Debug.Log("Rigid body index is greater than 0 and less than or equal to 15.");
            rbm = (RigidBodyManager)GameObject.FindGameObjectWithTag ("RBM").GetComponent<RigidBodyManager>();
            streamClient = (OptitrackStreamingClient)GameObject.FindGameObjectWithTag("Client").GetComponent<OptitrackStreamingClient>();

            InstantiateMesh ();
		}
        if (_rigidBodyIndex != 0 && _rigidBodyIndex >= 15)
        {
            Debug.Log("Code not reached");
            xmlRBDData = myGameManager.GetComponent<GameManager>().xmlDataList.rigidBodyList[_rigidBodyIndex - 16];
            // OO* 1/29/2018
            Debug.LogError(xmlRBDData);
            if (xmlRBDData.materialIncluded && standardMaterial != null && transparentMaterial != null && xmlRBDData != null)
                
                objectsRead = ObjReader.use.ConvertFile(xmlRBDData.objFilePath, false, standardMaterial, transparentMaterial);
            else
                objectsRead = ObjReader.use.ConvertFile(xmlRBDData.objFilePath, false, standardMaterial, transparentMaterial);

            if (objectsRead != null)
            {
                mesh = objectsRead[0];
                mesh.transform.localPosition.Set(xmlRBDData.tx, xmlRBDData.ty, xmlRBDData.tz);
                mesh.transform.localRotation = Quaternion.Euler(xmlRBDData.rx, xmlRBDData.ry, xmlRBDData.rz);
                mesh.transform.localScale.Set(xmlRBDData.sx, xmlRBDData.sy, xmlRBDData.sz);

            }
            if (myGameManager.GetComponent<RUISInputManager>().enableKinect2 && myGameManager.GetComponent<RUISInputManager>().enablePSMove)
            {
                mesh.AddComponent<RUISPSMoveWand>().enabled = true;
            }
            else
                mesh.AddComponent<OptiTrackObject>().enabled = true;
        }

	}
    
	void InstantiateMesh()
	{
        //Debug.LogError("Entering InstantiateMesh()");
        int children = transform.childCount;
        for (int i = children - 1; i >= 0; i--)
        {
            GameObject.Destroy(transform.GetChild(i).gameObject);
        }        
		mesh = (GameObject)Instantiate (rbm.rigidBodyMesh[(int)_rigidBodyIndex], this.transform.position, Quaternion.identity);
		mesh.transform.parent = transform;
		mesh.transform.rotation = transform.rotation;
        mesh.transform.localScale = new Vector3(magicScale, magicScale, magicScale);
        //RUISInputManager go = new RUISInputManager();
        //go = myGameManager.GetComponent<RUISInputManager>(); 
        mesh.transform.localScale = myGameManager.GetComponent<RUISInputManager>().rbdXmlScale[(int)_rigidBodyIndex];
        /*if (myGameManager.GetComponent<RUISInputManager>().enableKinect2 && myGameManager.GetComponent<RUISInputManager>().enablePSMove)
        {
            // OO* 1/29/2018
            //Debug.LogError("PSMove was selected.");
            mesh.GetComponent<RUISPSMoveWand>().enabled = true;
            mesh.GetComponent<OptiTrackObject>().enabled = false; 
        }
        else
        {
            // OO* 1/29/2018
           // Debug.LogError("Optitrack was selected.");
            mesh.GetComponent<RUISPSMoveWand>().enabled = false;
            mesh.GetComponent<OptiTrackObject>().enabled = true; 
        }*/

        if(enableOptitrack)
        {
            Debug.Log("Only Optitrack Flow.");
            mesh.GetComponent<OptitrackRigidBody>().StreamingClient = streamClient;
            mesh.GetComponent<OptitrackRigidBody>().enabled = true;
        }
    }
	// Update is called once per frame
	void Update () 
	{
        //Debug.LogError("rigidBodyCustomize.cs has updated.");
    }
	void OnGUI()
	{
//        Debug.Log(transform.childCount.ToString());
        if (switchingOn)//Change Meshes for Testing Purposes 
        {
            if (GUI.Button(new Rect(0f, 100f, 160f, 90f), "SwitchMesh"))
            {
                _rigidBodyIndex++;
                InstantiateMesh();
            }
        }
	}
    /// <summary>
    /// returns the Scale of a single rigid body from XML
    /// </summary>
    Vector3 fetchScaleFromXml(int rbdId)
    {
        
        return myGameManager.GetComponent<RUISInputManager>().rbdXmlScale[rbdId];
    }
}
