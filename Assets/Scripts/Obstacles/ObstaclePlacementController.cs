using System.Collections.Generic;
using UnityEngine;
using static CameraController;
using static TrapTriggerBase;

public class ObstaclePlacementController : MonoBehaviour {
	#region Properties

	[HideInInspector]
	public bool validPlacement;
	[HideInInspector]
	public RaycastHit hitInformation;

	private bool positionFinalized;
	private Material validMaterial;
	private Material invalidMaterial;
	private Dictionary<string, int> layerMasks;
	private Vector3 minimumBoundary;
	private Vector3 maximumBoundary;

	private new Camera camera;
	private GameObject placementObject;
	private MeshRenderer placementObjectRenderer;
	private Collider placementObjectCollider;
	private TrapTriggerBase heldObjectTrapController;

	#endregion

	#region Events

	private void Awake() {
		camera = Camera.main;

		positionFinalized = false;
		validMaterial = Resources.Load("Materials/Obstacles/Placement/ValidPlacement") as Material;
		invalidMaterial = Resources.Load("Materials/Obstacles/Placement/InvalidPlacement") as Material;

		validPlacement = true;

		CreateLayerMasks();
	}

	private void Update() {
		if (!positionFinalized) {
			CheckForRaycastHit();
			MoveOutOfOverlap();

			if (validPlacement)
				validPlacement = PositionWithinBoundaries(transform.position);

			ValidPlacementChangeMaterial();

			if (Input.GetButtonDown("Fire1") && validPlacement) {
				FinalizePosition();
			}
		}
	}

	#endregion

	#region Methods

		#region Get/Set

		public bool IsPositionFinalized() {
			return positionFinalized;
		}

		#endregion

	private void CreateLayerMasks() {
		layerMasks = new Dictionary<string, int>();
		layerMasks.Add("Floor", 1 << LayerMask.NameToLayer("Floor"));
		layerMasks.Add("Wall", 1 << LayerMask.NameToLayer("Wall") | 1 << LayerMask.NameToLayer("WallShouldHide"));
		layerMasks.Add("WallDecoration", 1 << LayerMask.NameToLayer("WallDecoration"));
		layerMasks.Add("WallWithDecoration", layerMasks["Wall"] | layerMasks["WallDecoration"]);
		layerMasks.Add("Terrain", layerMasks["Floor"] | layerMasks["WallWithDecoration"]);
	}
	
	public void SetHeldObject(GameObject heldObject) {
		PickUpObject pickUpController = heldObject.GetComponent<PickUpObject>();

		if (pickUpController == null) {
			print($"{heldObject.name} can't be picked up");
			return;
		}

		placementObject = Instantiate(pickUpController.placementPrefab) as GameObject;
		placementObject.transform.parent = transform;
		placementObject.transform.localPosition = pickUpController.placementPrefab.transform.position;

		CopyColliderToPlacementModel(heldObject);

		placementObjectRenderer = placementObject.GetComponent<MeshRenderer>();
		heldObjectTrapController = heldObject.GetComponent<TrapTriggerBase>();

		SetPositionBoundaries();
	}

	private void CopyColliderToPlacementModel(GameObject heldObject) {
		var colliderToCopy = heldObject.GetComponent<BoxCollider>();

		if (colliderToCopy != null) {
			placementObjectCollider = placementObject.AddComponent<BoxCollider>(colliderToCopy);
			placementObjectCollider.isTrigger = true;
		} else {
			print($"{gameObject.name} doesn't have a box collider");
		}
	}

	private void CheckForRaycastHit() {
        var cameraToMouseRay = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(cameraToMouseRay, out hitInformation, Mathf.Infinity, layerMasks["Terrain"])) {

			if (heldObjectTrapController != null) {
				if (heldObjectTrapController.GetSurfaceType().ToString().ToLower() == hitInformation.collider.tag.ToLower()) {
					validPlacement = true;
				} else {
					validPlacement = false;
				}
			} else {
				validPlacement = true;
			}
	
			var (position, rotation) = GetValidPositionAndRotation(hitInformation);
			transform.position = position;
			transform.rotation = rotation;
        }
	}

	private (Vector3, Quaternion) GetValidPositionAndRotation(RaycastHit hitInformation) {
        var position = hitInformation.point;
		var rotation = transform.rotation;
		
		if (heldObjectTrapController == null) {
			var minY = placementObjectCollider.bounds.size.y / 2;

			if (position.y <= minY) {
				position.y = minY;
			}

			return (position, rotation);
		}

		if (heldObjectTrapController.GetSurfaceType() == SurfaceType.Wall) {
			Vector3 hitNormal = hitInformation.normal;
			rotation = Quaternion.LookRotation(hitNormal);

			if (Mathf.Abs(hitNormal.x) == 1) {
				position.x = hitInformation.point.x;
			} else if (Mathf.Abs(hitNormal.z) == 1) {
				position.z = hitInformation.point.z;
			}
		}

        return (position, rotation);
	}

	private void MoveOutOfOverlap() {
		Collider[] colliders = Physics.OverlapBox(placementObject.transform.position, placementObjectCollider.bounds.size / 2, Quaternion.identity, layerMasks["WallWithDecoration"]);

		foreach (Collider overlappedCollider in colliders) {
			Transform overlappedTransform = overlappedCollider.gameObject.transform;
			Vector3 direction;
			float distance;

			bool overlapped = Physics.ComputePenetration(
				placementObjectCollider, placementObject.transform.position, placementObject.transform.rotation,
				overlappedCollider, overlappedTransform.position, overlappedTransform.rotation,
				out direction, out distance
			);

			if (overlapped) {
				transform.position += (direction * distance);
			}
		}
	}

	private void SetPositionBoundaries() {
		(var xBounds, var zBounds) = GeneralHelper.GetFloorObjectBoundaries(true);
		var wallThicknessOffset = 0.8f;

		xBounds.x += placementObjectCollider.bounds.extents.x + wallThicknessOffset;
		xBounds.y -= placementObjectCollider.bounds.extents.x  + wallThicknessOffset;

		zBounds.x += placementObjectCollider.bounds.extents.z + wallThicknessOffset;
		zBounds.y -= placementObjectCollider.bounds.extents.z + wallThicknessOffset;

		var yBound = 6 - placementObjectCollider.bounds.extents.y;

		minimumBoundary = new Vector3(xBounds.x, 0, zBounds.x);
		maximumBoundary = new Vector3(xBounds.y, yBound, zBounds.y);
	}

	private bool PositionWithinBoundaries(Vector3 position) {
		if (position.x < minimumBoundary.x
		|| position.z < minimumBoundary.z) {
			return false;
		}

		if (position.x > maximumBoundary.x
		|| position.y > maximumBoundary.y
		|| position.z > maximumBoundary.z) {
			return false;
		}

		return true;
	}

	private void ValidPlacementChangeMaterial() {
		if (placementObjectRenderer != null) {
			if (validPlacement) {
				placementObjectRenderer.material = validMaterial;
			} else {
				placementObjectRenderer.material = invalidMaterial;
			}
		}
	}

	private void FinalizePosition() {
		positionFinalized = true;
		References.Camera.cameraController.SetControllingState(ControllingState.ControllingSelf);
		References.Game.gameTraps.DisableObstaclePlacementActionText();
	}

	#endregion
}
