using System;
using System.Collections.Generic;
using UnityEngine;

public class SwingingTrap : TrapController {
	#region Properties

	[SerializeField]
	private float swingingTime;
	[SerializeField]
	private float pauseTime;

	protected Transform swingingChild;

	private bool isPaused;
	private Quaternion leftRotation;
	private Quaternion rightRotation;
	private SwingState currentState;
	private Dictionary<SwingState, Action> swingStateBehaviour;
	private float currentTimeSwinging;
	private float currentPausedTime;

	#endregion

	#region Events

	protected override void Awake() {
		base.Awake();

		leftRotation = Quaternion.Euler(new Vector3(0, 0, -90));
		rightRotation = Quaternion.Euler(new Vector3(0, 0, 90));
		
		swingStateBehaviour = new Dictionary<SwingState, Action> {
			{ SwingState.Left, SwingRight },
			{ SwingState.Right, SwingLeft }
		};

		currentState = SwingState.Right;
	}

	protected override void Update() {
		if (IsObstacleDisabled()) {
			return;
		}

		if (isPaused) {
			Pause();
			return;
		}

		if (swingStateBehaviour.ContainsKey(currentState)) {
			swingStateBehaviour[currentState]();
		}
	}

	#endregion

	#region Methods

	private void SwingLeft() {
		currentTimeSwinging += Time.deltaTime;
		SwingChild(rightRotation, leftRotation, ref currentTimeSwinging, swingingTime, SwingState.Left);
	}

	private void SwingRight() {
		currentTimeSwinging += Time.deltaTime;
		SwingChild(leftRotation, rightRotation, ref currentTimeSwinging, swingingTime, SwingState.Right);
	}

	private void SwingChild(Quaternion startRotation, Quaternion finishRotation, ref float timer, float totalTime, SwingState finishState) {
		swingingChild.localRotation = Quaternion.Lerp(startRotation, finishRotation, timer / totalTime);

		if (Quaternion.Angle(swingingChild.localRotation, finishRotation) <= 0.1) {
			currentState = finishState;
			timer = 0;
			isPaused = true;
		}
	}

	private void Pause() {
		// Only automatically unpause if not linked to a trigger
		if (GetLinkedTrigger() != null) {
			return;
		}

		currentPausedTime += Time.deltaTime;

		if (currentPausedTime >= pauseTime) {
			currentPausedTime = 0;

			isPaused = false;
		}
	}

	public override void TriggerTrap(Collider triggeredBy) {
		base.TriggerTrap(triggeredBy);

		isPaused = false;
	}

	#endregion

	#region Enums

	public enum SwingState {
		Left,
		Right,
		Pause
	}

	#endregion
}
