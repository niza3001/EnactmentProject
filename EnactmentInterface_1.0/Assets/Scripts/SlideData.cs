using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideData : MonoBehaviour {

    /*IN PROGRESS - this is a script attached to each slide to record which index of each object type it holds, as well as its recording data*/

    private AudioClip slideClip;
    private int charaIndex;
    private int backdropIndex;
    private int itemIndex;
    private AudioSource slideAudio;

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
    }

    public void setBackdrop(int ind)
    {
        backdropIndex = ind;
    }

    public void setItem(int ind)
    {
        itemIndex = ind;
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
}
