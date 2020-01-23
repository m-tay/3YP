using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
	public Transform endpoint;
    public Transform player;
	public int compassImprecision = 10;

	float angle;


	float deltaTime = 0.0f;
	int playerRotImprecise;
	float playerAngle;
 
	void Update()
	{
		// update deltatime
		deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

		angle = Vector3.Angle(player.transform.forward, -endpoint.transform.forward);
		
		

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
		text = string.Format("Angle to end: {0} degrees", playerAngle);
		GUI.Label(rect, text, style);


	}
}