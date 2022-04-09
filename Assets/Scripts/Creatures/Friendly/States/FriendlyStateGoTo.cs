using UnityEngine;

public class FriendlyStateGoTo : FriendlyState {
	#region Properties

	private int floorMask;

	private Camera camera;

	#endregion
	
	#region Constructor
	
	public FriendlyStateGoTo(GameObject gameObj) : base(gameObj) {
	}

	#endregion

	#region Events

	public override void Update() {
		Vector3? pointClicked = GetPointClicked();

		if (pointClicked.HasValue) {
			navMeshAgent.destination = pointClicked.Value;
		}
		
		animatorController.UpdateAnimatorValues(navMeshAgent.velocity.magnitude, 0);
	}

	public override void FixedUpdate() {
	}

	#endregion

	#region Methods

	protected override void SetupProperties() {
		base.SetupProperties();

		camera = Camera.main;
		floorMask = 1 << LayerMask.NameToLayer("Floor");
	}

	private Vector3? GetPointClicked() {
		if (Input.GetButtonDown("Fire1")) {
			Ray cameraToMouseRay = camera.ScreenPointToRay(Input.mousePosition);
			RaycastHit hitInformation;

			if (Physics.Raycast(cameraToMouseRay, out hitInformation, Mathf.Infinity, floorMask)) {
				return hitInformation.point;
			}
		}

		return null;
	}

	#endregion
}
