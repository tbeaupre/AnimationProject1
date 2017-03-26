using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineObj : MonoBehaviour {
	public GameObject controlPoint;
	Spline spline;

	// Use this for initialization
	void Start () {
		
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
