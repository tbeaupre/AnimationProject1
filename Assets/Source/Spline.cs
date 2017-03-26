using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spline{

	public float time;
	public List<Vector3> poss;
	public List<Vector3> rots;
	public List<Vector3> tans;

	public Spline (int numCtrlPts, float time) {
		poss = new List<Vector3>(numCtrlPts);
		rots = new List<Vector3>(numCtrlPts);
		this.time = time;
	}

	public void CalcTans()
	{
		if (poss.Count <= 2)
		{
			// Just a straight line
		} else
		{
			// Do the normal calculations
			CalcInitTan();
			for (int i = 1; i < poss.Count - 1; i++)
			{
				tans.Add((poss[i + 1] - poss[i - 1]) / 2);
			}
			CalcFinalTan(poss.Count - 1);
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
}
