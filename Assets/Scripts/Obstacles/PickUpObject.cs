using System.Collections.Generic;
using UnityEngine;

public class PickUpObject : MonoBehaviour {
	#region Properties

	public GameObject placementPrefab;
	public GameObject heldPrefab;
	[Range(1, 10)]
	public float heldHeight;
	
	private PickUpState currentState;
	private Vector3 heldPosition;
	private new Rigidbody rigidbody;
	private Animator animator;
	private GameObject heldObject;
	private Transform currentlyHeldBy;

	private List<Rigidbody> disabledRigidbodies;
	private List<Collider> disabledColliders;
	private List<Renderer> disabledRenderers;
	private List<Animator> disabledAnimators;

	#endregion

	#region Events

	private void Awake() {
		rigidbody = gameObject.GetComponent<Rigidbody>();
		animator = gameObject.GetComponent<Animator>();

		disabledRigidbodies = new List<Rigidbody>();
		disabledColliders = new List<Collider>();
		disabledRenderers = new List<Renderer>();
		disabledAnimators = new List<Animator>();

		currentState = PickUpState.Idle;
		heldPosition = new Vector3(0, heldHeight, 0);
	}

	private void Update() {
		if (heldObject == null) {
			return;
		}

		ResetHeldObjectPosition();
	}

	private void OnCollisionEnter(Collision collision) {
		if (collision.collider.gameObject.tag == "Floor") {
			currentState = PickUpState.Idle;
		}
	}

	#endregion

	#region Methods

		#region Get/Set

		public PickUpState GetCurrentState() {
			return currentState;
		}

		public void SetCurrentState(PickUpState state, Transform heldBy = null) {
			currentState = state;
			currentlyHeldBy = heldBy;

			if (currentState == PickUpState.Held) {
				DisableComponents();
				heldObject = Instantiate(heldPrefab, currentlyHeldBy);
				ResetHeldObjectPosition();

			} else {
				if (heldObject != null) {
					EnableComponents();
					Destroy(heldObject);
				}
			}
		}

		#endregion

	private void ResetHeldObjectPosition() {
		heldObject.transform.localPosition = heldPosition;
		heldObject.transform.rotation = heldPrefab.transform.rotation;
	}

	private void EnableComponents() {
		foreach(var rigidbody in disabledRigidbodies) {
			rigidbody.isKinematic = false;
		}
		disabledRigidbodies.Clear();
		
		foreach(var collider in disabledColliders) {
			collider.enabled = true;
		}
		disabledColliders.Clear();

		foreach(var renderer in disabledRenderers) {
			renderer.enabled = true;
		}
		disabledRenderers.Clear();

		foreach(var animator in disabledAnimators) {
			animator.enabled = true;
		}
		disabledAnimators.Clear();
	}

	public void DisableComponents() {
		var allRigidbodies = GetComponentsInChildren<Rigidbody>();
		var allColliders = GetComponentsInChildren<Collider>();
		var allRenderers = GetComponentsInChildren<Renderer>();
		var allAnimators = GetComponentsInChildren<Animator>();

		foreach(var rigidbody in allRigidbodies) {
			if (!rigidbody.isKinematic) {
				rigidbody.isKinematic = true;
				disabledRigidbodies.Add(rigidbody);
			}
		}
		
		foreach(var collider in allColliders) {
			if (collider.enabled) {
				collider.enabled = false;
				disabledColliders.Add(collider);
			}
		}

		foreach(var renderer in allRenderers) {
			if (renderer.enabled) {
				renderer.enabled = false;
				disabledRenderers.Add(renderer);
			}
		}

		foreach(var animator in allAnimators) {
			if (animator.enabled) {
				animator.enabled = false;
				disabledAnimators.Add(animator);
			}
		}
	}

	#endregion

	#region Enums

	public enum PickUpState {
		Idle,
		Held,
		Thrown
	}

	#endregion
}
