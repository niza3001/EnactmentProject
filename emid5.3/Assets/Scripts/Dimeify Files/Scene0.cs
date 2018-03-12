//This displays the FileBrowsers and let's the User select .Dinme file to load. 

using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;
using System.Globalization;
using System.Xml;
using System.Xml.Serialization;
//using System.String; 

public class Scene0 : MonoBehaviour {
	//public string filePath = "";
	//private Rect labelRect = new Rect (Screen.width*0.05f, Screen.height*0.05f, Screen.width*0.1f, Screen.height*0.1f);
    string statusMessage; 
	string filePath;
    private Rect rctWindow1;
    private string fileBrowserPath; 
	// Use this for initialization
	void Start () 
    {
        PlayerPrefs.SetInt("FileBrowserDisplay", 0);
        PlayerPrefs.SetString("FileBrowserPath",string.Empty);
        rctWindow1 = new Rect(Screen.width * 0.05f, Screen.height * 0.02f, Screen.width * 0.9f, Screen.height * 0.25f);
        fileBrowserPath = "";
        filePath = "";
        statusMessage = "";
		//myDataList = RigidBodyContainer.Load(Application.dataPath + "/Dime/rigidBodyInfo.dime");
	}

	// Update is called once per frame
	void Update () 
    {
	
	}

	void OnGUI()
	{
		//GUI.skin = customSkin;
		rctWindow1 = GUI.Window(0, rctWindow1, DoMyWindow, "Load Game", GUI.skin.GetStyle("window"));

	}//End OnGUI

	void DoMyWindow(int windowID)
	{
        fileBrowserPath = PlayerPrefs.GetString("FileBrowserPath", fileBrowserPath);
        if (statusMessage != "")
        {
            GUI.Label(new Rect(Screen.width * 0.05f, Screen.height * 0.2f, Screen.width * 0.15f, Screen.height * 0.05f), statusMessage);
        }
		//GUI.skin = customSkin;
		GUI.Label (new Rect (Screen.width * 0.05f, Screen.height * 0.05f, Screen.width * 0.1f, Screen.height * 0.07f), "Select a .dime file");
        
		if (fileBrowserPath != "") 
        {
			filePath = fileBrowserPath;
		}
		
		filePath = GUI.TextField (new Rect (Screen.width * 0.15f, Screen.height * 0.05f, Screen.width * 0.45f, Screen.height * 0.07f), filePath, 180);
		
        if (GUI.Button (new Rect (Screen.width * 0.60f, Screen.height * 0.05f, Screen.width * 0.1f, Screen.height * 0.07f), "Browse")) 
        {			
			PlayerPrefs.SetInt ("FileBrowserDisplay", 1);				
		}
			
		
		if (GUI.Button (new Rect (Screen.width * 0.35f, Screen.height * 0.15f, Screen.width * 0.15f, Screen.height * 0.05f), "Submit")) 
		{				
				
			if (Path.GetExtension (filePath) != ".story") {
				statusMessage = "Please select a file with .story extension";
				PlayerPrefs.SetString ("FileBrowserPath", "");
				return;
			}
            else if (File.Exists(filePath) == false)
            {
                statusMessage = "The File doesn't exist, Try again";
                PlayerPrefs.SetString("FileBrowserPath", "");
                return;
            }
            else
            {
                //Create a copy of these videos in another directory 
                DirectoryInfo sourceDirectoryInfo = Directory.GetParent(filePath);
                //string sourcePath = Path.GetDirectoryName(filePath); 

                string timeString = System.DateTime.Now.Hour.ToString()+System.DateTime.Now.Minute.ToString();
                string destinationDirectory = sourceDirectoryInfo.Parent.FullName+"\\"+timeString;

                string newXmlFile = destinationDirectory + "\\" + Path.GetFileName(filePath);

                
                DirectoryCopy(sourceDirectoryInfo.FullName, destinationDirectory, true); 
                //Rename XML file




                PlayerPrefs.SetString("FileBrowserPath", newXmlFile);
                Application.LoadLevel(5); 
            }

		}
        if (GUI.Button(new Rect(Screen.width * 0.20f, Screen.height * 0.15f, Screen.width * 0.15f, Screen.height * 0.05f), "Reset"))
            Application.LoadLevel(Application.loadedLevel);

        if (GUI.Button(new Rect(Screen.width * 0.05f, Screen.height * 0.15f, Screen.width * 0.15f, Screen.height * 0.05f), "Back"))
        {
            Application.LoadLevel(Application.loadedLevel-1); 
        }
		
	}
    /*
    void SerializeGameManagerToXml()
    {
        try
        {
            myGameManager.GetComponent<GameManager>().dd.s1 = PlayerPrefs.GetString("storyTeller1");
            myGameManager.GetComponent<GameManager>().dd.s2 = PlayerPrefs.GetString("storyTeller2");
            myGameManager.GetComponent<GameManager>().dd.storyTitle = PlayerPrefs.GetString("storyTitle");
            myGameManager.GetComponent<GameManager>().dd.dimeFile = PlayerPrefs.GetString("dimeFile");
            myGameManager.GetComponent<GameManager>().dd.dimeFolder = PlayerPrefs.GetString("dimeFolder");


            XmlSerializer serializer = new XmlSerializer(typeof(DimeData));
            FileStream fileStream = File.Open(dimeFile, FileMode.OpenOrCreate);
            serializer.Serialize(fileStream, (DimeData)myGameManager.GetComponent<GameManager>().dd);
            fileStream.Close();
        }
        catch (Exception ex)
        {
            Debug.Log("Exception during Serializing File:" + dimeFile);
            Debug.Log(ex);
        }
    }
    private void DeserializeXml(string FileBrowserPath)
    {
        try
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(DimeData));
            FileStream fileStream = File.Open(FileBrowserPath, FileMode.Open);
            myGameManager.GetComponent<GameManager>().dd = (DimeData)deserializer.Deserialize(fileStream);
            fileStream.Close();

            PlayerPrefs.SetString("storyTeller1", myGameManager.GetComponent<GameManager>().dd.s1);
            PlayerPrefs.SetString("storyTeller2", myGameManager.GetComponent<GameManager>().dd.s2);
            PlayerPrefs.SetString("storyTitle", myGameManager.GetComponent<GameManager>().dd.storyTitle);
            PlayerPrefs.SetString("dimeFile", myGameManager.GetComponent<GameManager>().dd.dimeFile);
            PlayerPrefs.SetString("dimeFolder", myGameManager.GetComponent<GameManager>().dd.dimeFolder);

            //You don't wanna Deserialize EVERY Time so set FileBrowserPath to "" 
            PlayerPrefs.SetString("FileBrowserPath", "");
        }
        catch (Exception ex)
        {
            Debug.Log("Exception during DESerializing File:" + dimeFile);
            Debug.Log(ex);
        }
    }*/
    /// <summary>
    /// Copies ALL Files in Source Folder to Destination Folder 
    /// </summary>
    /// <param name="sourceDirName"></param>
    /// <param name="destDirName"></param>
    /// <param name="copySubDirs"></param>
    private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
    {
        // Get the subdirectories for the specified directory.
        DirectoryInfo dir = new DirectoryInfo(sourceDirName);
        DirectoryInfo[] dirs = dir.GetDirectories();

        if (!dir.Exists)
        {
            throw new DirectoryNotFoundException(
                "Source directory does not exist or could not be found: "
                + sourceDirName);
        }

        // If the destination directory doesn't exist, create it. 
        if (!Directory.Exists(destDirName))
        {
            Directory.CreateDirectory(destDirName);
        }

        // Get the files in the directory and copy them to the new location.
        FileInfo[] files = dir.GetFiles();
        foreach (FileInfo file in files)
        {
            string temppath = Path.Combine(destDirName, file.Name);
            file.CopyTo(temppath, false);
        }

        // If copying subdirectories, copy them and their contents to new location. 
        if (copySubDirs)
        {
            foreach (DirectoryInfo subdir in dirs)
            {
                string temppath = Path.Combine(destDirName, subdir.Name);
                DirectoryCopy(subdir.FullName, temppath, copySubDirs);
            }
        }
    }
}


