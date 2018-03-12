using UnityEngine;
using System.Collections;

public class FrameRateClass : MonoBehaviour {
	public float updateInterval = 0.5F;
	private double lastInterval;
	private int frames = 0;
	private double fps;
	void Start() {
		lastInterval = Time.realtimeSinceStartup;
		frames = 0;
	}
	void OnGUI() {
		GUILayout.Label("" + fps.ToString("f2"));
	}
	void Update() {
		++frames;
		float timeNow = Time.realtimeSinceStartup;
		if (timeNow > lastInterval + updateInterval) {
			fps = frames / (timeNow - lastInterval);
			frames = 0;
			lastInterval = timeNow;
		}
	}
}