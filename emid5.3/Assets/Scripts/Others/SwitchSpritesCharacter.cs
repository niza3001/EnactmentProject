using UnityEngine;
using System.Collections;
/// <summary>
/// Switches the character sprite from Boy(Default) - Girl 
/// </summary>
public class SwitchSpritesCharacter : MonoBehaviour {
    public int characterGrid;
    public Sprite girlHead;
    public Sprite girlTorso;

	// Use this for initialization
	void Start () {
        characterGrid = PlayerPrefs.GetInt("characterGrid");
        Debug.Log("character selected from playerprefs = " + characterGrid);
        if (characterGrid == 1)
        {
            GameObject head = GameObject.FindGameObjectWithTag("Head");
            head.GetComponent<SpriteRenderer>().sprite = girlHead;

            GameObject torso = GameObject.FindGameObjectWithTag("Torso");
            torso.GetComponent<SpriteRenderer>().sprite = girlTorso; 
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
