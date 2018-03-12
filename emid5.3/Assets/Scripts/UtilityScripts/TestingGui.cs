

using UnityEngine;
using System.Collections;

public class TestingGui : MonoBehaviour {
    IEnumerator Start() 
    {
        StartCoroutine("DoSomething", 2.0F);
        yield return new WaitForSeconds(1);
        StopCoroutine("DoSomething");

        print("OUTSIDE loop");
    }
    IEnumerator fetchTexture()
    {
        string url = "http://C://Users//Kumar//Documents//Dime//Assets//screen1.png";

        WWW www = new WWW(url);
        yield return www;
     }
    IEnumerator DoSomething(float someParameter) 
    {
        while (true) {
            print("DoSomething Loop");
            yield return null;
        }
    }
}
