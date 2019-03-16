﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]

public class PlayerMoveController : MonoBehaviour {

	// PUBLIC
	public SimpleTouchController leftController;
	public SimpleTouchController rightController;
	public float speedMovements = 5f;
	public float speedContinuousLook = 100f;
	public float speedProgressiveLook = 3000f;
	public bool continuousRightController = true;

	// PRIVATE
	private Rigidbody _rigidbody;
	private Vector3 localEulRot;
	private Vector2 prevRightTouchPos;

	void Awake()
	{
		_rigidbody = GetComponent<Rigidbody>();
		rightController.TouchEvent += RightController_TouchEvent;
		rightController.TouchStateEvent += RightController_TouchStateEvent;
	}

	public bool ContinuousRightController
	{
		set{continuousRightController = value;}
	}

	void RightController_TouchStateEvent (bool touchPresent)
	{
		if(!continuousRightController)
		{
			prevRightTouchPos = Vector2.zero;
		}
	}

	void RightController_TouchEvent (Vector2 value)
	{
		if(!continuousRightController)
		{
			Vector2 deltaValues = value - prevRightTouchPos;
			prevRightTouchPos = value;

			Quaternion rot = Quaternion.Euler(transform.localEulerAngles.x - rightController.GetTouchPosition.y * Time.deltaTime * speedContinuousLook,
				transform.localEulerAngles.y + rightController.GetTouchPosition.x * Time.deltaTime * speedContinuousLook,
				0f);

			_rigidbody.MoveRotation(rot);
		}
	}

	void Update()
	{
		string[] names = Input.GetJoystickNames();
		for (int x = 0; x < names.Length; x++)
         {
			print(names[x].Length);
			/*
			if (names[x].Length == 19)
			{
				print("PS4 CONTROLLER IS CONNECTED");
			}
			*/
			if (names[x].Length == 33)
			{
			print("XBOX ONE CONTROLS ACTIVE");
			float moveX = Input.GetAxis ("Xbox U/D J1");
			float moveY = Input.GetAxis ("Xbox L/R J1");
			float rotY = Input.GetAxis ("Xbox U/D J2");
			float rotx = Input.GetAxis ("Xbox L/R J2");
			float triggers = Input.GetAxis("Xbox Trigger");

			Vector3 movement = new Vector3 (moveX, triggers, moveY);
			gameObject.GetComponent<Rigidbody>().velocity = movement * 100;
			transform.Rotate(rotY, rotx, 0);
			}
		}

		_rigidbody.MovePosition(transform.position + (transform.forward * leftController.GetTouchPosition.y * Time.deltaTime * speedMovements) +
			(transform.right * leftController.GetTouchPosition.x * Time.deltaTime * speedMovements) );

		if(continuousRightController)
		{
			Quaternion rot = Quaternion.Euler(transform.localEulerAngles.x - rightController.GetTouchPosition.y * Time.deltaTime * speedContinuousLook,
				transform.localEulerAngles.y + rightController.GetTouchPosition.x * Time.deltaTime * speedContinuousLook,
				0f);

			_rigidbody.MoveRotation(rot);
		
		}
	}

	void OnDestroy()
	{
		rightController.TouchEvent -= RightController_TouchEvent;
		rightController.TouchStateEvent -= RightController_TouchStateEvent;
	}

}
