using System;
using System.Collections;
using System.Collections.Generic;
using Gamelogic.Extensions;
using Lean.Touch;
using UnityEngine;


public class TouchHandler : Singleton<TouchHandler>
{
	public static Action<Vector2, TouchPhase> ontouchPosition;
	private const float ClampValue = 20;

	private List<LeanFingerTrail.FingerData> fingerDatas = new List<LeanFingerTrail.FingerData>();

	protected virtual void OnEnable()
	{
		// Hook events
		LeanTouch.OnFingerSet += FingerSet;
		LeanTouch.OnFingerUp += FingerUp;
	}

	protected virtual void OnDisable()
	{
		// Unhook events
		LeanTouch.OnFingerSet -= FingerSet;
		LeanTouch.OnFingerUp -= FingerUp;
	}

//	void OnGesture(List<LeanFinger> fingers)
//	{
//        if (GameManager.Instance.State != GameState.Play)
//        {
//            return;
//        }

//		var delta = LeanGesture.GetScreenDelta(fingers);
//
//		var adjusted = Mathf.Clamp(-(delta.x + delta.y) * 0.37f, -ClampValue, ClampValue);

	//OnTouchRotate?.Invoke(adjusted);
//	}


	void FingerSet(LeanFinger finger)
	{
		// ignore?

		if (finger.StartedOverGui == true)
		{
			return;
		}


		// Get data for this finger
		var fingerData = LeanFingerData.FindOrCreate(ref fingerDatas, finger);
		foreach (var snapshot in finger.Snapshots)
		{
            ontouchPosition(snapshot.ScreenPosition, finger.Down ? TouchPhase.Began : TouchPhase.Moved);
		}

	}

	void FingerUp(LeanFinger finger)
	{
		// Find link for this finger, and clear it
		var fingerData = LeanFingerData.Find(ref fingerDatas, finger);

		if (fingerData != null)
		{
			fingerDatas.Remove(fingerData);

			ontouchPosition(finger.ScreenPosition, TouchPhase.Ended);
		}
	}
}