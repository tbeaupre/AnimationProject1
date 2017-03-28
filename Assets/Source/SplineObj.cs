using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The SplineObj class is a script for a unity GameObject which draws the control points on the screen
public class SplineObj : MonoBehaviour {
	public GameObject controlPoint;
	Spline spline;
	public float time;

	// Use this for initialization
	void Start () {
		
	}

	public void Initialize (Spline spline, float time)
	{
		this.time = time;
		this.spline = spline;
		spline.CalcSplineDefinition(); // One time calculation for the tangents of the spline.
		DrawControlPoints();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public Vector3 CalcPosAtTime(float t)
	{
		return spline.CalcPosAtTime(t);
	}

	public Vector3 CalcRotAtTime(float t)
	{
		float u = t * (spline.numCtrlPts - 1); // u is now a value between 0 and the last control point.
		int i = Mathf.FloorToInt(u); // i now represents the subsection of the spline to use.
		u = u - i; // u now represents the u of the subsection of the spline.

		if (t > 1 || t < 0) // Check to make sure the time is valid.
		{
			return new Vector3(0, 0, 0);
		}
		if (t == 1) // Check corner cases for 0 and 1.
		{
			return spline.rots[spline.numCtrlPts - 1];
		}
		if (t == 0)
		{
			return spline.rots[0];
		} else
		{
			MyQuaternion q1 = new MyQuaternion(spline.rots[i]);
			MyQuaternion q2 = new MyQuaternion(spline.rots[i = 1]);

			MyQuaternion result = MyQuaternion.Slerp(q1, q2, u);
			return result.ConvertToEuler();
		}
	}
		
	void DrawControlPoints()
	{
		for (int i = 0; i < spline.poss.Count; i++)
		{
			Quaternion rot = new Quaternion();
			rot.SetFromToRotation(new Vector3(0,0,0), spline.rots[i]);
			GameObject controlPointClone = Object.Instantiate(controlPoint, spline.poss[i], rot);
			Debug.Log(string.Format("Now displaying control point {0} at {1} with Euler rotations {2}.", i, spline.poss[i], spline.rots[i]));
		}
		Debug.Log("Successfully drew all control points!");
	}
}
