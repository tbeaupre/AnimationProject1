using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The SplineObj class is a script for a unity GameObject which draws the control points on the screen
public class SplineObj : MonoBehaviour {
	public GameObject controlPoint;
	Spline spline;

	// Use this for initialization
	void Start () {
		
	}

	public void Initialize (Spline spline)
	{
		this.spline = spline;
		DrawControlPoints();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
		
	void DrawControlPoints()
	{
		for (int i = 0; i < spline.poss.Count; i++)
		{
			GameObject controlPointClone = Object.Instantiate(controlPoint, spline.poss[i], new Quaternion());
		}
	}
}
