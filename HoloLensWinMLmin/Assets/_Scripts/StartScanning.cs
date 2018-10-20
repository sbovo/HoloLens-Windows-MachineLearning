using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScanning : MonoBehaviour {

	// Unity 3D Text object that contains 
	// the displayed TextMesh in the FOV
	public GameObject OutputText;
	// TextMesh object provided by the OutputText game object
	private TextMesh OutputTextMesh;
	// string to be affected to the TextMesh object
	private string OutputTextString = string.Empty;
	// Indicate if we have to Update the text displayed
	bool OutputTextChanged = false;


	// Use this for initialization
	void Start ()
	{
		OutputTextMesh = OutputText.GetComponent<TextMesh>();

#if UNITY_WSA && !UNITY_EDITOR // RUNNING ON WINDOWS
		//InitializeModel();
		//CreateMediaCapture();
#else                          // RUNNING IN UNITY
		ModifyOutputText("Sorry ;-( The app is not supported in the Unity player.");
#endif
	}

	/// <summary>
	/// Modify the text to be displayed in the FOV
	/// or/and in the debug traces
	/// + Indicate that we have to update the text to display
	/// </summary>
	/// <param name="newText">new string value to display</param>
	private void ModifyOutputText(string newText)
	{
		OutputTextString = newText;
		OutputTextChanged = true;
	}

	// Update is called once per frame
	void Update () {
		if (OutputTextChanged)
		{
			OutputTextMesh.text = OutputTextString;
			OutputTextChanged = false;
		}
	}
}
