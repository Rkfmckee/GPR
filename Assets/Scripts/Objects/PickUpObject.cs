using UnityEngine;

public class PickUpObject : MonoBehaviour {
	#region Properties

	public bool canBeThrown;
	public GameObject placementPrefab;
	[Range(1, 10)]
	public float heldHeight;
	[HideInInspector]
	public State currentState;

	private Vector3 heldPosition;
	private new Rigidbody rigidbody;

	#endregion

	#region Events

	private void Awake() {
		rigidbody = gameObject.GetComponent<Rigidbody>();

		currentState = State.IDLE;
		heldPosition = new Vector3(0, heldHeight, 0);
	}

	private void Update() {
		if (currentState == State.HELD) {
			transform.localPosition = heldPosition;
			transform.eulerAngles = Vector3.zero;
			if (rigidbody != null) rigidbody.velocity = Vector3.zero;
		}
	}

	private void OnCollisionEnter(Collision collision) {
		if (collision.collider.gameObject.tag == "Floor") {
			currentState = State.IDLE;
		}
	}

	#endregion

	#region Methods

	public void SetCurrentState(State state) {
		currentState = state;

		if (currentState == State.HELD) {
			if (rigidbody != null) rigidbody.useGravity = false;
		} else {
			if (rigidbody != null) rigidbody.useGravity = true;
		}
	}

	#endregion

	#region Enums

	public enum State {
		IDLE,
		HELD,
		THROWN
	}

	#endregion
}
