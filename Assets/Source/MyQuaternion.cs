using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyQuaternion
{
	float s;
	Vector3 v;

	public float GetS()
	{
		return this.s;
	}

	public Vector3 GetV()
	{
		return this.v;
	}

	public void SetS(float s)
	{
		this.s = s;
	}

	public void SetV(Vector3 v)
	{
		this.v = v;
	}

	public MyQuaternion (float w, float x, float y, float z)
	{
		SetS(w);
		SetV(new Vector3(x, y, z));
	}

	public MyQuaternion (float s, Vector3 v)
	{
		SetS(s);
		SetV(v);
	}

	public MyQuaternion (Vector3 euler)
	{
		float xRad = Mathf.Deg2Rad * euler.x;
		float yRad = Mathf.Deg2Rad * euler.y;
		float zRad = Mathf.Deg2Rad * euler.z;

		float c1 = Mathf.Cos(yRad / 2);
		float c2 = Mathf.Cos(zRad / 2);
		float c3 = Mathf.Cos(xRad / 2);
		float s1 = Mathf.Sin(yRad / 2);
		float s2 = Mathf.Sin(zRad / 2);
		float s3 = Mathf.Sin(xRad / 2);

		SetS(c1 * c2 * c3 - s1 * s2 * s3);
		SetV(new Vector3(
			s1 * s2 * c3 + c1 * c2 * s3,
			s1 * c2 * c3 + c1 * s2 * s3,
			c1 * s2 * c3 - s1 * c2 * s3));
	}

	static public Vector3 ConvertToEuler (MyQuaternion q0)
	{
		MyQuaternion q = Normalize(q0);

		float test = q.v.x * q.v.y + q.v.z * q.s;

		// Account for singularity at north pole
		if (test > 0.499) {
			return new Vector3(
				0,
				Mathf.Rad2Deg * 2 * Mathf.Atan2(q.v.x, q.s),
				90);
		}
		// Account for singularity at south pole
		if (test < -0.499) {
			return new Vector3(
				0,
				Mathf.Rad2Deg * -2 * Mathf.Atan2(q.v.x, q.s),
				-90);
		}
		// Standard calculation
		float x2 = q.v.x*q.v.x;
		float y2 = q.v.y*q.v.y;
		float z2 = q.v.z*q.v.z;

		return new Vector3(
			Mathf.Rad2Deg * Mathf.Atan2(2 * q.v.x * q.s - 2 * q.v.y * q.v.z, 1 - 2 * x2 - 2 * z2),
			Mathf.Rad2Deg * Mathf.Atan2(2 * q.v.y * q.s - 2 * q.v.x * q.v.z, 1 - 2 * y2 - 2 * z2),
			Mathf.Rad2Deg * Mathf.Asin(2 * test));
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
		return Mathf.Sqrt((q.s * q.s) + (q.v.x * q.v.x) + (q.v.y * q.v.y) + (q.v.z * q.v.z));
	}

	static public MyQuaternion Normalize (MyQuaternion q)
	{
		return q * (1 / Length(q));
	}

	static public MyQuaternion Inverse (MyQuaternion q)
	{
		MyQuaternion result = q;
		result.SetV(result.v * -1);
		return  result * ((1/Length(q)) * (1/Length(q)));
	}
}

