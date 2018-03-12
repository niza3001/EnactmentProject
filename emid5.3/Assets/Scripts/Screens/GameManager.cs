using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System;

[Serializable, XmlRoot("GameManager")]
public class GameManager : MonoBehaviour 
{
    public Texture2D[] rbdTexture; 
	public Texture2D[] bgTexture;
    public DimeData dd;
    public RigidBodyDirectory xmlDataList;
    public int[] KenneyArray;

    public enum transformType
    {
        Excel = 0,
        Optitrack,
        Kinect
    };
    public transformType typeOfTransform;

    void Awake()
	{
		DontDestroyOnLoad (this);
	}
    void loadRBDTextureList()
    {
        
        if (xmlDataList.rigidBodyList.Count > 0)
        {
            int i = 0; 
            for( i = 0; i < xmlDataList.rigidBodyList.Count; i++)
            {
                if (File.Exists(xmlDataList.rigidBodyList[i].imgFilePath))
                {
                    rbdTexture[16 + i] = LoadPNG(xmlDataList.rigidBodyList[i].imgFilePath);
                }
            }
            Array.Resize(ref rbdTexture, 16 + i);
        } 

    }

    void loadBGTextureList()
    {
        if(!Directory.Exists(Application.dataPath+"\\BackgroundImages"))
        {
            Directory.CreateDirectory(Directory.GetParent(Application.dataPath) + "\\BackgroundImages");
        }
        int i;
        for( i=30; i>0; i--)
        {
            if(File.Exists(Directory.GetParent(Application.dataPath)+"\\BackgroundImages\\"+i.ToString()+".png")) 
            {
                bgTexture = new Texture2D[i+1];
                break;
            }               
        }
        for (int j=0; j <= i; j++)
        {
            bgTexture[j] = LoadPNG(Directory.GetParent(Application.dataPath) + "\\BackgroundImages\\" + j.ToString() + ".png");
            
        }

        return; 
    }
    // Use this for initialization
    void Start () 
	{
        loadRBDFromXml();
        loadRBDTextureList();

        bgTexture = new Texture2D[30];
        loadBGTextureList();
         

        dd = new DimeData();  
		//numberOfTabs = 16; 
		dd.BIGList = new List<string> ();
		for(int i =0; i <dd.numberOfTabs; i++)
		{
			dd.BIGList.Add("");
		}
		Application.LoadLevel (1);
        //clearScreenShotsFolder();
	}
	
	// Update is called once per frame
	void Update () 
	{
        
	}
    /// <summary>
    /// Loads App.dataPath "/RBD/rigidBodyInfo.xml" file and deserializes it. 
    /// </summary>
    void loadRBDFromXml()
    {
        if (!Directory.Exists(Directory.GetParent(Application.dataPath) + "/GraphicObjects/"))
            Directory.CreateDirectory(Directory.GetParent(Application.dataPath) + "/GraphicObjects/");
        if (File.Exists(Directory.GetParent(Application.dataPath) + "/GraphicObjects.xml"))
        {
            XmlSerializer ds = new XmlSerializer(typeof(RigidBodyDirectory));
            FileStream file = File.Open(Directory.GetParent(Application.dataPath) + "/GraphicObjects.xml", FileMode.Open);
            object obj = ds.Deserialize(file);
            xmlDataList = (RigidBodyDirectory)obj;
            file.Close();
        }
    }

    /// <summary>
    /// Returns a Texture2D and takes in FilePath for PNG , uses System.IO
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public Texture2D LoadPNG(string filePath)
    {
        Texture2D tex = new Texture2D(2, 2);
        byte[] data = File.ReadAllBytes(filePath);
        if (data == null)
            Debug.Log("BAD FILE PATH");
        else
        {

            tex.LoadImage(data);
        }
        return tex;
    }

}



[Serializable, XmlRoot("DimeData")]
public class DimeData
{
    [XmlElement("BIGList")]
    public List<string> BIGList = new List<string>(); //Stores all the filePaths for videos 
    [XmlElement("TotalTabs")]
    public int numberOfTabs = 16; 

    //Playerprefs PlayerPrefs.SetString("dimeFolder",dimeFolder);
    [XmlElement("PlayerPrefs")]
    public string dimeFile;
    public string dimeFolder;
    public string s1;
    public string s2;
    public string storyTitle; 
    
}


