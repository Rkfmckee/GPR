using System.Collections.Generic;
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
		allPlayers = References.Player.players;
	}

	private void Update() {
		if (currentState != null) currentState.Update();
	}

	private void FixedUpdate() {
		if (currentState != null) currentState.FixedUpdate();
	}

	private void OnDestroy() {
		if (References.enemies.Contains(gameObject)) {
			References.enemies.Remove(gameObject);
		}

		if (References.enemies.Count <= 0) {
			if (References.GameController.roundStage != null) {
				References.GameController.roundStage.SetCurrentStage(new PreparingStage());
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
