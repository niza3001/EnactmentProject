
using UnityEngine;
using System.Collections;

public enum physicalObject
{
   
    Lantern = 0,
    Racket = 1, 
    SmallBox, 
    Sphere, 
    Stick, 
    Shield, 

    Head , 
    Torso , 
    LeftHand, 
    RightHand, 
    LeftLeg, 
    RightLeg,
    

}
public class OptiTrackObject : MonoBehaviour {

	public bool disableTranslationUpdate; 
	public bool disableRotationUpdate;
	private  int rigidbodyIndex;
    public physicalObject phyObj; 
	public Vector3 rotationOffset;
	public  float localScale = 0.2f;   
	private Vector3 scaleVector;    

	private GameObject managerObject; 
	private bool _optiTrackOn; 
	// Use this for initialization
	void Start () {
        // OO* 1/29/2018
        //Debug.LogError("Start() func was called from OptiTrackObject.cs .");
        this.transform.localPosition = new Vector3(0f, 0f, 0f);
		//rigidbodyIndex--;
        rigidbodyIndex = (int)phyObj;
        //TODO FIX rigidBodyIndex once Optitrack is back 
        if (_optiTrackOn == false)
            rigidbodyIndex++;
        else if (_optiTrackOn == true)    
            rigidbodyIndex--;

		scaleVector = new Vector3 (localScale, localScale, localScale);
		managerObject = GameObject.FindGameObjectWithTag ("Manager");
        if (managerObject.GetComponent<TransformManager>().currentTransform == TransformType.Optitrack)
            _optiTrackOn = true; 

	}
	
	// Update is called once per frame
	void Update () 
	{
        //Debug.LogError(_optiTrackOn);
        if (_optiTrackOn)
		{
            
			Vector3 pos = TransformManager.Instance.getPosition(rigidbodyIndex-1);
			Quaternion rot = TransformManager.Instance.getOrientation(rigidbodyIndex-1);
			rot = rot * Quaternion.Euler(rotationOffset);
			if(!disableTranslationUpdate)
			{
				this.transform.position = pos*localScale;
			}
			if(!disableRotationUpdate)
			{
				this.transform.rotation = rot;
			}
		}
		else
		{

			Vector3 pos = TransformManager.Instance.getPositionExcel(rigidbodyIndex);
			Quaternion rot = TransformManager.Instance.getOrientationExcel(rigidbodyIndex);
            
			rot = rot * Quaternion.Euler(rotationOffset);
			if(!disableTranslationUpdate)
			{
				this.transform.localPosition = pos*localScale;

                if (rigidbodyIndex == 1)
                {
                    Debug.Log("Lantern is" + pos);
                }
			}
			if(!disableRotationUpdate)
			{
				this.transform.rotation = rot;
			}
		}
	}
    void OnGUI()
    {
       
    }
}
