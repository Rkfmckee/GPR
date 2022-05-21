using System.Collections.Generic;
using UnityEngine;
using static CameraController;
using static ObstacleController;

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
	private float ceilingHeight;
	private List<string> actionText;

	private new Camera camera;
	private GameObject placementObject;
	private MeshRenderer[] placementObjectRenderers;
	private Collider placementObjectCollider;
	private ObstacleController heldObjectObstacleController;

	#endregion

	#region Events

	private void Awake() {
		camera = References.Camera.camera;
		ceilingHeight = References.Game.globalObstacles.GetMaxObstacleHeight();

		positionFinalized = false;
		validMaterial = Resources.Load("Materials/Obstacles/Placement/ValidPlacement") as Material;
		invalidMaterial = Resources.Load("Materials/Obstacles/Placement/InvalidPlacement") as Material;

		validPlacement = true;

		layerMasks = GeneralHelper.GetLayerMasks();
	}

	private void Update() {
		if (!positionFinalized) {
			CheckForRaycastHit();
			MoveOutOfOverlap();

			if (validPlacement){
				validPlacement = PositionWithinBoundaries(transform.position);
			}

			ValidPlacementChangeMaterial();

			if (Input.GetButtonDown("Fire1") && validPlacement) {
				FinalizePosition();
			}

			if (Input.GetButtonDown("Fire2")) {
				RotateIfNotWallObstacle();
			}
		}
	}

	#endregion

	#region Methods

		#region Get/Set

		public bool IsPositionFinalized() {
			return positionFinalized;
		}

		public List<string> GetActionText() {
			return actionText;
		}

		#endregion
	
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

		placementObjectRenderers = placementObject.GetComponentsInChildren<MeshRenderer>();
		heldObjectObstacleController = heldObject.GetComponent<ObstacleController>();

		SetPositionBoundaries();

		actionText = new List<string> {
			"Left click to Place"
		};
		if (heldObjectObstacleController != null) {
			if (heldObjectObstacleController.GetSurfaceType() == SurfaceType.Wall) {
				return;
			}
		}
		actionText.Add("Right click to Rotate");
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

			if (heldObjectObstacleController != null) {
				if (heldObjectObstacleController.GetSurfaceType().ToString().ToLower() == hitInformation.collider.tag.ToLower() ||
					(heldObjectObstacleController.GetSurfaceType() == SurfaceType.Ceiling && hitInformation.collider.tag == "Floor") ||
					heldObjectObstacleController.GetSurfaceType() == SurfaceType.Any) {
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
		
		if (heldObjectObstacleController == null) {
			return (MoveColliderVertically(placementObjectCollider, position), rotation);
		}

		if (heldObjectObstacleController.GetSurfaceType() == SurfaceType.Floor) {
			if (heldObjectObstacleController.gameObject.GetComponent<Rigidbody>()) {
				// If it is a physics object, it's pivot point is likely in the center of it's collider
				// So adjust it's position to account for that
				return (MoveColliderVertically(placementObjectCollider, position), rotation);
			}
		} else if (heldObjectObstacleController.GetSurfaceType() == SurfaceType.Wall) {
			Vector3 hitNormal = hitInformation.normal;
			rotation = Quaternion.LookRotation(hitNormal);

			if (Mathf.Abs(hitNormal.x) == 1) {
				position.x = hitInformation.point.x;
			} else if (Mathf.Abs(hitNormal.z) == 1) {
				position.z = hitInformation.point.z;
			}
		} else if (heldObjectObstacleController.GetSurfaceType() == SurfaceType.Ceiling) {
			position.y = ceilingHeight - 0.5f;
		}

		return (position, rotation);
	}

	private Vector3 MoveColliderVertically(Collider collider, Vector3 position) {
		var minY = collider.bounds.size.y / 2;

			if (position.y <= minY) {
				position.y = minY;
			}

			return position;
	}

	private void MoveOutOfOverlap() {
		Collider[] colliders = Physics.OverlapBox(placementObject.transform.position, placementObjectCollider.bounds.size / 2, Quaternion.identity, layerMasks["TerrainWithWallHidden"]);

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

		var yBound = ceilingHeight - placementObjectCollider.bounds.extents.y;
		if (heldObjectObstacleController.GetSurfaceType() == SurfaceType.Ceiling) {
			yBound = ceilingHeight;
		}

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
			print($"max {maximumBoundary}");
			return false;
		}

		return true;
	}

	private void ValidPlacementChangeMaterial() {
		Material newMaterial;
		if (validPlacement) newMaterial = validMaterial;
		else newMaterial = invalidMaterial;
		
		if (placementObjectRenderers.Length > 0) {
			foreach(var renderer in placementObjectRenderers) {
				renderer.material = newMaterial;
			}
		}
	}

	private void FinalizePosition() {
		positionFinalized = true;
		References.Camera.cameraController.SetControllingState(ControllingState.ControllingSelf);
		References.UI.canvasController.DisableActionText(actionText);
	}

	private void RotateIfNotWallObstacle() {
		if (heldObjectObstacleController != null) {
			if (heldObjectObstacleController.GetSurfaceType() == SurfaceType.Wall) {
				return;
			}
		}

		transform.Rotate(new Vector3(0, 45, 0));
	}

	#endregion
}
