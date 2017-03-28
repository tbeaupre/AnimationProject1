using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyQuaternion
{
	public float s;
	public Vector3 v;

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
		float t0 = Mathf.Cos(Mathf.Deg2Rad * euler[1]) * 0.5f;
		float t1 = Mathf.Sin(Mathf.Deg2Rad * euler[1]) * 0.5f;
		float t2 = Mathf.Cos(Mathf.Deg2Rad * euler[2]) * 0.5f;
		float t3 = Mathf.Sin(Mathf.Deg2Rad * euler[2]) * 0.5f;
		float t4 = Mathf.Cos(Mathf.Deg2Rad * euler[0]) * 0.5f;
		float t5 = Mathf.Sin(Mathf.Deg2Rad * euler[0]) * 0.5f;

		this.s = t0 * t2 * t4 + t1 * t3 * t5;
		this.v = new Vector3(
			t0 * t3 * t4 - t1 * t2 * t5,
			t0 * t2 * t5 + t1 * t3 * t4,
			t1 * t2 * t4 - t0 * t3 * t5);
	}

	public Vector3 ConvertToEuler ()
	{
		MyQuaternion q = Normalize(this);
		Vector3 result = new Vector3();

		float y2 = q.v[1] * q.v[1];

		float t2 = 2.0f * (q.s * q.v[1] - q.v[2] * q.v[0]);
		t2 = Mathf.Min(t2, 1);
		t2 = Mathf.Max(t2, -1);
		result[0] = Mathf.Rad2Deg * Mathf.Asin(t2);

		float t3 = 2.0f * (q.s * q.v[2] + q.v[0] * q.v[1]);
		float t4 = 1.0f - 2.0f * (y2 + q.v[2] * q.v[2]);
		result[1] = Mathf.Rad2Deg * Mathf.Atan2(t3, t4);

		float t0 = 2.0f * (q.s * q.v[0] + q.v[1] * q.v[2]);
		float t1 = 1.0f - 2.0f * (q.v[0] * q.v[0] + y2);
		result[2] = Mathf.Rad2Deg * Mathf.Atan2(t0, t1);

		return result;
	}

	static public MyQuaternion operator * (MyQuaternion q, float scalar)
	{
		return new MyQuaternion(q.s * scalar, q.v * scalar);
	}
		
	static public MyQuaternion operator + (MyQuaternion q1, MyQuaternion q2)
	{
		return new MyQuaternion(q1.s + q2.s, q1.v + q2.v);
	}

	static public MyQuaternion operator - (MyQuaternion q1, MyQuaternion q2)
	{
		return new MyQuaternion(q1.s - q2.s, q1.v - q2.v);
	}

	static public float Dot (MyQuaternion q1, MyQuaternion q2)
	{
		return ((q1.s * q2.s) + Vector3.Dot(q1.v, q2.v));
	}

	static public MyQuaternion Slerp (MyQuaternion q1, MyQuaternion q2, float u)
	{
		MyQuaternion uq1 = Normalize(q1);
		MyQuaternion uq2 = Normalize(q2);

		float dot = Dot(uq1, uq2);

		if (dot < 0)
		{
			uq2 *= -1;
		}

		float theta = Mathf.Acos(Dot(uq1, uq2));

		if (Mathf.Sin(theta) == 0)
		{
			return uq1 + ((uq2 - uq1) * u);
		}

		MyQuaternion result = (uq1 * (Mathf.Sin((1 - u) * theta) / Mathf.Sin(theta))) + (uq2 * (Mathf.Sin(u * theta) / Mathf.Sin(theta)));
		result = Normalize(result);
		return result;
	}

	static public float Length (MyQuaternion q)
	{
		return Mathf.Sqrt((q.s * q.s) + (q.v[0] * q.v[0]) + (q.v[1] * q.v[1]) + (q.v[2] * q.v[2]));
	}

	static public MyQuaternion Normalize (MyQuaternion q)
	{
		return q * (1 / Length(q));
	}

	static public MyQuaternion Inverse (MyQuaternion q)
	{
		MyQuaternion result = q;
		result.v *= -1;
		result = Normalize(result);
		return result;
	}
}

