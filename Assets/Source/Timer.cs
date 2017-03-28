﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
	const float interval = 1/30; // The time in seconds between redraws. 1/30th of a second for this animation
	float time; // The total time that the motion takes.
	float startTime; // The time when this timer was created.
	float nextTime; // The next time that an update must be drawn.
	float endTime; // The final time that an update will be drawn.

	public Timer (float timeForMove)
	{
		time = timeForMove;
		startTime = Time.time;
		nextTime = startTime;
		endTime = startTime + timeForMove;
	}

	public float Update()
	{
		if (Time.time >= nextTime)
		{
			return ((Time.time - startTime) / time);
		} else
		{
			return -1; // Dummy value so the traveler doesn't change position.
		}
	}
}
