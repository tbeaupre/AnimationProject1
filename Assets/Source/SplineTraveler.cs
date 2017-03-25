using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineTraveler : MonoBehaviour {

	public string filePath;
	public List<Spline> splines;
	List<string> data;
	int dataIter;

	// Use this for initialization
	void Start () {
		splines = ReadFile();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// Read data from a new file
	List<Spline> ReadFile () {
		Debug.Log(filePath);

		// Check to see if file exists
		if (!System.IO.File.Exists(@filePath))
		{
			Debug.Log("File Does Not Exist!");
			return new List<Spline>();
		}

		string[] lines = System.IO.File.ReadAllLines(@filePath);
		data = new List<string>();
		dataIter = 0;

		// Remove comments (the lines that start with # or empty lines).
		foreach(string s in lines)
		{
			if (!s.StartsWith("#") && s != "")
			{
				data.Add (s);
			}
		}
		Debug.Log(string.Format("file contains {0} lines of data.", data.Count));
	
		// Record number of splines
		int numSplines = int.Parse(GetNextString());

		List<Spline> splines = new List<Spline>(numSplines);

		// Repeat these operations for each spline in the file.
		for (int i = 0; i < numSplines; i++)
		{
			// Determine the number of control points for this spline
			int numCtrlPts = int.Parse(GetNextString());

			Spline spline = new Spline(numCtrlPts);
			spline.time = float.Parse(GetNextString());

			// Repeat these operations for each control point in the spline
			for (int j = 0; j < numCtrlPts; j++)
			{
				string[] vals = GetNextString().Split(',');
				spline.positions.Add(new Vector3(float.Parse(vals[0]), float.Parse(vals[1]), float.Parse(vals[2])));

				vals = GetNextString().Split(',');
				spline.rotations.Add(new Vector3(float.Parse(vals[0]), float.Parse(vals[1]), float.Parse(vals[2])));
			}

			splines.Add (spline);
		}

		Debug.Log("Successfully read file!");
		return splines;
	}

	string GetNextString() {
		string s = data[dataIter];
		Debug.Log(string.Format("Retrieving line {0} of {1} : " + s, dataIter, data.Count));
		dataIter++;
		return s;
	}
}
