using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineTraveler : MonoBehaviour {

	public string filePath; // The filepath to the spline data
	public List<Spline> splines; // The splines described in the text file
	List<string> strData; // The strings of data in the text file
	int strDataIter; // An iterator for cycling through the lines of the text file

	// Use this for initialization
	void Start () {
		splines = ReadFile();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// Read data from a new file
	List<Spline> ReadFile () {
		Debug.Log(string.Format("Reading data from {0}", filePath));

		// Check to see if file exists
		if (!System.IO.File.Exists(@filePath))
		{
			Debug.Log("File Does Not Exist!");
			return new List<Spline>();
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
		Debug.Log(string.Format("file contains {0} lines of data.", strData.Count));
	
		// Record number of splines
		int numSplines = int.Parse(GetNextString());
		// Initialize list of splines based on number retrieved
		List<Spline> splines = new List<Spline>(numSplines);

		// Repeat these operations for each spline in the file.
		for (int i = 0; i < numSplines; i++)
		{
			// Determine the number of control points for this spline and initialize it
			int numCtrlPts = int.Parse(GetNextString());
			Spline spline = new Spline(numCtrlPts, float.Parse(GetNextString()));

			// Repeat these operations for each control point in the spline
			for (int j = 0; j < numCtrlPts; j++)
			{
				// Split up the X, Y, and Z values of the control point
				string[] vals = GetNextString().Split(',');
				spline.positions.Add(new Vector3(float.Parse(vals[0]), float.Parse(vals[1]), float.Parse(vals[2])));

				// Split up the XRot, YRot, and ZRot values of the control point
				vals = GetNextString().Split(',');
				spline.rotations.Add(new Vector3(float.Parse(vals[0]), float.Parse(vals[1]), float.Parse(vals[2])));
			}
			splines.Add (spline);
		}
		Debug.Log("Successfully read file!");
		return splines;
	}

	// Retrieves the next string in the list of data strings
	string GetNextString() {
		string s = strData[strDataIter];
		Debug.Log(string.Format("Retrieving line {0} of {1} : " + s, strDataIter, strData.Count));
		strDataIter++;
		return s;
	}
}
