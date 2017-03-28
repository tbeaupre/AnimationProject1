using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyQuaternion
{
	float s;
	Vector3 v;

	public MyQuaternion (float w, float x, float y, float z)
	{
		this.s = w;
		this.v = new Vector3(x, y, z);
	}

	public MyQuaternion (float s, Vector3 v)
	{
		this.s = s;
		this.v = v;
	}

	public MyQuaternion (Vector3 euler)
	{
		float t0 = Mathf.Cos(euler[2] * 0.5f);
		float t1 = Mathf.Sin(euler[2] * 0.5f);
		float t2 = Mathf.Cos(euler[1] * 0.5f);
		float t3 = Mathf.Sin(euler[1] * 0.5f);
		float t4 = Mathf.Cos(euler[0] * 0.5f);
		float t5 = Mathf.Sin(euler[0] * 0.5f);

		this.s = t0 * t2 * t4 + t1 * t3 * t5;
		this.v = new Vector3(
			t0 * t3 * t4 - t1 * t2 * t5,
			t0 * t2 * t5 + t1 * t3 * t4,
			t1 * t2 * t4 - t0 * t3 * t5);
	}

	public Vector3 ConvertToEuler ()
	{
		Vector3 result = new Vector3();

		float y2 = v[1] * v[1];

		float t0 = 2.0f * (s * v[0] + v[1] * v[2]);
		float t1 = 1.0f - 2.0f * (v[0] * v[0] + y2);
		result[0] = Mathf.Atan2(t0, t1);

		float t2 = 2.0f * (s * v[1] - v[2] * v[0]);
		t2 = Mathf.Min(t2, 1);
		t2 = Mathf.Max(t2, -1);
		result[1] = Mathf.Asin(t2);

		float t3 = 2.0f * (s * v[2] + v[0] * v[1]);
		float t4 = 1.0f - 2.0f * (y2 + v[2] * v[2]);
		result[2] = Mathf.Atan2(t3, t4);

		return result;
	}

	static public MyQuaternion operator * (MyQuaternion q, float scalar)
	{
		MyQuaternion result = q;
		result.s *= scalar;
		result.v *= scalar;
		return result;
	}

	static public MyQuaternion operator + (MyQuaternion q1, MyQuaternion q2)
	{
		return new MyQuaternion(q1.s + q2.s, q1.v + q2.v);
	}

	static public float Dot (MyQuaternion q1, MyQuaternion q2)
	{
		return (q1.s * q2.s + Vector3.Dot(q1.v, q2.v));
	}

	static public MyQuaternion Slerp (MyQuaternion q1, MyQuaternion q2, float u)
	{
		MyQuaternion uq1 = q1 * (1 / Length(q1));
		MyQuaternion uq2 = q2 * (1 / Length(q2));

		float theta = Mathf.Acos(Dot(uq1, uq2));

		return (uq1 * (Mathf.Sin((1 - u) * theta) / Mathf.Sin(theta))) + (uq2 * (Mathf.Sin(u * theta) / Mathf.Sin(theta)));
	}

	static public float Length (MyQuaternion q)
	{
		return Mathf.Sqrt(q.s * q.s + q.v[0] * q.v[0] + q.v[1] * q.v[1] + q.v[2] * q.v[2]);
	}
}

