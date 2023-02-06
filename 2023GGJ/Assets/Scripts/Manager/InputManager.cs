using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using HedgehogTeam.EasyTouch;

namespace GGJ {

    public class InputManager : Singleton<InputManager> 
    {

		public bool isDraging { get; private set; }
		public Vector2 inertiaVelocity { get; private set; }
		public Vector2 inertiaPos { get; private set; }

		public const float TouchSize = 6f;
		public Rect TouchArea { get; private set; } = new Rect(-TouchSize, -TouchSize, TouchSize * 2, TouchSize * 2);

		protected override void Awake()
		{
			base.Awake();
			EasyTouch.On_TouchStart += OnTouchStart;
			EasyTouch.On_DoubleTap += OnDoubleClick;
			EasyTouch.On_TouchDown += OnTouchDown;
			EasyTouch.On_TouchUp += OnTouchUp;
		}

		private void FixedUpdate()
		{
			//if (!isDraging && inertiaVelocity.magnitude != 0)
			//{
			//	inertiaPos += inertiaVelocity * Time.deltaTime;
			//	inertiaVelocity *= 0.97f;
			//	inertiaPos = TouchArea.Clamp(inertiaPos);
			//	BallManager.Instance.SetRootTarget(inertiaPos);
			//}
		}

		private void Update()
		{
			if (Input.GetMouseButtonDown(1))
			{
				BallManager.Instance.ChangeRootElement(TouchArea.Clamp(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
				BallManager.Instance.SetRootTarget(TouchArea.Clamp(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
			}
		}

		private void OnTouchUp(Gesture gesture)
		{
			//if (isDraging)
			//{
			//	isDraging = false;
			//	inertiaPos = gesture.FixedTouchPos();
			//	inertiaVelocity = (new Vector2(gesture.deltaPosition.x / Camera.main.pixelWidth, gesture.deltaPosition.y / Camera.main.pixelHeight) / gesture.deltaTime);
			//}
		}

		private void OnTouchDown(Gesture gesture)
		{
			//if (isDraging) 
			BallManager.Instance.SetRootTarget(gesture.FixedTouchPos());
		}

		private void OnTouchStart(Gesture gesture)
		{
			//isDraging =
			//BallManager.Instance.CheckClickRoot(gesture.FixedTouchPos());
		}

		//ÐÞbug
		private void OnDoubleClick(Gesture gesture)
		{
			BallManager.Instance.ChangeRootElement(gesture.FixedTouchPos());
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			EasyTouch.On_TouchStart -= OnTouchStart;
			EasyTouch.On_TouchDown -= OnTouchDown;
			EasyTouch.On_TouchUp -= OnTouchUp;
		}
	}
}