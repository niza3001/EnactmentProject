using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class notes : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

/*
            ready = true;
            int num = 1;
            GameObject.FindGameObjectWithTag("record_screen").GetComponent<Button>().interactable = false;

            if (donePlanning == false) { GameObject.FindGameObjectWithTag("planning_button").GetComponent<Button>().interactable = true; }

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

                        if (grandchild.GetComponentInChildren<SlideData>().isFilled() != true || (arrays[0].getCurrent() > 0 && arrays[1].getCurrent() > 0 && arrays[2].getCurrent() > 0 == false))
                        {
                            if (ready == false) { GameObject.FindGameObjectWithTag("planning_button").GetComponent<Button>().interactable = false; }
                            ready = false;
                        }
                    }
                }
            }
        }*/
