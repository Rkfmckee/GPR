using System.Collections.Generic;
using UnityEngine;
using static CameraController;
using static CameraOrientationController;

public class CameraSeeThroughWalls : MonoBehaviour
{
	#region Properties

	private List<GameObject> hiddenObjects;
	private int hiddenLayers;
	private int floorLayer;

	private CameraController cameraController;
	private CameraOrientationController orientationController;

	#endregion

	#region Events

	private void Awake()
	{
		cameraController      = GetComponent<CameraController>();
		orientationController = GetComponent<CameraOrientationController>();

		hiddenObjects = new List<GameObject>();

		var layerMasks   = GeneralHelper.GetLayerMasks();
		floorLayer       = layerMasks["Floor"];
		hiddenLayers     = layerMasks["WallShouldHide"] | layerMasks["WallHidden"] | floorLayer;
	}

	private void Update()
	{
		if (cameraController.MovementState == CameraMovementState.Transitioning)
			return;

		if (orientationController.CurrentOrientation == CameraOrientation.ANGLED)
		{
			SeeThroughWalls();
		}
	}

	#endregion

	#region Methods

	public void ClearAllHiddenObjects()
	{
		foreach (var hiddenObject in hiddenObjects)
		{
			ClearHiddenObject(hiddenObject);
		}

		hiddenObjects.Clear();
	}

	private void SeeThroughWalls()
	{
		var cameraForwardRay = new Ray(transform.position, transform.forward);
		RaycastHit hitInformation;
		GameObject currentHit = null;

		if (Physics.Raycast(cameraForwardRay, out hitInformation, Mathf.Infinity, hiddenLayers))
		{
			if (hitInformation.transform.gameObject.tag == "Floor")
			{
				// Only include this so the Raycast won't hit the wall colliders
				// which extend below the floor level
				CheckForNoLongerHiddenObjects(currentHit);
				return;
			}

			Debug.DrawLine(transform.position, hitInformation.point, Color.yellow);
			currentHit = hitInformation.transform.gameObject;

			if (!hiddenObjects.Contains(currentHit))
			{
				hiddenObjects.Add(currentHit);
				currentHit.GetComponent<Renderer>().enabled = false;
				currentHit.layer = LayerMask.NameToLayer("WallHidden");
			}
		}

		CheckForNoLongerHiddenObjects(currentHit);
	}

	private void CheckForNoLongerHiddenObjects(GameObject currentHit)
	{
		for (int i = 0; i < hiddenObjects.Count; i++)
		{
			bool isHit = false;

			if (currentHit == hiddenObjects[i])
			{
				isHit = true;
				break;
			}

			if (!isHit)
			{
				ClearHiddenObject(hiddenObjects[i]);

				hiddenObjects.RemoveAt(i);
				i--;
			}
		}
	}

	private void ClearHiddenObject(GameObject hiddenObject)
	{
		hiddenObject.GetComponent<Renderer>().enabled = true;
		hiddenObject.layer = LayerMask.NameToLayer("WallShouldHide");
	}

	#endregion
}
