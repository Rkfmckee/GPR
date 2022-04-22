﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour {
	#region Properties

	public float movementSpeed;

	private new Rigidbody rigidbody;
	private FieldOfView fieldOfView;
	private NavMeshAgent navMeshAgent;

	private List<GameObject> allPlayers;
	private GameObject targetPlayer;
	private Vector3 movementDirection;
	private EnemyState currentState;

	#endregion

	#region Events

	private void Awake() {
		rigidbody = GetComponent<Rigidbody>();
		fieldOfView = GetComponent<FieldOfView>();
		navMeshAgent = GetComponent<NavMeshAgent>();

		movementDirection = transform.forward;
		SetCurrentState(new EnemyStateWander(gameObject));
	}

	private void Start() {
		allPlayers = References.FriendlyCreature.goblins;
	}

	private void Update() {
		if (currentState != null) currentState.Update();
	}

	private void FixedUpdate() {
		if (currentState != null) currentState.FixedUpdate();
	}

	private void OnDestroy() {
		if (References.HostileCreature.enemies.Contains(gameObject)) {
			References.HostileCreature.enemies.Remove(gameObject);
		}

		if (References.HostileCreature.enemies.Count <= 0) {
			if (References.Game.roundStage != null) {
				References.Game.roundStage.SetCurrentStage(new PreparingStage());
			}
		}
	}

	private void OnCollisionEnter(Collision collision) {
		if (currentState != null) currentState.OnCollisionEnter(collision);
	}

	#endregion

	#region Methods

	public EnemyState GetCurrentState() {
		return currentState;
	}

	public void SetCurrentState(EnemyState state) {
		if (state is EnemyStateWander) {
			navMeshAgent.enabled = true;
		} else {
			navMeshAgent.enabled = false;
		}

		currentState = state;
	}

	#endregion
}
