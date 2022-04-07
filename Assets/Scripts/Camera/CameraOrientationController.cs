using System.Collections;
using UnityEngine;
using static CameraController;

public class CameraOrientationController : MonoBehaviour {
	#region Properties

	public float orientationChangeTime;

	private CameraOrientation currentOrientation;
	private Quaternion angledRotation;
	private Quaternion topDownRotation;
	private float angledHeight;
	private float orientationChangeTimeCurrent;

	private CameraController cameraController;
	private CameraSeeThroughWalls seeThroughWalls;

	#endregion

	#region Events

	private void Awake() {
		cameraController = GetComponent<CameraController>();
		seeThroughWalls = GetComponent<CameraSeeThroughWalls>();
		
		angledRotation = transform.rotation;
		topDownRotation = Quaternion.Euler(90, 0, 0);
		angledHeight = transform.position.y;
		currentOrientation = CameraOrientation.ANGLED;
	}

	private void Update() {
		if (cameraController.GetCurrentState() == CameraState.TRANSITIONING)
		return;

		if (Input.GetButtonDown("Jump")) {
			ChangeOrientation();
		}
	}

	#endregion

	#region Methods

		#region Get/Set

		public CameraOrientation GetCurrentOrientation() {
			return currentOrientation;
		}

		public void SetCurrentOrientation(CameraOrientation orientation) {
			currentOrientation = orientation;
		}

		#endregion

	private void ChangeOrientation() {
		CameraOrientation orientation = CameraOrientation.TOP_DOWN;

		if (currentOrientation == CameraOrientation.TOP_DOWN) {
			orientation = CameraOrientation.ANGLED;
		}

		StartCoroutine(TransitionOrientation(orientation));
	}

	#endregion

	#region Coroutine

	private IEnumerator TransitionOrientation(CameraOrientation orientation) {
		cameraController.SetCurrentState(CameraState.TRANSITIONING);
		float topDownHeight = angledHeight + 7;

		orientationChangeTimeCurrent = 0;
		Quaternion rotationFrom = angledRotation;
		Quaternion rotationTo = topDownRotation;
		Vector3 heightFrom = new Vector3(transform.position.x, angledHeight, transform.position.z);
		Vector3 heightTo = new Vector3(transform.position.x, topDownHeight, transform.position.z);

		Vector2 zBounds = cameraController.GetZBounds();
		float clampZOffset = cameraController.GetClampZOffset();

		if (orientation == CameraOrientation.ANGLED) {	
			rotationFrom = topDownRotation;
			rotationTo = angledRotation;
			topDownHeight = transform.position.y;
			heightFrom = new Vector3(transform.position.x, topDownHeight, transform.position.z);
			heightTo = new Vector3(transform.position.x, angledHeight, transform.position.z);

			// Accomodate for the zOffset at the top
			var maxZ = zBounds.y - clampZOffset;
			if (transform.position.z > maxZ) {
				heightTo.z = maxZ;
			}
		} else {
			// Accomodate for the zOffset at the bottom
			if (transform.position.z < zBounds.x) {
				heightTo.z = zBounds.x;
			}
		}

		while (orientationChangeTimeCurrent < orientationChangeTime) {
			float time = orientationChangeTimeCurrent / orientationChangeTime;

			transform.rotation = Quaternion.Lerp(rotationFrom, rotationTo, time);
			transform.position = Vector3.Lerp(heightFrom, heightTo, time);
			orientationChangeTimeCurrent += Time.deltaTime;

			yield return null;
		}

		transform.position = heightTo;

		if (orientation == CameraOrientation.ANGLED) {
			transform.rotation = angledRotation;
		} else {
			transform.rotation = topDownRotation;
			seeThroughWalls.ClearAllHiddenObjects();
		}

		currentOrientation = orientation;
		cameraController.SetCurrentState(CameraState.CONTROLLED_MOVEMENT);
	}

	#endregion

	#region Enums

	public enum CameraOrientation {
		TOP_DOWN,
		ANGLED
	}

	#endregion
}
