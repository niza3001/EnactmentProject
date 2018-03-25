using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class SlideData : MonoBehaviour {

    /*IN PROGRESS - this is a script attached to each slide to record which index of each object type it holds, as well as its recording data*/

    private AudioClip slideClip;
    private int charaIndex;
    private int backdropIndex;
    private int itemIndex;
    private AudioSource slideAudio;
    private int slidePose = 0;
    private bool isItem = false;
    private bool isChara = false;
    private bool isBackdrop = false;
	// Use this for initialization
	void Start () {
        slideAudio = gameObject.AddComponent<AudioSource>();
        slideClip = new AudioClip();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void startRecord()
    {
        slideAudio.clip = Microphone.Start(null, true, 300, 44100);
        Debug.Log("We have started");
    }

    public void endRecord()
    {
        int timeCut = Microphone.GetPosition(null);
        Microphone.End(null);

        float[] samples = new float[timeCut];
        slideAudio.clip.GetData(samples, 0);


        int freq = slideAudio.clip.frequency;
        slideAudio.clip = AudioClip.Create("SlideSound", samples.Length, 1, freq, false);
        slideAudio.clip.SetData(samples, 0);

        Debug.Log("We have stopped");

        if (slideAudio.clip == null) { Debug.Log("uuhm"); }

    }

    public void playAudio()
    {
        if (slideAudio.clip != null) {
            Debug.Log("well something is playing");
            slideAudio.Play();
           
        }
    }

    public void setChara(int ind)
    {
        charaIndex = ind;
        isChara = true;
    }

    public void setBackdrop(int ind)
    {
        backdropIndex = ind;
        isBackdrop = true;
    }

    public void setItem(int ind)
    {
        itemIndex = ind;
        isItem = true;
    }

    public int getChara()
    {
        return charaIndex;
    }

    public int getBackdrop()
    {
        return backdropIndex;
    }

    public int getItem()
    {
        return itemIndex;
    }

    public void updateEnactmentScreen()
    {
        Sprite chara = GameObject.FindGameObjectWithTag("object_arrays").GetComponent<ObjectArray>().CharaPoseSets[charaIndex].GetComponent<CharaPoses>().poses[slidePose];
        Sprite backdrop = GameObject.FindGameObjectWithTag("object_arrays").GetComponent<ObjectArray>().Backdrops[backdropIndex].GetComponent<Backdrop>().backdrop;
        GameObject item = GameObject.FindGameObjectWithTag("object_arrays").GetComponent<ObjectArray>().Items[backdropIndex];

       /* if (item!= GameObject.FindGameObjectWithTag("current_item"))
        {
            Destroy(GameObject.FindGameObjectWithTag("current_item"));
            GameObject newItem = (GameObject)Instantiate(item, item.GetComponent<Item>().getPosition(slidePose), transform.rotation);
            newItem.transform.localScale = new Vector3(newItem.GetComponent<Item>().getScale(), newItem.GetComponent<Item>().getScale(), newItem.GetComponent<Item>().getScale());
            newItem.tag = "current_item";
        }
        */
        GameObject.Find("EnactmentBackdrop").GetComponent<SpriteRenderer>().sprite = backdrop;
        GameObject.Find("EnactmentCharacter").GetComponent<Image>().sprite = chara;

    }

    public void setPose(int sp)
    {
        slidePose = sp;
    }

    public bool isFilled()
    {
        if (isChara==true && isItem == true && isBackdrop == true) { return true; }
        else { return false; }
    }

}
