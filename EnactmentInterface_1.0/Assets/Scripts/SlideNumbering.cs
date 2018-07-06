using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;




public class SlideNumbering : MonoBehaviour
{

    /*This class updates the numbers for each slide according to their position in the heirarchy, working its way down from the Beginning slide array to the Ending slides*/
    /*For this reason, it's important to keep the slides in order in the heirarchy by their id in each array*/

    private bool isRecording = false;
    private bool donePlanning = false;
    private bool ready = false;
    private int condition = 2; //0-bottom up, 1-hybrid, 2-top down
    // Use this for initialization

    public Sprite recordStop;
    public Sprite recordStart;
    public Sprite blankSprite;
    //public Sprite playStop;

    private Sprite playSprite;

    private GameObject playButton;
    private Vector3 playPosition;
    

    private GameObject selectionTrio;
    private Vector3 trioPosition;

    public Sprite titleCard;
    public Sprite subtitleCard;
    public Sprite endCard;

    private int begSlideCount;
    private int midSlideCount;
    private int endSlideCount;
    private int slideTotalCount;

    private int selectedID;
    private int selectedSection;

    //Bottom Up
    //private bool emptySlide = false;


    //Top Down

    void Start()
    {
        
        playSprite = GameObject.FindGameObjectWithTag("play_slide_button").GetComponent<Image>().sprite;

        playButton = GameObject.FindGameObjectWithTag("play_screen");
        playPosition = playButton.transform.position;

        selectionTrio = GameObject.FindGameObjectWithTag("object_selection");
        trioPosition = selectionTrio.transform.position;

        //Set up Enactment Screen//
        GameObject[] charaPosButtons = GameObject.FindGameObjectsWithTag("chara_position");
        GameObject[] objPosButtons = GameObject.FindGameObjectsWithTag("obj_position");

        foreach (GameObject button in charaPosButtons)
        {
            button.GetComponent<Button>().interactable = false;
            button.GetComponent<Image>().color = new Color(1, 1, 1, 0);

        }

        foreach (GameObject button in objPosButtons)
        {
            button.GetComponent<Button>().interactable = false;
            button.GetComponent<Image>().color = new Color(1,1,1,0);
        }

        GameObject.FindGameObjectWithTag("enactment_check").GetComponent<Button>().interactable=false;
        GameObject.FindGameObjectWithTag("enactment_check").GetComponent<Image>().color = new Color(1,1,1,0);
        //end of setting up Enactment Screen//


        //Setting up Timeline for condition

        //function here to get condition

        switch (condition)
        {
            case 0:
                //Bottom up- record button is visible but not yet active, planning button goes away, play story button is visible but disabled
                Destroy(GameObject.Find("NextSlide"));
                Destroy(GameObject.Find("LastSlide"));
                GameObject.FindGameObjectWithTag("record_screen").GetComponent<Button>().interactable = false;
                GameObject.FindGameObjectWithTag("play_screen").GetComponent<Button>().interactable = false;
                GameObject.FindGameObjectWithTag("planning_button").GetComponent<Button>().interactable = false;
                GameObject.FindGameObjectWithTag("planning_button").GetComponent<Image>().color = new Color(1, 1, 1, 0);
                GameObject.FindGameObjectWithTag("planning_button").transform.SetAsFirstSibling();
                GameObject.FindGameObjectWithTag("instructions").GetComponent<Text>().text = "Create Your Story";

                break;
            case 1:
                break;
            case 2:
                //Top down - record button and play story button goes away, planning button is visible but disabled
                GameObject.FindGameObjectWithTag("planning_button").transform.SetAsLastSibling();
                GameObject.FindGameObjectWithTag("planning_button").GetComponent<Button>().interactable = false;
                GameObject.FindGameObjectWithTag("record_screen").GetComponent<Button>().interactable = false;
                GameObject.FindGameObjectWithTag("record_screen").GetComponent<Image>().color = new Color(1, 0, 0, 0);
                GameObject.FindGameObjectWithTag("play_screen").GetComponent<Button>().interactable = false;
                GameObject.FindGameObjectWithTag("play_screen").GetComponent<Image>().color = new Color(1, 1, 1, 0);
                GameObject.FindGameObjectWithTag("play_screen").transform.position = new Vector3(5000,5000);
                GameObject.FindGameObjectWithTag("instructions").GetComponent<Text>().text = "Plan Your Story";
                break;
            default:
                break;

        }
    }

    // Update is called once per frame
    void Update()
    {
        SlideArray[] arrays = GetComponentsInChildren<SlideArray>();

        begSlideCount = arrays[0].getCurrent();
        midSlideCount = arrays[1].getCurrent();
        endSlideCount = arrays[2].getCurrent();


        slideTotalCount = begSlideCount + midSlideCount + endSlideCount;

        

        if (getSelectedStatus()&&getSelectedData().isFilled())
        {
            getSelectedData().UpdatePlayRecordButton(begSlideCount, midSlideCount, endSlideCount, slideTotalCount, selectedSection, selectedID);
        }
        


        int num = 1;
        
        
        foreach (Transform child in this.transform) //Gets all slide arrays set as children to the GameObject with this script
        {
            foreach (Transform grandchild in child.transform)
            {
                if (grandchild.GetComponentInChildren<SlideSelectSlide>() != null) //Since the add slide buttons and slides are under the same parent, only objects with a Slide script is assigned a number to its Text component
                {
                    if (grandchild.GetComponentInChildren<Text>() != null)
                    {
                        grandchild.GetComponentInChildren<Text>().text = num.ToString();
                        num++;
                    }
                }
            }
        }


        GameObject[] addButtons = GameObject.FindGameObjectsWithTag("add_slide");
        SlideArray[] children = GetComponentsInChildren<SlideArray>();


        SlideData[] begSlides = children[0].GetComponentsInChildren<SlideData>();
        SlideData[] midSlides = children[1].GetComponentsInChildren<SlideData>();
        SlideData[] endSlides = children[2].GetComponentsInChildren<SlideData>();

        switch (condition)
            {

                case 0:
                    

                    if (!EmptySlide()) //no slides are empty
                    {
                        foreach (GameObject button in addButtons)
                        {
                            button.GetComponent<Button>().interactable = true;
                        }

                    if (begSlideCount>=5) {GameObject.Find("AddBeg").GetComponent<Button>().interactable = false; }
                    if (midSlideCount>=5) {GameObject.Find("AddMid").GetComponent<Button>().interactable = false; }
                    if (endSlideCount>=5) {GameObject.Find("AddEnd").GetComponent<Button>().interactable = false; }

                    SlideArray[] childrenSlides = GetComponentsInChildren<SlideArray>();

                    for (int i = 0; i < childrenSlides.Length; i++)
                    {
                        SlideSelectSlide[] grandchildren = childrenSlides[i].GetComponentsInChildren<SlideSelectSlide>();

                        for (int k = 0; k < grandchildren.Length; k++)
                        {
                            grandchildren[k].draggingOn();

                        }
                    }
                }
                    else
                    {
                        foreach (GameObject button in addButtons)
                        {
                            button.GetComponent<Button>().interactable = false;
                        }

                    SlideArray[] childrenSlides = GetComponentsInChildren<SlideArray>();

                    for (int i = 0; i < childrenSlides.Length; i++)
                    {
                        SlideSelectSlide[] grandchildren = childrenSlides[i].GetComponentsInChildren<SlideSelectSlide>();

                        for (int k = 0; k < grandchildren.Length; k++)
                        {
                            grandchildren[k].draggingOff();

                        }
                    }


                }

                    if (getSelectedStatus())
                    {
                        if (getSelectedData().isFilled())
                        {
                            GameObject.FindGameObjectWithTag("record_screen").GetComponent<Button>().interactable = true;
                        }
                    }
                    else { GameObject.FindGameObjectWithTag("record_screen").GetComponent<Button>().interactable = false; }

                    
                    if (begSlides.Length > 0 && midSlides.Length > 0 && endSlides.Length > 0 && !EmptySlide() && titlesFilled())
                    {
                        GameObject.FindGameObjectWithTag("play_screen").GetComponent<Button>().interactable = true;
                    }
                    else
                    {
                        GameObject.FindGameObjectWithTag("play_screen").GetComponent<Button>().interactable = false;
                    }

                    break;

                case 1:
                    

                    break;
                case 2:
                    if (donePlanning)
                    {
                    GameObject.FindGameObjectWithTag("instructions").GetComponent<Text>().text = "Act Out Your Story";
                    GameObject[] allTitles = GameObject.FindGameObjectsWithTag("title");

                    foreach (GameObject title in allTitles)
                        {
                        title.GetComponent<InputField>().interactable = false;
                        }


                    foreach (GameObject button in addButtons)
                            {
                                button.GetComponent<Button>().interactable = false;
                            }

                        SlideArray[] childrenSlides = GetComponentsInChildren<SlideArray>();

                        for (int i = 0; i < childrenSlides.Length; i++)
                        {
                            SlideSelectSlide[] grandchildren = childrenSlides[i].GetComponentsInChildren<SlideSelectSlide>();

                            for (int k = 0; k < grandchildren.Length; k++)
                            {
                                grandchildren[k].draggingOff();

                            }
                        }

                    //record button & play button come to life
                    //planning button goes away
                    GameObject.FindGameObjectWithTag("planning_button").transform.SetAsFirstSibling();
                    GameObject.FindGameObjectWithTag("planning_button").GetComponent<Button>().interactable = false;
                    GameObject.FindGameObjectWithTag("planning_button").GetComponent<Image>().color = new Color(1, 1, 1, 0);
                    GameObject.FindGameObjectWithTag("record_screen").GetComponent<Image>().color = new Color(1, 0, 0, 1);
                    GameObject.FindGameObjectWithTag("play_screen").GetComponent<Image>().color = new Color(.235f, .788f, .4f, 1);
                    GameObject.FindGameObjectWithTag("play_screen").transform.position = playPosition;
                    selectionTrio.transform.position = new Vector3(5000, 5000);


                    if (getSelectedStatus())
                    {
                        GameObject.FindGameObjectWithTag("record_screen").GetComponent<Button>().interactable = true;
                    }
                    else
                    {
                        GameObject.FindGameObjectWithTag("record_screen").GetComponent<Button>().interactable = false;
                    }

                    
                    if (begSlides.Length > 0 && midSlides.Length > 0 && endSlides.Length > 0 && !EmptySlide())
                    {
                        GameObject.FindGameObjectWithTag("play_screen").GetComponent<Button>().interactable = true;
                    }
                    else
                    {
                        GameObject.FindGameObjectWithTag("play_screen").GetComponent<Button>().interactable = false;
                    }
                }
                else
                {
                    if (begSlideCount >= 5) { GameObject.Find("AddBeg").GetComponent<Button>().interactable = false; }
                    else { GameObject.Find("AddBeg").GetComponent<Button>().interactable = true; }
                    if (midSlideCount >= 5) { GameObject.Find("AddMid").GetComponent<Button>().interactable = false; }
                    else { GameObject.Find("AddMid").GetComponent<Button>().interactable = true; }
                    if (endSlideCount >= 5) { GameObject.Find("AddEnd").GetComponent<Button>().interactable = false; }
                    else { GameObject.Find("AddEnd").GetComponent<Button>().interactable = true; }


                }

                if (donePlanButtonReady()) { GameObject.FindGameObjectWithTag("planning_button").GetComponent<Button>().interactable = true; }
                else { GameObject.FindGameObjectWithTag("planning_button").GetComponent<Button>().interactable = false; }
                

                break;

                default:
                    break;

            }


        
    }


    public bool titlesFilled()
    {
        GameObject[] allTitles = GameObject.FindGameObjectsWithTag("title");

        foreach(GameObject title in allTitles)
        {
            if (title.GetComponent<InputField>().text == "") { return false; }
        }

        return true;
    }

    public void donePlan()
    {   
        donePlanning = true;
        GameObject.FindGameObjectWithTag("trash").GetComponent<Button>().interactable = false;
    }

    public bool donePlanButtonReady()
    {

        SlideArray[] children = GetComponentsInChildren<SlideArray>();

        for (int i = 0; i < children.Length; i++)
        {
            SlideData[] grandchildrenData = children[i].GetComponentsInChildren<SlideData>();

            if (grandchildrenData.Length==0) { return false; }

            for (int k = 0; k < grandchildrenData.Length; k++)
            {
                if (grandchildrenData[k].isFilled()==false)
                {
                    return false;
                }

            }
        }

        if (titlesFilled() == false) { return false; }

        return true;

    }

    //deselects all slides across all three arrays so only one slide is selected
    //is called by SelectMe in SlideSelectSlide script, by the slide being selected
    public void selectNew(int section, int slide)
    {
        SlideArray[] children = GetComponentsInChildren<SlideArray>();

        for (int i = 0; i < children.Length; i++)
        {
            SlideSelectSlide[] grandchildren = children[i].GetComponentsInChildren<SlideSelectSlide>();
            SlideData[] grandchildrenData = children[i].GetComponentsInChildren<SlideData>();
            for (int k = 0; k < grandchildren.Length; k++)
            {
                if (i == section && grandchildren[k].getListID() == slide)
                {

                    //do nothing, the slide calling this function handles its own state
                }
                else { grandchildren[k].deselectMe(); }

            }
        }



        updateSelectedVars();

    }

    public void selectNewRemotely(int section, int slide)
    {
        SlideArray[] children = GetComponentsInChildren<SlideArray>();

        for (int i = 0; i < children.Length; i++)
        {
            SlideSelectSlide[] grandchildren = children[i].GetComponentsInChildren<SlideSelectSlide>();
            SlideData[] grandchildrenData = children[i].GetComponentsInChildren<SlideData>();
            for (int k = 0; k < grandchildren.Length; k++)
            {
                if (i == section && grandchildren[k].getListID() == slide)
                {
                    grandchildren[k].selectMe();
                    //do nothing, the slide calling this function handles its own state
                }
                

            }
        }

    }


    //goes through all slides when the delete button is pressed to find which one is selected for deletion
    public void deleteSlide()
    {
        SlideArray[] children = GetComponentsInChildren<SlideArray>();

        for (int i = 0; i < children.Length; i++)
        {
            SlideSelectSlide[] grandchildren = children[i].GetComponentsInChildren<SlideSelectSlide>();

            for (int k = 0; k < grandchildren.Length; k++)
            {
                if (grandchildren[k].getSelected() == true) { grandchildren[k].deleteMe(); }

            }
        }


    }


    public void recordSelectedSlide()
    {

        GameObject playButton = GameObject.FindGameObjectWithTag("play_slide_button");
        GameObject backButton = GameObject.FindGameObjectWithTag("back_button");
        GameObject recordButton = GameObject.FindGameObjectWithTag("record_button");
        GameObject[] poseButtons = GameObject.FindGameObjectsWithTag("pose_button");

        if (isRecording == false)
        {
            getSelectedData().startRecord();
            isRecording = true;
            playButton.GetComponent<Button>().interactable = false;
            backButton.GetComponent<Button>().interactable = false;
            recordButton.GetComponent<Image>().sprite = recordStop;
            foreach (GameObject button in poseButtons)
            {
                button.GetComponent<Button>().interactable = false;

            }
        }
        else
        {
            getSelectedData().endRecord();
            getSelectedSlide().showRecordedStatus();
            isRecording = false;
            playButton.GetComponent<Button>().interactable = true;
            backButton.GetComponent<Button>().interactable = true;
            recordButton.GetComponent<Image>().sprite = recordStart;
            foreach (GameObject button in poseButtons)
            {
                button.GetComponent<Button>().interactable = true;

            }
        }

    }

    public void playSelectedSlide()
    {
        getSelectedData().playAudio();

    }


    public void poseZero()
    {

        getSelectedData().setPose(0);
        getSelectedData().updatePoseMode();
    }

    public void poseOne()
    {
        getSelectedData().setPose(1);
        getSelectedData().updatePoseMode();
    }

    public void poseTwo()
    {
        getSelectedData().setPose(2);
        getSelectedData().updatePoseMode();
    }

    public void poseThree()
    {
        getSelectedData().setPose(3);
        getSelectedData().updatePoseMode();
    }

    public void poseFour()
    {
        getSelectedData().setPose(4);
        getSelectedData().updatePoseMode();
    }

   

    public void updateEnactmentPose()
    {
        getSelectedData().updateCharaPose(true);
       
    }

    public void updateEnactmentObjects()
    {
        getSelectedData().updateEnactmentScreen();
      
    }

    public void updateNewEnactmentObjects(int sect, int id)
    {

        SlideArray[] children = GetComponentsInChildren<SlideArray>();

        for (int i = 0; i < children.Length; i++)
        {
            SlideSelectSlide[] grandchildren = children[i].GetComponentsInChildren<SlideSelectSlide>();
            SlideData[] grandchildrenData = children[i].GetComponentsInChildren<SlideData>();
            for (int k = 0; k < grandchildren.Length; k++)
            {
                if (sect == i && grandchildren[k].getListID() == id)
                {

                    grandchildrenData[k].updateEnactmentScreen(); 

                }

            }
        }

       // getSelectedData().updateEnactmentScreen();

    }

    public void enactmentCheck()
    {
        getSelectedData().updatePoseMode();
    }

 /*   public void handheldObjects()
    {
        getSelectedData().setPose(getSelectedData().getPose(), false);
        
    }
    */

    public void charaLeft()
    {
        getSelectedData().setCharaPos(0);
        getSelectedData().updateCharaPos(true);
        //getSelectedData().updatePoseMode();
    }

    public void charaRight()
    {
        getSelectedData().setCharaPos(1);
        getSelectedData().updateCharaPos(true);
        //getSelectedData().updatePoseMode();
    }

    public void charaUp()
    {
        getSelectedData().setCharaPos(2);
        getSelectedData().updateCharaPos(true);
        //getSelectedData().updatePoseMode();
    }

    public void charaDown()
    {
        getSelectedData().setCharaPos(3);
        getSelectedData().updateCharaPos(true);
        //getSelectedData().updatePoseMode();
    }

    public void objectLeft()
    {
        getSelectedData().setObjectPos(0);
        getSelectedData().updateItemPos(true);
        //getSelectedData().updatePoseMode();
    }

    public void objectRight()
    {
        getSelectedData().setObjectPos(1);
        getSelectedData().updateItemPos(true);
        //getSelectedData().updatePoseMode();
    }

    public void objectUp()
    {
        getSelectedData().setObjectPos(2);
        getSelectedData().updateItemPos(true);
        //getSelectedData().updatePoseMode();
    }

    public void objectDown()
    {
        getSelectedData().setObjectPos(3);
        getSelectedData().updateItemPos(true);
        //getSelectedData().updatePoseMode();
    }

    private SlideData getSelectedData()
    {
        SlideArray[] children = GetComponentsInChildren<SlideArray>();

        for (int i = 0; i < children.Length; i++)
        {
            SlideSelectSlide[] grandchildren = children[i].GetComponentsInChildren<SlideSelectSlide>();
            SlideData[] grandchildrenData = children[i].GetComponentsInChildren<SlideData>();
            for (int k = 0; k < grandchildren.Length; k++)
            {
                if (grandchildren[k].getSelected() == true)
                {
                    return grandchildrenData[k];
                    
                }
                
            }
        }
        return new SlideData();
    }

    private SlideSelectSlide getSelectedSlide()
    {
        SlideArray[] children = GetComponentsInChildren<SlideArray>();

        for (int i = 0; i < children.Length; i++)
        {
            SlideSelectSlide[] grandchildren = children[i].GetComponentsInChildren<SlideSelectSlide>();
            //SlideData[] grandchildrenData = children[i].GetComponentsInChildren<SlideData>();
            for (int k = 0; k < grandchildren.Length; k++)
            {
                if (grandchildren[k].getSelected() == true)
                {

                    return grandchildren[k];

                }

            }
        }
        return new SlideSelectSlide();
    }


    private void updateSelectedVars()
    {
        SlideArray[] children = GetComponentsInChildren<SlideArray>();

        for (int i = 0; i < children.Length; i++)
        {
            SlideSelectSlide[] grandchildren = children[i].GetComponentsInChildren<SlideSelectSlide>();
            //SlideData[] grandchildrenData = children[i].GetComponentsInChildren<SlideData>();
            for (int k = 0; k < grandchildren.Length; k++)
            {
                if (grandchildren[k].getSelected() == true)
                {
                    selectedSection = i;
                    selectedID = grandchildren[k].getListID();

                }

            }
        }

    }


    private bool getSelectedStatus()
    {
        SlideArray[] children = GetComponentsInChildren<SlideArray>();

        for (int i = 0; i < children.Length; i++)
        {
            SlideSelectSlide[] grandchildren = children[i].GetComponentsInChildren<SlideSelectSlide>();

            for (int k = 0; k < grandchildren.Length; k++)
            {
                if (grandchildren[k].getSelected() == true)
                {

                    return true;

                }

            }
        }
        return false;
    }


    public void setTitle()
    {
        GameObject[] currents;

        currents = GameObject.FindGameObjectsWithTag("current_item");

        foreach (GameObject current in currents)
        {
            Destroy(current);
        }

        GameObject.Find("EnactmentBackdrop").GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);

        GameObject.Find("EnactmentBackdrop").GetComponent<SpriteRenderer>().sprite = titleCard;
        GameObject.Find("PlayCharacter").GetComponent<Image>().color = new Color(1, 1, 1, 0);
        GameObject.Find("PlayTitle").GetComponent<Text>().text = GameObject.FindGameObjectWithTag("main_title").GetComponent<Text>().text;
        GameObject.Find("PlayTitle").GetComponent<Text>().color = new Color(0, 0, 0, 1);

    }


    public IEnumerator SequenceTiming()
      {
          SlideArray[] sections = GetComponentsInChildren<SlideArray>();

          SlideData[] beginning = sections[0].GetComponentsInChildren<SlideData>();
          SlideData[] middle = sections[1].GetComponentsInChildren<SlideData>();
          SlideData[] end = sections[2].GetComponentsInChildren<SlideData>();

        float wait = 3.0f;

        int whichClip = 1;
        int iterator = 1;

        GameObject.Find("PlayWindow").GetComponent<Image>().color = new Color(1, 1, 1, 0);

        //GameObject.Find("PlayInstruction").GetComponent<Text>().color = new Color(0, 0, 0, 0);
        GameObject.FindGameObjectWithTag("play_story_button").GetComponent<Button>().interactable = false;
        GameObject.FindGameObjectWithTag("playscreen_back_button").GetComponent<Button>().interactable = false;
        GameObject.FindGameObjectWithTag("play_story_button").GetComponent<Image>().color = new Color(1,1,1,0);
        GameObject.Find("PlayText").GetComponent<Text>().color = new Color(1, 1, 1, 0);
        GameObject.FindGameObjectWithTag("playscreen_back_button").GetComponent<Image>().color = new Color(1, 1, 1, 0);

        GameObject.Find("SaveStory").GetComponent<Image>().color = new Color(1, 1, 1, 0);
        GameObject.Find("SaveText").GetComponent<Text>().color = new Color(1, 1, 1, 0);
        GameObject.Find("SaveStory").GetComponent<Button>().interactable = false;

        //Start Recording
        //GameObject.Find("Recorder").GetComponent<Animator>().StartRecording(24*60*5);
        GameObject.Find("EnactmentBackdrop").GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        GameObject.Find("EnactmentBackdrop").GetComponent<SpriteRenderer>().sprite = titleCard;

        yield return new WaitForSeconds(wait); //pause for title

        GameObject.Find("EnactmentBackdrop").GetComponent<SpriteRenderer>().sprite = subtitleCard;
        GameObject.Find("PlayTitle").GetComponent<Text>().text = GameObject.FindGameObjectWithTag("beg_title").GetComponent<Text>().text;

        yield return new WaitForSeconds(wait);

        //GameObject.Find("EnactmentBackdrop").GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        GameObject.Find("PlayTitle").GetComponent<Text>().color = new Color(0, 0, 0, 0);
        GameObject.Find("PlayCharacter").GetComponent<Image>().color = new Color(1, 1, 1, 1);

        for (int i = 0; i < beginning.Length; i++)
          {
            wait = beginning[i].getTime();
            
            beginning[i].updatePlayScreen();
            beginning[i].playAudio();
            yield return new WaitForSeconds(wait);

            while (whichClip == 1)
            {
                if (System.IO.File.Exists("myfile"))
                {
                    GetComponent<SavWav>().Save("myfile" + 1, beginning[i].getAudio() );
                    whichClip = 2;
                }
                else
                {
                    GetComponent<SavWav>().Save("myfile", beginning[i].getAudio());
                    whichClip = 2;
                }
            }


        }

      

        GameObject[] currents;

        currents = GameObject.FindGameObjectsWithTag("current_item");

        foreach (GameObject current in currents)
        {
            Destroy(current);
        }

        GameObject.Find("EnactmentBackdrop").GetComponent<SpriteRenderer>().sprite = subtitleCard;
        //GameObject.Find("EnactmentBackdrop").GetComponent<SpriteRenderer>().color = new Color(.596f, .824f, .894f, 1);

        GameObject.Find("PlayCharacter").GetComponent<Image>().color = new Color(1, 1, 1, 0);
        GameObject.Find("PlayTitle").GetComponent<Text>().text = GameObject.FindGameObjectWithTag("mid_title").GetComponent<Text>().text;
        GameObject.Find("PlayTitle").GetComponent<Text>().color = new Color(0, 0, 0, 1);
        yield return new WaitForSeconds(3.0f);

        GameObject.Find("EnactmentBackdrop").GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        GameObject.Find("PlayTitle").GetComponent<Text>().color = new Color(0, 0, 0, 0);
        GameObject.Find("PlayCharacter").GetComponent<Image>().color = new Color(1, 1, 1, 1);

        for (int i = 0; i < middle.Length; i++)
          {
            wait = middle[i].getTime();
            
            middle[i].updatePlayScreen();
            middle[i].playAudio();
            yield return new WaitForSeconds(wait);
        }


        currents = GameObject.FindGameObjectsWithTag("current_item");

        foreach (GameObject current in currents)
        {
            Destroy(current);
        }

        GameObject.Find("EnactmentBackdrop").GetComponent<SpriteRenderer>().sprite = subtitleCard;
        //GameObject.Find("EnactmentBackdrop").GetComponent<SpriteRenderer>().color = new Color(.596f, .824f, .894f, 1);

        GameObject.Find("PlayCharacter").GetComponent<Image>().color = new Color(1, 1, 1, 0);
        GameObject.Find("PlayTitle").GetComponent<Text>().text = GameObject.FindGameObjectWithTag("end_title").GetComponent<Text>().text;
        GameObject.Find("PlayTitle").GetComponent<Text>().color = new Color(0, 0, 0, 1);

        yield return new WaitForSeconds(3.0f);

        GameObject.Find("EnactmentBackdrop").GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        GameObject.Find("PlayTitle").GetComponent<Text>().color = new Color(0, 0, 0, 0);
        GameObject.Find("PlayCharacter").GetComponent<Image>().color = new Color(1, 1, 1, 1);

        for (int i = 0; i < end.Length; i++)
          {
            wait = end[i].getTime();
            
            end[i].updatePlayScreen();
            end[i].playAudio();
            yield return new WaitForSeconds(wait);
        }



        currents = GameObject.FindGameObjectsWithTag("current_item");

        foreach (GameObject current in currents)
        {
            Destroy(current);
        }

        GameObject.Find("EnactmentBackdrop").GetComponent<SpriteRenderer>().sprite = endCard;
        //GameObject.Find("EnactmentBackdrop").GetComponent<SpriteRenderer>().color = new Color(.596f, .824f, .894f, 1);

        GameObject.Find("PlayCharacter").GetComponent<Image>().color = new Color(1, 1, 1, 0);
        GameObject.Find("PlayTitle").GetComponent<Text>().text = "The End";
        GameObject.Find("PlayTitle").GetComponent<Text>().color = new Color(0, 0, 0, 1);

        yield return new WaitForSeconds(3.0f);


        //Stop Recording
        //GameObject.Find("Recorder").GetComponent<Animator>().StopRecording();


        GameObject.Find("PlayWindow").GetComponent<Image>().color = new Color(1, 1, 1, .7098f);
        setTitle();

        //GameObject.Find("PlayInstruction").GetComponent<Text>().color = new Color(0, 0, 0, 1);
        GameObject.FindGameObjectWithTag("play_story_button").GetComponent<Button>().interactable = true;
        GameObject.FindGameObjectWithTag("playscreen_back_button").GetComponent<Button>().interactable = true;
        GameObject.FindGameObjectWithTag("play_story_button").GetComponent<Image>().color = new Color(.235f, .788f, .4f, 1);
        GameObject.Find("PlayText").GetComponent<Text>().color = new Color(1, 1, 1, 1);
        GameObject.FindGameObjectWithTag("playscreen_back_button").GetComponent<Image>().color = new Color(1, 1, 1, 1);

        GameObject.Find("SaveStory").GetComponent<Image>().color = new Color(.235f, .788f, .4f, 1);
        GameObject.Find("SaveText").GetComponent<Text>().color = new Color(1, 1, 1, 1);
        GameObject.Find("SaveStory").GetComponent<Button>().interactable = true;
    }


    /* public IEnumerator TestingCoRoutine()
     {
         yield return new WaitForSeconds(3.0f);
         Debug.Log("3 have passed");
         yield return new WaitForSeconds(4.0f);
         Debug.Log("Done");

     }*/

    public void PlayThrough()
    {
        StopAllCoroutines();
        StartCoroutine(SequenceTiming());
    }

    private bool EmptySlide()
    {
        SlideArray[] children = GetComponentsInChildren<SlideArray>();

        for (int i = 0; i < children.Length; i++)
        {
            SlideData[] grandchildrenData = children[i].GetComponentsInChildren<SlideData>();

            for (int k = 0; k < grandchildrenData.Length; k++)
            {
                if (grandchildrenData[k].isSlideEmpty() == true)
                {
                    return true;
                }

            }
        }

        return false;
        
    }


    public void nextSlide()
    {
        int newSection;
        int newID;
        int sectionLength;

        updateSelectedVars();
        Debug.Log(selectedSection);
        Debug.Log(selectedID);
        switch (selectedSection)
        {
            case 0:
                sectionLength = begSlideCount;
                break;
            case 1:
                sectionLength = midSlideCount;
                break;
            case 2:
                sectionLength = endSlideCount;
                break;
            default:
                sectionLength = 100;
                break;
        }


        if (selectedID + 1 == sectionLength)
        {
            newSection = selectedSection+1;
            newID = 0;
            Debug.Log("Next");
        }
        else
        {
            newSection = selectedSection;
            newID = selectedID+1;
            Debug.Log("NextElse");
        }

        Debug.Log(newSection);
        Debug.Log(newID);
        selectNewRemotely(newSection, newID);
        updateNewEnactmentObjects(newSection, newID);
    }



    public void lastSlide()
    {
        int newSection;
        int newID;
        int sectionLength;

        updateSelectedVars();

        Debug.Log(selectedSection);
        Debug.Log(selectedID);


        if (selectedID == 0)
        {
            newSection = selectedSection-1;
            switch (newSection)
            {
                case 0:
                    sectionLength = begSlideCount;
                    break;
                case 1:
                    sectionLength = midSlideCount;
                    break;
                case 2:
                    sectionLength = endSlideCount;
                    break;
                default:
                    sectionLength = 100;
                    break;
            }
            newID = sectionLength-1;
            Debug.Log("Last");
            
        }
        else
        {
            newSection = selectedSection;
            newID = selectedID-1;
            Debug.Log("LastElse");
        }

        Debug.Log(newSection);
        Debug.Log(newID);
        selectNewRemotely(newSection, newID);
        updateNewEnactmentObjects(newSection, newID);
    }




}
