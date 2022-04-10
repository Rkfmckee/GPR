using UnityEngine;

internal class FriendlyStateListening : FriendlyState {
	#region Properties

	private int floorMask;
	private float ignoreMouseClickTimer;
	private float ignoreMouseClickTime;

	private Camera camera;

	#endregion
	
	#region Constructor
	
	public FriendlyStateListening(GameObject gameObj) : base(gameObj) {
		ResetIgnoreMouseClickTimer();
	}

	#endregion

	#region Events

	public override void Update() {
		base.Update();

		Vector3? pointClicked = GetPointClicked();

		if (pointClicked.HasValue) {
			behaviour.SetCurrentState(new FriendlyStateGoTo(gameObject, pointClicked.Value));
		}
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

	private bool ShouldIgnoreMouseClick() {
		if (ignoreMouseClickTimer < ignoreMouseClickTime) {
			ignoreMouseClickTimer += Time.deltaTime;
			return true;
		}

		return false;
	}

	private void ResetIgnoreMouseClickTimer() {
		ignoreMouseClickTimer = 0;
		ignoreMouseClickTime = 1;
	}

	private Vector3? GetPointClicked() {
		if (ShouldIgnoreMouseClick()) return null;

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