using System.Collections.Generic;
using UnityEngine;
using static TrapController;

public class ObjectPlacementController : MonoBehaviour {
	#region Properties

	[HideInInspector]
	public bool validPlacement;
	[HideInInspector]
	public RaycastHit hitInformation;
	public float maxPlacementDistance;

	private Material validMaterial;
	private Material invalidMaterial;
	private Dictionary<string, string> placementPrefabs;
	private int floorMask;
	private int wallMask;
	private int terrainMask;
	private int maskToUse;
	private SurfaceType? surfaceToPlaceOn;

	private new Camera camera;
	private GameObject placementObject;
	private MeshRenderer placementObjectRenderer;

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
	}

	private void Update() {
		CheckForRaycastHit();
		ValidPlacementChangeMaterial();
	}

	// public void OnCollisionEnter(Collision other) {
	// 	if (other.gameObject.tag != "Wall") {
	// 		Physics.IgnoreCollision(child, other.collider);
	// 	}
	// }

	#endregion

	#region Methods

	public void SetHeldObject(GameObject heldObject) {
		string objectName = heldObject.name;

		if (!placementPrefabs.ContainsKey(objectName)) {
			print($"Placement prefab not defined for {objectName}");
			return;
		}

		maskToUse = LayerMaskToUse(heldObject);

		GameObject placementPrefab = Resources.Load($"Prefabs/Objects/Placement/{placementPrefabs[objectName]}") as GameObject;
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
			placementObject.AddComponent<BoxCollider>(heldObjectCollider);
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
        }
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
