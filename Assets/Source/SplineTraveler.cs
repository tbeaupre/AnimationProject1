using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineTraveler : MonoBehaviour {

	public GameObject splineObjPrefab;
	public string filePath; // The filepath to the spline data.
	string currentFilePath; // Tracker so that the program recalculates when a new file is selected.
	public List<GameObject> splines; // The splines described in the text file.
	List<string> strData; // The strings of data in the text file.
	int strDataIter; // An iterator for cycling through the lines of the text file.

	int curSplineIndex;
	SplineObj curSpline;

	Timer timer;
	bool done = false;

	// Use this for initialization
	void Start () {
		currentFilePath = filePath;
		splines = ReadFile();
		if (splines.Count > 0)
		{
			curSplineIndex = 0;
			curSpline = splines[curSplineIndex].GetComponent<SplineObj>();
			transform.position = curSpline.CalcPosAtTime(0);
			timer = new Timer(curSpline.time);
			done = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (filePath != currentFilePath)
		{
			ReadFile();
		}
		if (!done)
		{
			float timerVal = timer.Update(); // Find the time from 0 to 1 representing the traveler's position on the spline.
			if (timerVal != -1) // If a position change is necessary.
			{
				if (timerVal == -2) // If the animation has ended.
				{
					transform.position = curSpline.CalcPosAtTime(1);
					// Move onto the next spline in the list.
					curSplineIndex++;
					if (curSplineIndex < splines.Count)
					{
						curSpline = splines[curSplineIndex].GetComponent<SplineObj>();
						transform.position = curSpline.CalcPosAtTime(0);
						transform.eulerAngles = curSpline.CalcRotAtTime(0).ConvertToEuler();
						timer = new Timer(curSpline.time);
					} else
					{
						done = true;
						Debug.Log("Done!");
					}
				} else
				{
					transform.position = curSpline.CalcPosAtTime(timerVal);
					transform.eulerAngles = curSpline.CalcRotAtTime(timerVal).ConvertToEuler();
					//Debug.Log(string.Format("Now Moving to t={0}, at location: {1} with rotation: {2}.", timerVal, transform.position, transform.eulerAngles));
				}
			}
		}
	}

	// Read data from a new file
	List<GameObject> ReadFile () {
		Debug.Log(string.Format("Reading data from {0}", filePath));

		// Check to see if file exists
		if (!System.IO.File.Exists(@filePath))
		{
			Debug.Log("File Does Not Exist!");
			return new List<GameObject>();
		}

		// Read the file and remove commented lines
		string[] lines = System.IO.File.ReadAllLines(@filePath);
		strData = new List<string>();
		strDataIter = 0;
		// Remove comments (the lines that start with # or empty lines).
		foreach(string s in lines)
		{
			if (!s.StartsWith("#") && s != "")
			{
				strData.Add (s);
			}
		}
		//Debug.Log(string.Format("file contains {0} lines of data.", strData.Count));
	
		// Record number of splines
		int numSplines = int.Parse(GetNextString());
		// Initialize list of splines based on number retrieved
		List<GameObject> splines = new List<GameObject>(numSplines);

		// Repeat these operations for each spline in the file.
		for (int i = 0; i < numSplines; i++)
		{
			// Determine the number of control points for this spline and initialize it
			int numCtrlPts = int.Parse(GetNextString());
			float time = float.Parse(GetNextString());
			Spline spline = new Spline(numCtrlPts);

			// Repeat these operations for each control point in the spline
			for (int j = 0; j < numCtrlPts; j++)
			{
				// Split up the X, Y, and Z values of the control point
				string[] vals = GetNextString().Split(',');
				spline.poss.Add(new Vector3(float.Parse(vals[0]), float.Parse(vals[1]), float.Parse(vals[2])));

				// Split up the XRot, YRot, and ZRot values of the control point
				vals = GetNextString().Split(',');
				spline.rots.Add(new Vector3(float.Parse(vals[0]), float.Parse(vals[1]), float.Parse(vals[2])));
			}
			GameObject splineObj = (GameObject) Instantiate(splineObjPrefab);
			splineObj.GetComponent<SplineObj>().Initialize(spline, time);
			splines.Add (splineObj);
		}
		Debug.Log("Successfully read file!");
		return splines;
	}

	// Retrieves the next string in the list of data strings
	string GetNextString() {
		string s = strData[strDataIter];
		//Debug.Log(string.Format("Retrieving line {0} of {1} : " + s, strDataIter + 1, strData.Count));
		strDataIter++;
		return s;
	}
}
