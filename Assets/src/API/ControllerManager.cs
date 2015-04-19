﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ControllerManager : MonoBehaviour 
{

	void Start()
	{
		if (ControllerInterface.Instance == null)
		{
			new ControllerInterface();
		}
	}

	void Update()
	{
		ControllerInterface.Instance.UpdateInternal();
	}

}
