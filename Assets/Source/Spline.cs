using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The spline class contains all of the math functions related to splines.
public class Spline{

	public float time;
	public List<Vector3> poss;
	public List<Vector3> rots;
	public List<Vector3> tans;
	int numCtrlPts;

	public Spline (int numCtrlPts, float time) {
		this.numCtrlPts = numCtrlPts;
		poss = new List<Vector3>(numCtrlPts);
		rots = new List<Vector3>(numCtrlPts);
		this.time = time;
	}

	public void CalcTans()
	{
		if (numCtrlPts <= 2)
		{
			// Just a straight line
		} else
		{
			// Do the normal calculations
			CalcInitTan();
			for (int i = 1; i < numCtrlPts - 1; i++)
			{
				tans.Add((poss[i + 1] - poss[i - 1]) / 2);
			}
			CalcFinalTan(numCtrlPts - 1);
		}
	}

	void CalcInitTan()
	{
		tans = new List<Vector3>();
		tans.Add((poss[1] - (poss[2] - poss[1]) - poss[0]) / 2);
	}
		
	void CalcFinalTan(int i)
	{
		tans.Add(-(poss[i - 1] - (poss[i - 2] - poss[i - 1]) - poss[i]) / 2);
	}

	// Calculates the position at the time t along the spline. 0 <= t <= 1
	void CalcPosAtTime(float t)
	{
		float u = t * (numCtrlPts - 1);
		int i = Mathf.FloorToInt(u);
	}
}
