using UnityEngine;
using System.Collections;

public class SwitchSprites : MonoBehaviour {

	public Sprite[] spriteArray = new Sprite[10]; 
	public int partId;
    private GameObject myGameManager; 
	private SpriteRenderer sr ;
	// Use this for initialization
	void Start () {
		//Initialize 
		sr = this.GetComponent<SpriteRenderer>();
		//go = (GameObject)GameObject.FindWithTag("SpriteManager");
		//sm = go.GetComponent<SpriteManager>();
		//Draw Sprite 
        if (Application.loadedLevelName == "OptitrackScreen")
        {
            myGameManager = GameObject.FindGameObjectWithTag("GameManager") as GameObject;
            updateSpriteArray();
            sr.sprite = spriteArray[SpriteManager.counterArray[partId] % spriteArray.Length];
        }
        else
        {
            sr.sprite = spriteArray[SpriteManager.counterArray[SpriteManager.selGridInt] % spriteArray.Length];
        }
	}
    /// <summary>
    /// Update the Sprites from PlayerPrefs  Or GameManager Class
    /// </summary>
    void updateSpriteArray()
    {
        if (Application.loadedLevelName == "OptitrackScreen")
        {
            SpriteManager.counterArray = myGameManager.GetComponent<GameManager>().KenneyArray;
        }
    }
	
	// Update is called once per frame
	void Update () {
        //In GUI Selection Mode
        if (Application.loadedLevelName != "OptitrackScreen")
        {
            if (partId == SpriteManager.selGridInt)
            {
                sr.sprite = spriteArray[SpriteManager.counterArray[SpriteManager.selGridInt] % spriteArray.Length];
                //if(SpriteManager.counterArray[SpriteManager.selGridInt] == spriteArray.Length)
                //	SpriteManager.counterArray[SpriteManager.selGridInt] = 0;

            }
        }
	}
	void OnGUI()
	{

		//GUI.Label(new Rect(150.0f,0.0f,100.0f,100.0f),SpriteManager.selGridInt.ToString());		
		
	}
}
