using UnityEngine;
using System.Collections;

public class ChangeBackground : MonoBehaviour {
	public Texture2D[] bgTexture; 
	public Texture2D[] pptSlides; 
	private GameObject myGameManager ; 
	private int slideNumber; 
	private int backgroundSelected; 
	private bool thesisOn; 
	// Use this for initialization
	void Start () {
		backgroundSelected = PlayerPrefs.GetInt ("backgroundGrid");
		myGameManager = GameObject.FindGameObjectWithTag("GameManager") as GameObject; 
		thesisOn = false; 
		//this.renderer.material.mainTexture = myGameManager.GetComponent<GameManager> ().bgTexture [1];
		slideNumber = 0; 
		this.GetComponent<Renderer>().material.mainTexture = pptSlides [slideNumber];
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!thesisOn)
		{
			this.GetComponent<Renderer>().material.mainTexture = myGameManager.GetComponent<GameManager>().bgTexture[backgroundSelected];
			if(Input.GetKeyDown(KeyCode.Return))
			{
				thesisOn = true; 
			}
		}
		if(thesisOn)
			runThesisDefense (); 
			

	}
	void OnGUI()
	{

	}
	void runThesisDefense()
	{
		if(Input.GetKeyDown(KeyCode.LeftArrow))
		{
			Debug.Log (slideNumber);
			if(slideNumber==0)
				slideNumber = 0; 
			else 
				slideNumber--;
		}
		else if(Input.GetKeyDown(KeyCode.RightArrow))
		{
			Debug.Log (slideNumber);
			if(slideNumber == pptSlides.Length-1)
				slideNumber = pptSlides.Length-1; 
			else 
				slideNumber++;
			
		}
		else if(Input.GetKeyDown(KeyCode.UpArrow))
		{
			slideNumber = 0; 
		}
		else if(Input.GetKeyDown(KeyCode.DownArrow))
		{
			slideNumber = pptSlides.Length-1;
		}
		this.GetComponent<Renderer>().material.mainTexture = pptSlides [slideNumber];
	}
}
