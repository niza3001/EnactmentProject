using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text.RegularExpressions;

public class PlayBackQueueDemo : MonoBehaviour
{
    public AVProWindowsMediaMovie _movieA;
    public AVProWindowsMediaMovie _movieB;
    public string _folder;
    public List<string> _filenames;
    public GUIStyle textOnFrame;

    private AVProWindowsMediaMovie[] _movies;
    private int _moviePlayIndex; // Current movie that is being played! 
    private int _movieLoadIndex; // Next movie to be played 
    private int _index = -1; // Current Index? But -1 at Start. 
    private bool _loadSuccess = true;
    private int _playItemIndex = -1;

    public AVProWindowsMediaMovie PlayingMovie { get { return _movies[_moviePlayIndex]; } }
    public AVProWindowsMediaMovie LoadingMovie { get { return _movies[_movieLoadIndex]; } }
    public int PlayingItemIndex { get { return _playItemIndex; } }
    public bool IsPaused { get { if (PlayingMovie.MovieInstance != null) return !PlayingMovie.MovieInstance.IsPlaying; return false; } }

    private bool movieStarted = false; 
    //PlayerPrefs Data 
    //private String[] tempArray = new String[10];
    private String s1, s2;


    private List<string> BIGList;
    private GameObject myGameManager;
    private int noOfTabs;

    void Start()
    {
        //GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AVProMovieCaptureFromCamera>().StartCapture();
        //GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AVProMovieCaptureFromCamera>()
        textOnFrame.normal.textColor = Color.white; 
        //PlayerPrefs Data
        //s1 = PlayerPrefs.GetString("storyTeller1"); 
        //s2 = PlayerPrefs.GetString("storyTeller2"); 
        //tempArray = PlayerPrefsX.GetStringArray("beginTabArray");
        myGameManager = GameObject.FindGameObjectWithTag("GameManager") as GameObject;
        noOfTabs = (int)myGameManager.GetComponent<GameManager>().dd.numberOfTabs;
        BIGList = myGameManager.GetComponent<GameManager>().dd.BIGList;

        _movieA._loop = false;
        _movieB._loop = false;
        _movies = new AVProWindowsMediaMovie[2];
        _movies[0] = _movieA;
        _movies[1] = _movieB;
        _moviePlayIndex = 0;
        _movieLoadIndex = 1;
        setupVideoPaths();

        NextMovie();
    }
    private void setupVideoPaths()
    {

        //String VideosFolder = Application.dataPath+"/Resources/Videos/"+s1+s2+"/"; 
        //Debug.Log (VideosFolder);
        /*if(Directory.Exists(VideosFolder))
        {*/

        for (int i = 0; i < BIGList.Count; i++)
        {
            if (BIGList[i].ToString() != String.Empty)
            {
                if (File.Exists(BIGList[i]))
                {
                    _filenames.Insert(i, BIGList[i]);
                }
                else
                {
                    _filenames.Insert(i, Directory.GetParent(Application.dataPath) + "/blank.avi");
                }
            }
        }


        /*}
        else 
            Debug.Log("Folder doesn't EXIST!!!!!!!!!!!");

        //_filenames = new List<string> (BIGList);
        Debug.Log (_filenames.Count.ToString ());*/
    }
    bool isTextFrame()
    {

        if (BIGList[_moviePlayIndex] == "VideoFrame")
            return false;
        else
            return true;
    }
    /*
    bool isTextFrame(String videoPath)
    {
        Match match = Regex.Match(videoPath, @"*",
                                  RegexOptions.IgnoreCase);
        if(match.Success)
        {
            return false;
        }
        else
        {
            return true;
        }
    }*/
    void Update()
    {
        if(_index == 1)
        {
            movieStarted = true; 

        }
        if(movieStarted)
        {
            if(_index == 0)
            {
                GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AVProMovieCaptureFromCamera>().StopCapture();
                Destroy(myGameManager);
                //Application.LoadLevel(0);
            }
        }
        Debug.Log("index is " + _index.ToString() + "moviePlayIndex is " + _moviePlayIndex.ToString());
        if (PlayingMovie.MovieInstance != null)
        {
            if ((int)PlayingMovie.MovieInstance.PositionFrames >= (PlayingMovie.MovieInstance.DurationFrames - 1))
            {
                NextMovie();
            }
        }

        if (!_loadSuccess)
        {
            _loadSuccess = true;
            NextMovie();
            
        }
    }

    void OnGUI()
    {
        Texture texture = PlayingMovie.OutputTexture;
        if (texture == null)
            texture = LoadingMovie.OutputTexture;		// Display the previous video until the current one has loaded the first frame

        if (texture != null)
        {
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), texture, ScaleMode.StretchToFill, false);
        }

        if (!File.Exists(BIGList[_index]))
        {
            //It's a blank Video, so use the Label 
            GUI.Label(new Rect(Screen.width * 0.25f, Screen.height * 0.25f, Screen.width * 0.5f, Screen.height * 0.5f), BIGList[_index], textOnFrame);
        }
    }

    public void Next()
    {
        NextMovie();
    }

    public void Previous()
    {
        _index -= 2;
        if (_index < 0)
            _index += _filenames.Count;

        NextMovie();
    }

    public void Pause()
    {
        if (PlayingMovie != null)
        {
            PlayingMovie.Pause();
        }
    }

    public void Unpause()
    {
        if (PlayingMovie != null)
        {
            PlayingMovie.Play();
        }
    }

    private void NextMovie()
    {
        Pause();

        if (_filenames.Count > 0)
        {
            _index = (Mathf.Max(0, _index + 1)) % _filenames.Count;
        }
        else
            _index = -1;

        if (_index < 0)
            return;


        LoadingMovie._folder = _folder;
        LoadingMovie._filename = _filenames[_index];
        LoadingMovie._playOnStart = true;
        _loadSuccess = LoadingMovie.LoadMovie(true);
        _playItemIndex = _index;

        _moviePlayIndex = (_moviePlayIndex + 1) % 2;
        _movieLoadIndex = (_movieLoadIndex + 1) % 2;
    }
}
