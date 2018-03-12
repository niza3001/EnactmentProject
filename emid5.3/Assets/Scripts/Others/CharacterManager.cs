using UnityEngine;
using System.Collections;

public class CharacterManager : MonoBehaviour {

    public GameObject[] characters;
    private int characterGrid;

    private GameObject character; 
	// Use this for initialization
	void Start () 
    {
        characterGrid = PlayerPrefs.GetInt("characterGrid");
        if (characterGrid != 2)
        {
            if (characterGrid <= 1)
            {
                // character = (GameObject)Instantiate(characters[0], new Vector3(0, 0, 0), Quaternion.identity);
                GameObject kenney = GameObject.FindGameObjectWithTag("Kenney") as GameObject;
                if(kenney!= null)
                    kenney.SetActive(false);
                GameObject man = GameObject.FindGameObjectWithTag("Man") as GameObject;
                if(man != null)
                    man.SetActive(false);

            }
            else
            {
                //character = (GameObject)Instantiate(characters[1], new Vector3(0, 0, 0), Quaternion.identity); 
                GameObject boy = GameObject.FindGameObjectWithTag("Boy") as GameObject;
                if(boy !=null)boy.SetActive(false);
                GameObject man = GameObject.FindGameObjectWithTag("Man") as GameObject;
                if(!man)man.SetActive(false);
            }
        }
        else
        {
            //3D Character Mannequin
            GameObject kenney = GameObject.FindGameObjectWithTag("Kenney") as GameObject;
            kenney.SetActive(false);
            GameObject boy = GameObject.FindGameObjectWithTag("Boy") as GameObject;
            boy.SetActive(false);
            /*
            GameObject man = GameObject.FindGameObjectWithTag("Man") as GameObject;
            man.SetActive(true);
            */
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
