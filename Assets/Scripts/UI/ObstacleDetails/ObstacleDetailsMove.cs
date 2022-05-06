using UnityEngine;

public class ObstacleDetailsMove : ObstacleDetails {
	#region Properties

	private Vector3 mouseOffset;

	private new Camera camera;

	#endregion

	#region Events

	protected override void Awake() {
		base.Awake();

		var backgroundTransform = background.GetComponent<RectTransform>();
		mouseOffset = Resources.Load<GameObject>("Prefabs/UI/ObstacleDetails/ObstacleDetails").transform.position;
	}

	private void Start() {
		camera = References.Camera.camera;
	}

	private void Update() {
		transform.position = Input.mousePosition + mouseOffset;
	}

	#endregion
}
