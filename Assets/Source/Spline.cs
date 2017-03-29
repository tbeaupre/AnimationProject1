using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The spline class contains all of the math functions related to splines.
public class Spline{
	public List<Vector3> poss;
	public List<Vector3> rots;
	public List<Vector3> tans;
	public List<MyQuaternion> quats;
	public int numCtrlPts;
	public List<Vector3> bSplinePoss;

	public Spline (int numCtrlPts) {
		this.numCtrlPts = numCtrlPts;
		poss = new List<Vector3>(numCtrlPts);
		rots = new List<Vector3>(numCtrlPts);
	}

	public void Init(float time, bool pre)
	{
		if (pre)
		{
			PreCalculateBSplinePos(time);
		}
		CalcCRSplineTans();
		CalcQuaternions();
	}

	public void PreCalculateBSplinePos(float time)
	{
		if (SplineTraveler.bSplinePoss.Count != 0)
		{
			this.bSplinePoss = SplineTraveler.bSplinePoss;
		} else
		{
			bSplinePoss = new List<Vector3>();
			for (int i = 0; i <= (20 * time); i++)
			{
				// i = [0, 20 * time] with one step for each frame
				bSplinePoss.Add(ApproxPosAtTime(i / (20 * time), 2));
			}
		}
		SplineTraveler.bSplinePoss = bSplinePoss;
		Debug.Log("Done Calculating BSpline Positions!");
	}

	public void CalcQuaternions()
	{
		quats = new List<MyQuaternion>(numCtrlPts);
		foreach (Vector3 rot in rots)
		{
			quats.Add(new MyQuaternion(rot));
		}
	}

	public void CalcCRSplineTans()
	{
		if (numCtrlPts <= 2)
		{
			// Just a straight line
		} else
		{
			// Do the normal calculations
			tans = new List<Vector3>();
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

	public List<int> CalcKnotVector(int k)
	{
		List<int> result = new List<int>(numCtrlPts + k);
		for (int i = 0; i < numCtrlPts + k; i++)
		{
			result.Add(i);
		}
		Debug.Log(string.Format("Knot Vector: {0} to {1}", result[0], result[result.Count - 1]));
		return result;
	}

	public Vector3 ApproxPosAtTime(float t, int k)
	{
		if (t > 1 || t < 0) // Check to make sure the time is valid.
		{
			return new Vector3(0, 0, 0);
		}
		// Reparameterize the t value to [0, numCtrlPts]
		float x = t * (numCtrlPts - 1);
		int l = Mathf.FloorToInt(x); // i now represents the subsection of the spline to use.
		List<int> knotVector = CalcKnotVector(k);

		Vector3 result = new Vector3();
		for (int i = Mathf.Max(0, l - k); i < (numCtrlPts - 1); i++)
		{
			result += poss[i] * Basis(ref knotVector, i, k, x);
		}
		return result;
	}

	//public float DeBoor(ref List<int> u, )

	public float Basis(ref List<int> t, int i, int k, float x)
	{
		//Debug.Log(string.Format("Basis(i={0}, k={1}, x={2}))\nMax Knot Vector Index: {3}\nMin Knot Vector Index: {4}", i, k, x, (i + k + 1), i));
		if (k <= 0)
		{
			if (x >= t[i] && x < t[i + 1])
			{
				return 1.0f;
			} else
			{
				return 0.0f;
			}
		} else
		{
			return (((x - t[i]) / (t[i + k] - t[i])) * Basis(ref t, i, k - 1, x)) + (((t[i + k + 1] - x) / (t[i + k + 1] - t[i + 1])) * Basis(ref t, i + 1, k - 1, x));
		}
	}

	public Vector3 CalcRotAtTime(float t)
	{
		float u = t * (numCtrlPts - 1); // u is now a value between 0 and the last control point.
		int i = Mathf.FloorToInt(u); // i now represents the subsection of the spline to use.
		u = u - i; // u now represents the u of the subsection of the spline.

		if (t > 1 || t < 0) // Check to make sure the time is valid.
		{
			return new Vector3(0, 0, 0);
		}
		if (t == 1) // Check corner cases for 0 and 1.
		{
			return rots[numCtrlPts - 1];
		}
		if (t == 0)
		{
			return rots[0];
		} else
		{
			if (rots[i] == rots[i + 1])
			{
				Debug.Log(string.Format("Rotation at time {0}(i = {1}, u = {2}) : {3}", t, i, u, rots[i]));
				return rots[i];
			}
			MyQuaternion q = MyQuaternion.Slerp(quats[i], quats[i + 1], u);
			Vector3 result = MyQuaternion.ConvertToEuler(q);
			Debug.Log(string.Format("Rotation at time {0}(i = {1}, u = {2}) : {3}", t, i, u, result));
			return result;
		}
	}
}
