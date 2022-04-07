using System.Collections.Generic;
using UnityEngine;
using static CameraController;
using static CameraOrientationController;

public class CameraSeeThroughWalls : MonoBehaviour {
	#region Properties

	private List<GameObject> hiddenObjects;
	private int hiddenLayers;

	private CameraController cameraController;
	private CameraOrientationController orientationController;

	#endregion

	#region Events

	private void Awake() {
		cameraController = GetComponent<CameraController>();
		orientationController = GetComponent<CameraOrientationController>();
		
		hiddenObjects = new List<GameObject>();

		int wallMask = 1 << LayerMask.NameToLayer("Wall");
		int wallIgnoreMask = 1 << LayerMask.NameToLayer("WallIgnoreRaycast");
		hiddenLayers = wallMask | wallIgnoreMask;
	}

	private void Update() {
		if (cameraController.GetCurrentState() == CameraState.TRANSITIONING)
		return;

		if (orientationController.GetCurrentOrientation() == CameraOrientation.ANGLED) {
			SeeThroughWalls();
		}
	}

	#endregion

	#region Methods

	public void ClearAllHiddenObjects() {
		foreach (var hiddenObject in hiddenObjects) {
			ClearHiddenObject(hiddenObject);
		}

		hiddenObjects.Clear();
	}

	private void SeeThroughWalls() {
		Vector3 direction = transform.forward;

		RaycastHit[] hits = Physics.RaycastAll(transform.position, direction, Mathf.Infinity, hiddenLayers);
		Debug.DrawRay(transform.position, direction, Color.yellow);

		foreach (RaycastHit hit in hits) {
			GameObject currentHit = hit.transform.gameObject;

			if (!hiddenObjects.Contains(currentHit)) {
				hiddenObjects.Add(currentHit);
				currentHit.GetComponent<Renderer>().enabled = false;
				currentHit.layer = LayerMask.NameToLayer("WallIgnoreRaycast");
			}
		}

		CheckForNoLongerHiddenObjects(hits);
	}

	private void CheckForNoLongerHiddenObjects(RaycastHit[] hits) {
		for (int i = 0; i < hiddenObjects.Count; i++) {
			bool isHit = false;

			for (int j = 0; j < hits.Length; j++) {
				if (hits[j].transform.gameObject == hiddenObjects[i]) {
					isHit = true;
					break;
				}
			}

			if (!isHit) {
				ClearHiddenObject(hiddenObjects[i]);

				hiddenObjects.RemoveAt(i);
				i--;
			}
		}
	}

	private void ClearHiddenObject(GameObject hiddenObject) {
		hiddenObject.GetComponent<Renderer>().enabled = true;

		if (hiddenObject.layer == LayerMask.NameToLayer("WallIgnoreRaycast")) {
			hiddenObject.layer = LayerMask.NameToLayer("Wall");
		}
	}

	#endregion
}
