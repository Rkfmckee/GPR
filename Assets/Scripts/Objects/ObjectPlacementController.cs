using System.Collections.Generic;
using UnityEngine;
using static TrapController;

public class ObjectPlacementController : MonoBehaviour {
	#region Properties

	public float maxPlacementDistance;
	[HideInInspector]
	public bool validPlacement;
	[HideInInspector]
	public RaycastHit hitInformation;

	private Material validMaterial;
	private Material invalidMaterial;
	private Dictionary<string, string> placementPrefabs;
	private List<string> collidersToHit;
	private int floorMask;
	private int wallMask;
	private int terrainMask;
	private int maskToUse;
	private SurfaceType? surfaceToPlaceOn;

	private new Camera camera;
	private GameObject placementPrefab;
	private GameObject placementObject;
	private MeshRenderer placementObjectRenderer;
	private Collider placementObjectCollider;

	#endregion

	#region Events

	private void Awake() {
		camera = Camera.main;

		validMaterial = Resources.Load("Materials/Objects/Placement/ValidPlacement") as Material;
		invalidMaterial = Resources.Load("Materials/Objects/Placement/InvalidPlacement") as Material;

		validPlacement = true;

		floorMask = 1 << LayerMask.NameToLayer("Floor");
        wallMask = 1 << LayerMask.NameToLayer("Wall");
        terrainMask = floorMask | wallMask;

		SetPlacementPrefabs();
		SetCollidersToHit();
	}

	private void Update() {
		CheckForRaycastHit();
		ValidPlacementChangeMaterial();
	}

	#endregion

	#region Methods

	public void SetHeldObject(GameObject heldObject) {
		string objectName = heldObject.name;

		if (!placementPrefabs.ContainsKey(objectName)) {
			print($"Placement prefab not defined for {objectName}");
			return;
		}

		maskToUse = LayerMaskToUse(heldObject);

		placementPrefab = Resources.Load($"Prefabs/Objects/Placement/{placementPrefabs[objectName]}") as GameObject;
		placementObject = Instantiate(placementPrefab) as GameObject;
		placementObject.transform.parent = transform;
		placementObject.transform.localPosition = placementPrefab.transform.position;

		if (placementObject.transform.childCount > 1) {
			print($"{objectName} has more than 1 child. It may not display properly.");
		}

		CopyColliderToPlacementModel(heldObject);

		placementObjectRenderer = placementObject.transform.GetComponentInChildren<MeshRenderer>();
	}

	private void SetPlacementPrefabs() {
		placementPrefabs = new Dictionary<string, string> {
			{"Crate", "CratePlacement"},
			{"SpikeTrap", "SpikeTrapPlacement"}
		};
	}

	private void SetCollidersToHit() {
		collidersToHit = new List<string> {
			"Wall"
		};
	}

	private int LayerMaskToUse(GameObject heldObject) {
		surfaceToPlaceOn = heldObject.GetComponent<TrapController>() ? heldObject.GetComponent<TrapController>().GetSurfaceType() : SurfaceType.ANY;

		switch(surfaceToPlaceOn) {
			case SurfaceType.FLOOR:
			return floorMask;

			case SurfaceType.WALL:
			return wallMask;

			default:
			return terrainMask;
		}
	}

	private void CopyColliderToPlacementModel(GameObject heldObject) {
		BoxCollider heldObjectCollider = heldObject.GetComponent<BoxCollider>();

		if (heldObjectCollider != null) {
			placementObjectCollider = placementObject.AddComponent<BoxCollider>(heldObjectCollider);
		} else {
			print($"{gameObject.name} doesn't have a box collider");
		}
	}

	private void CheckForRaycastHit() {
        var cameraToMouseRay = camera.ScreenPointToRay(Input.mousePosition);
        validPlacement = true;

        if (Physics.Raycast(cameraToMouseRay, out hitInformation, Mathf.Infinity, maskToUse)) {
            Vector3 pointHit = hitInformation.point;
            validPlacement = GetValidDistance(pointHit);
			transform.position = pointHit;

			Vector3? positionToMove = CheckForOverlap(hitInformation);
			if (positionToMove.HasValue) {
				transform.position = positionToMove.Value;
			}
        }
	}

	private Vector3? CheckForOverlap(RaycastHit hitInformation) {
		Collider otherCollider = hitInformation.collider;
		Vector3 otherPosition = otherCollider.gameObject.transform.position;
		Quaternion otherRotation = otherCollider.gameObject.transform.rotation;
		Vector3 direction;
		float distance;

		bool overlapped = Physics.ComputePenetration(
			placementObjectCollider, placementObject.transform.position, placementObject.transform.rotation,
			otherCollider, otherPosition, otherRotation,
			out direction, out distance
		);

		if (overlapped) {
			return Vector3.MoveTowards(transform.position, direction, distance);
		}

		return null;
	}

    private bool GetValidDistance(Vector3 pointHit) {
        return Vector3.Distance(References.Player.currentPlayer.transform.position, pointHit) <= maxPlacementDistance;
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

	#endregion
}
