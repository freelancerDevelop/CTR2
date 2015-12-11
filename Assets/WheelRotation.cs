﻿using UnityEngine;
using System.Collections;

public class WheelRotation : MonoBehaviour
{

	public GameObject LeftWheel, RightWheel;
	public Vector3 InitialLeftRotation;

	public void Start()
	{
		InitialLeftRotation = LeftWheel.transform.localRotation.eulerAngles;
	}

	public void TurnRight()
	{
		if(m_coefRotation<m_maxAngle)
		{
			LeftWheel.transform.localRotation = Quaternion.Euler(new Vector3(LeftWheel.transform.localRotation.x+m_coefRotation,0,0)+InitialLeftRotation);
			RightWheel.transform.localRotation = LeftWheel.transform.localRotation;
			m_coefRotation+=2*80*Time.deltaTime;
		}
	}
	public void TurnLeft()
	{
		if(m_coefRotation>-m_maxAngle)
		{
			LeftWheel.transform.localRotation = Quaternion.Euler(new Vector3(LeftWheel.transform.localRotation.x+m_coefRotation,0,0)+InitialLeftRotation);
			RightWheel.transform.localRotation = LeftWheel.transform.localRotation;
			m_coefRotation-=2*80*Time.deltaTime;
		}
	}
	public void Reset()
	{
		if (m_coefRotation > 0)
			TurnLeft ();
		else if (m_coefRotation < 0)
			TurnRight ();
	}

	private float m_maxAngle=40;
	private float m_coefRotation = 0;
}
