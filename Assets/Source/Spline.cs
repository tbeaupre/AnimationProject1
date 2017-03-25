using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spline{

	public float time;
	public List<Vector3> positions;
	public List<Vector3> rotations;

	public Spline (int numCtrlPts, float time) {
		positions = new List<Vector3>(numCtrlPts);
		rotations = new List<Vector3>(numCtrlPts);
		this.time = time;
	}
}
