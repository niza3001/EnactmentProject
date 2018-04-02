﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlideNumbering : MonoBehaviour
{

    /*This class updates the numbers for each slide according to their position in the heirarchy, working its way down from the Beginning slide array to the Ending slides*/
    /*For this reason, it's important to keep the slides in order in the heirarchy by their id in each array*/

    private bool isRecording = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        int num = 1;
        GameObject.FindGameObjectWithTag("record_screen").GetComponent<Button>().interactable = false;
        GameObject.FindGameObjectWithTag("play_screen").GetComponent<Button>().interactable = true;

        SlideArray[] arrays = GetComponentsInChildren<SlideArray>();
 

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

                    if (grandchild.GetComponentInChildren<SlideSelectSlide>().getSelected() && grandchild.GetComponentInChildren<SlideData>().isFilled())
                    {
                        GameObject.FindGameObjectWithTag("record_screen").GetComponent<Button>().interactable = true;
                    }

                    if (grandchild.GetComponentInChildren<SlideData>().isFilled() != true ||(arrays[0].getCurrent() > 0 && arrays[1].getCurrent() > 0 && arrays[2].getCurrent() > 0==false))
                    {
                        GameObject.FindGameObjectWithTag("play_screen").GetComponent<Button>().interactable = false;
                    }
                }
            }
        }

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
        

        SlideArray[] children = GetComponentsInChildren<SlideArray>();

        for (int i = 0; i < children.Length; i++)
        {
            SlideSelectSlide[] grandchildren = children[i].GetComponentsInChildren<SlideSelectSlide>();
            SlideData[] grandchildrenData = children[i].GetComponentsInChildren<SlideData>();
            for (int k = 0; k < grandchildren.Length; k++)
            {
                if (grandchildren[k].getSelected() == true) {


                    if (isRecording == false)
                    {
                        grandchildrenData[k].startRecord();
                        isRecording = true;
                        playButton.GetComponent<Button>().interactable = false;
                    }
                    else
                    {
                        grandchildrenData[k].endRecord();
                        isRecording = false;
                        playButton.GetComponent<Button>().interactable = true;
                    }


                }

            }
        }

    }

    public void playSelectedSlide()
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

                    
                        grandchildrenData[k].playAudio();
                    


                }

            }
        }
    }


    public void poseZero()
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
                    
                    grandchildrenData[k].setPose(0, grandchildrenData[k].getGround());
                    
                }

            }
        }
    }

    public void poseOne()
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

                    grandchildrenData[k].setPose(1, grandchildrenData[k].getGround());

                }

            }
        }
    }

    public void poseTwo()
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

                    grandchildrenData[k].setPose(2, grandchildrenData[k].getGround());

                }

            }
        }
    }

    public void poseThree()
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

                    grandchildrenData[k].setPose(3, grandchildrenData[k].getGround());

                }

            }
        }
    }

    public void poseFour()
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

                    grandchildrenData[k].setPose(4, grandchildrenData[k].getGround());

                }

            }
        }
    }

    public void groundPose()
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

                    grandchildrenData[k].setPose(grandchildrenData[k].getPose(), true);

                }

            }
        }


    }

    public void updateEnactmentPose()
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

                    grandchildrenData[k].updateCharaPose();

                }

            }
        }
    }

    public void updateEnactmentObjects()
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

                    grandchildrenData[k].updateEnactmentScreen();

                }

            }
        }
    }

    public void handheldObjects()
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

                    grandchildrenData[k].setPose(grandchildrenData[k].getPose(), false);

                }

            }
        }
    }

}
