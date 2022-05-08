using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ExtendingTrap : TrapController {
	public float extendingTime;
	public float retractingTime;
	public float timeToStayExtended;

	protected Vector3 extendedScale;
	protected Vector3 retractedScale;
	protected GameObject extendingChildPrefab;

	private ExtendedState currentState;
	private float currentTimeExtending;
	private float currentTimeRetracting;
	private float currentTimeExtended;
	private Dictionary<ExtendedState, Action> extendingStateBehaviour;

	private Transform extendingChild;

	#region Events

	protected override void Awake() {
		base.Awake();

		currentTimeExtending = 0;
		currentTimeRetracting = 0;
		currentTimeExtended = 0;

		currentState = ExtendedState.Retracted;

		extendingStateBehaviour = new Dictionary<ExtendedState, Action> {
			{ ExtendedState.Extending, Extending },
			{ ExtendedState.Extended, Extended },
			{ ExtendedState.Retracting, Retracting },
			{ ExtendedState.Retracted, Retracted }
		};
	}

	private void Update() {
		extendingStateBehaviour[currentState]();
	}

	#endregion

	#region Methods

		#region Get/Set

		public ExtendedState GetCurrentState() {
			return currentState;
		}

		#endregion

	public override void TriggerTrap(Collider triggeredBy) {
		base.TriggerTrap(triggeredBy);
		
		if (currentState == ExtendedState.Retracted) {
			extendingChild = Instantiate(extendingChildPrefab, transform).transform;

			currentState = ExtendedState.Extending;
		}
	}

	private void Extending() {
		currentTimeExtending += Time.deltaTime;
		ExtendOrRetractChild(retractedScale, extendedScale, ref currentTimeExtending, extendingTime, ExtendedState.Extended);
	}

	private void Extended() {
		currentTimeExtended += Time.deltaTime;

		if (currentTimeExtended >= timeToStayExtended) {
			currentState = ExtendedState.Retracting;
			currentTimeExtended = 0;
		}
	}

	private void Retracting() {
		currentTimeRetracting += Time.deltaTime;
		ExtendOrRetractChild(extendedScale, retractedScale, ref currentTimeRetracting, retractingTime, ExtendedState.Retracted);
	}

	private void Retracted() {
		if (extendingChild != null) {
			Destroy(extendingChild.gameObject);
		}
	}

	private void ExtendOrRetractChild(Vector3 startScale, Vector3 finishScale, ref float timer, float totalTime, ExtendedState finishState) {
		extendingChild.localScale = Vector3.Lerp(startScale, finishScale, timer / totalTime);

		if (extendingChild.localScale == finishScale) {
			currentState = finishState;
			timer = 0;
		}
	}

	#endregion

	#region Enums

	public enum ExtendedState {
		Extended,
		Extending,
		Retracted,
		Retracting
	}

	#endregion
}
