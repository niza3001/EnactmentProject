using UnityEngine;
using System.Collections;

public class PlayQueueDemoUI : MonoBehaviour
{
    public GUISkin _skin;
    public PlayQueueDemo _demo;
    
    public bool isPlayBackScene;
    private bool _visible = true;
    private float _alpha = 1.0f;
    private bool alertBox = false; 
    void Update()
    {
        Rect r = new Rect(0, 0, Screen.width / 2, Screen.height);
        /*
        if (r.Contains(new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y)))
        {
            _visible = true;
            _alpha = 1.0f;
        }
        else
        {
            _alpha -= Time.deltaTime * 4f;
            if (_alpha <= 0.0f)
            {
                _alpha = 0.0f;
                _visible = false;
            }
        }*/
        _visible = true;
        _alpha = 1.0f;
    }

    public void ControlWindow(int id)
    {
        if (_demo == null)
            return;

        GUILayout.Space(16f);

        GUILayout.BeginVertical("box");


        AVProWindowsMediaMovie movie = _demo.PlayingMovie;
        if (movie != null)
        {
            AVProWindowsMedia moviePlayer = movie.MovieInstance;
            if (moviePlayer != null)
            {
                /*GUILayout.BeginHorizontal();
                GUILayout.Label("Playing", GUILayout.Width(80));
                GUILayout.Label(movie._filename + " (" + moviePlayer.Width + "x" + moviePlayer.Height + " @ " + moviePlayer.FrameRate.ToString("F2") + ")");
                GUILayout.EndHorizontal();
                */
                GUILayout.BeginHorizontal();
                GUILayout.Label("Time ", GUILayout.Width(80));
                GUILayout.HorizontalSlider(moviePlayer.PositionSeconds, 0.0f, moviePlayer.DurationSeconds, GUILayout.ExpandWidth(true));
                GUILayout.Label(moviePlayer.PositionSeconds.ToString("F1") + "/" + moviePlayer.DurationSeconds.ToString("F1") + "s", GUILayout.ExpandWidth(false));
                GUILayout.EndHorizontal();
            }
        }

        GUILayout.BeginHorizontal();
        //GUILayout.Space(80);
        GUILayout.Label(string.Empty, GUILayout.Width(80));
        if (GUILayout.Button("Prev"))
        {
            _demo.Previous();
        }

        if (_demo.IsPaused)
        {
            if (GUILayout.Button("Play"))
            {
                _demo.Unpause();
            }
        }
        else
        {
            if (GUILayout.Button("Pause"))
            {
                _demo.Pause();
            }
        }

        if (GUILayout.Button("Next"))
        {
            _demo.Next();
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(32f);

        /*
        GUILayout.BeginHorizontal();
        GUILayout.Label("Folder: ", GUILayout.Width(80));
        _demo._folder = GUILayout.TextField(_demo._folder, 192);
        GUILayout.EndHorizontal();
        GUILayout.Space(32f);
            */

        int removeIndex = -1;
        int cloneIndex = -1;
        int upIndex = -1;
        int downIndex = -1;
        //GUILayout.Label("Playlist");
        /*
        for (int movieIndex = 0; movieIndex < _demo._filenames.Count; movieIndex++)
        {
            Color prevBackCol = GUI.backgroundColor;
            if (movieIndex == _demo.PlayingItemIndex)
                GUI.backgroundColor = Color.green;
			
            GUILayout.BeginHorizontal();
            GUILayout.Label((movieIndex+1).ToString(), GUILayout.Width(80));
            _demo._filenames[movieIndex] = GUILayout.TextField(_demo._filenames[movieIndex], 192, GUILayout.MinWidth(256f));
            if (GUILayout.Button("X", GUILayout.Width(24)))
            {
                removeIndex = movieIndex;
            }
            if (GUILayout.Button("Clone", GUILayout.Width(64)))
            {
                cloneIndex = movieIndex;
            }
            if (GUILayout.Button("Up", GUILayout.Width(32)))
            {
                upIndex = movieIndex;
            }
            if (GUILayout.Button("Down", GUILayout.Width(48)))
            {
                downIndex = movieIndex;
            }
            GUILayout.EndHorizontal();
			
            if (movieIndex == _demo.PlayingItemIndex)
                GUI.backgroundColor = prevBackCol;			
        }*/

        if (removeIndex >= 0)
        {
            _demo._filenames.RemoveAt(removeIndex);
            removeIndex = -1;
        }
        if (cloneIndex >= 0)
        {
            _demo._filenames.Insert(cloneIndex + 1, _demo._filenames[cloneIndex]);
        }

        if (upIndex > 0)
        {
            int indexA = upIndex - 1;
            int indexB = upIndex;
            string old = _demo._filenames[indexA];
            _demo._filenames[indexA] = _demo._filenames[indexB];
            _demo._filenames[indexB] = old;
        }
        if (downIndex > 0 && downIndex + 1 < _demo._filenames.Count)
        {
            int indexA = downIndex + 1;
            int indexB = downIndex;
            string old = _demo._filenames[indexA];
            _demo._filenames[indexA] = _demo._filenames[indexB];
            _demo._filenames[indexB] = old;
        }
        /*
        if (GUILayout.Button("+"))
        {
            _demo._filenames.Add(string.Empty);
        }*/



        GUILayout.EndVertical();
    }
    public GUIStyle text; 
    void OnGUI()
    {
        if (isPlayBackScene == false)
        {
            //GUI.skin = _skin;
            GUI.color = new Color(1f, 1f, 1f, 1f - _alpha);
            //GUI.Box(new Rect(Screen.width*0.4f, Screen.height*0.9, 128, 32), "Demo Controls");
            if (_visible)
            {
                GUI.color = new Color(1f, 1f, 1f, _alpha);
                //GUILayout.Box("Review Story");
                //GUILayout.BeginArea(new Rect(0, 0, 440, 200), GUI.skin.box);
                ControlWindow(0);
            }
            /*
            if (GUI.Button(new Rect(Screen.width * 0.05f, Screen.height * 0.88f, Screen.width * 0.1f, Screen.height * 0.1f), "back"))
            {
                Application.LoadLevel(5);
            }
            */
            if (GUI.Button(new Rect(Screen.width * 0.85f, Screen.height * 0.88f, Screen.width * 0.1f, Screen.height * 0.1f), "Publish"))
            {
                alertBox = true;
            }
            if (alertBox)
            {
                GUI.Label(new Rect(Screen.width * 0.25f, Screen.height * 0.75f, Screen.width * 0.5f, Screen.height * 0.2f), "Are you sure you want to publish? (If you click OK, you will not be able to edit your story anymore and the final video will be shown to you)",text);
                if (GUI.Button(new Rect(Screen.width * 0.25f, Screen.height * 0.88f, Screen.width * 0.25f, Screen.height * 0.1f), "OK"))
                {
                    Application.LoadLevel(12);
                }
                if (GUI.Button(new Rect(Screen.width * 0.5f, Screen.height * 0.88f, Screen.width * 0.25f, Screen.height * 0.1f), "Cancel"))
                {
                    alertBox = false;
                }

            }
        }

    }
}
