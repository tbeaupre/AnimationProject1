﻿using System.Collections;
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
