using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
	public Transform endpoint;
    public Transform player;

	Light torch;

	float newIntensity;

	public float torchIntensityMin = 1;
	public float torchIntensityMax = 10;

	public float torchRangeMin = 30;
	public float torchRangeMax = 70;


	float angleToExit;

	float deltaTime = 0.0f;
 
	void Start() {
		torch = GameObject.Find("PlayerTorch").GetComponent<Light>();
	}
	
	void Update()
	{
		// update deltatime
		deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

		// calculate player's angle to the exit
		angleToExit = Vector3.Angle(player.transform.forward, -endpoint.transform.forward);

		// get ratio of angle in angle range to get normal
		float normal = Mathf.InverseLerp(0, 180, angleToExit);

		// invert normal because you want to go from low angles to high torch values
		float invertNormal = 1 - normal;

		// calculate new intensity
		newIntensity = Mathf.Lerp(torchIntensityMin, torchIntensityMax, invertNormal);
		
		// apply new intensity to torch
		torch.intensity = newIntensity;	

		// calculate new range
		float newRange = Mathf.Lerp(torchRangeMin, torchRangeMax, invertNormal);
		torch.range = newRange;
	

	}


 
	void OnGUI()
	{
		int w = Screen.width, h = Screen.height;
 
		GUIStyle style = new GUIStyle();
 
		// fps display
		Rect rect = new Rect(0, 0, w, h * 2 / 75);
		style.alignment = TextAnchor.UpperLeft;
		style.fontSize = h * 2 / 100;
		style.normal.textColor = new Color (1.0f, 0.92f, 0.016f, 1.0f);
		float msec = deltaTime * 1000.0f;
		float fps = 1.0f / deltaTime;
		string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
		GUI.Label(rect, text, style);

		// player direction display
		rect = new Rect(0, 15, w, h * 2 / 75);
		style.alignment = TextAnchor.UpperLeft;
		style.fontSize = h * 2 / 100;
		style.normal.textColor = new Color (1.0f, 0.92f, 0.016f, 1.0f);
		text = string.Format("Angle to end: {0} degrees", angleToExit);
		GUI.Label(rect, text, style);

		// torch intensity display
		rect = new Rect(0, 30, w, h * 2 / 75);
		style.alignment = TextAnchor.UpperLeft;
		style.fontSize = h * 2 / 100;
		style.normal.textColor = new Color (1.0f, 0.92f, 0.016f, 1.0f);
		text = string.Format("Torch intensity: {0}", newIntensity);
		GUI.Label(rect, text, style);


	}
}