using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The spline class contains all of the math functions related to splines.
public class Spline{
	public List<Vector3> poss;
	public List<Vector3> rots;
	public List<Vector3> tans;
	int numCtrlPts;

	public Spline (int numCtrlPts) {
		this.numCtrlPts = numCtrlPts;
		poss = new List<Vector3>(numCtrlPts);
		rots = new List<Vector3>(numCtrlPts);
	}

	public void CalcSplineDefinition()
	{
		CalcTans();
		Debug.Log("Successfully Calculated Spline Definition");
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
	public Vector3 CalcPosAtTime(float t)
	{
		if (t > 1 || t < 0) // Check to make sure the time is valid.
		{
			return new Vector3(0, 0, 0);
		}
		if (t == 1) // Check corner cases for 0 and 1.
		{
			return poss[numCtrlPts - 1];
		}
		if (t == 0)
		{
			return poss[0];
		}

		float u = t * (numCtrlPts - 1); // u is now a value between 0 and the last control point.
		int i = Mathf.FloorToInt(u); // i now represents the subsection of the spline to use.
		u = u - i; // u now represents the u of the subsection of the spline.
		float u2 = u * u;
		float u3 = u * u * u;
		Vector3 p0 = poss[i];
		Vector3 p1 = poss[i + 1];
		Vector3 v0 = tans[i];
		Vector3 v1 = tans[i + 1];

		// While it may not look as clean as the matrix version, this formula is what made the most sense to me.
		return ((((2 * u3) - (3 * u2) + 1) * p0) +
		((u3 - (2 * u2) + u) * v0) +
		(((-2 * u3) + (3 * u2)) * p1) +
		((u3 - u2) * v1));
	}
}
