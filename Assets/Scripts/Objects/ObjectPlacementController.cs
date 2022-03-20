using System;
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
	private Dictionary<string, int> layerMasks;
	private SurfaceType? surfaceToPlaceOn;

	private new Camera camera;
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

		CreateLayerMasks();
		SetPlacementPrefabs();
	}

	private void Update() {
		CheckForRaycastHit();
		MoveOutOfOverlap();
		ValidPlacementChangeMaterial();
	}

	#endregion

	#region Methods

	public void SetHeldObject(GameObject heldObject) {
		PickUpController pickUpController = heldObject.GetComponent<PickUpController>();

		if (pickUpController == null) {
			print($"{heldObject.name} can't be picked up");
			return;
		}

		placementObject = Instantiate(pickUpController.placementPrefab) as GameObject;
		placementObject.transform.parent = transform;
		placementObject.transform.localPosition = pickUpController.placementPrefab.transform.position;

		CopyColliderToPlacementModel(heldObject);

		placementObjectRenderer = placementObject.GetComponent<MeshRenderer>();
	}

	private void CreateLayerMasks() {
		layerMasks = new Dictionary<string, int>();
		layerMasks.Add("Floor", 1 << LayerMask.NameToLayer("Floor"));
		layerMasks.Add("Wall", 1 << LayerMask.NameToLayer("Wall"));
		layerMasks.Add("WallDecoration", 1 << LayerMask.NameToLayer("WallDecoration"));
		layerMasks.Add("WallWithDecoration", layerMasks["Wall"] | layerMasks["WallDecoration"]);
		layerMasks.Add("Terrain", layerMasks["Floor"] | layerMasks["WallWithDecoration"]);
	}

	private void SetPlacementPrefabs() {
		placementPrefabs = new Dictionary<string, string> {
			{"Crate", "CratePlacement"},
			{"SpikeTrap", "SpikeTrapPlacement"}
		};
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

        if (Physics.Raycast(cameraToMouseRay, out hitInformation, Mathf.Infinity, layerMasks["Terrain"])) {
            Vector3 pointHit = hitInformation.point;
            validPlacement = GetValidDistance(pointHit);
			transform.position = pointHit;
        }
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
