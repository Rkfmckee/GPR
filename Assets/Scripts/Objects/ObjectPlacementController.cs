using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacementController : MonoBehaviour {
	#region Properties

	[HideInInspector]
	public bool validPlacement;
	[HideInInspector]
	public RaycastHit hitInformation;

	private bool positionFinalized;
	private Material validMaterial;
	private Material invalidMaterial;
	private Dictionary<string, int> layerMasks;

	private new Camera camera;
	private GameObject placementObject;
	private MeshRenderer placementObjectRenderer;
	private Collider placementObjectCollider;
	private TrapController heldObjectTrapController;

	#endregion

	#region Events

	private void Awake() {
		camera = Camera.main;

		positionFinalized = false;
		validMaterial = Resources.Load("Materials/Objects/Placement/ValidPlacement") as Material;
		invalidMaterial = Resources.Load("Materials/Objects/Placement/InvalidPlacement") as Material;

		validPlacement = true;

		CreateLayerMasks();
	}

	private void Update() {
		if (!positionFinalized) {
			CheckForRaycastHit();
			MoveOutOfOverlap();
			ValidPlacementChangeMaterial();

			if (Input.GetButtonDown("Fire1")) {
				positionFinalized = validPlacement;
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
		heldObjectTrapController = heldObject.GetComponent<TrapController>();
	}

	private void CreateLayerMasks() {
		layerMasks = new Dictionary<string, int>();
		layerMasks.Add("Floor", 1 << LayerMask.NameToLayer("Floor"));
		layerMasks.Add("Wall", 1 << LayerMask.NameToLayer("Wall") | 1 << LayerMask.NameToLayer("WallShouldHide"));
		layerMasks.Add("WallDecoration", 1 << LayerMask.NameToLayer("WallDecoration"));
		layerMasks.Add("WallWithDecoration", layerMasks["Wall"] | layerMasks["WallDecoration"]);
		layerMasks.Add("Terrain", layerMasks["Floor"] | layerMasks["WallWithDecoration"]);
	}

	private void CopyColliderToPlacementModel(GameObject heldObject) {
		BoxCollider heldObjectCollider = heldObject.GetComponent<BoxCollider>();

		if (heldObjectCollider != null) {
			placementObjectCollider = placementObject.AddComponent<BoxCollider>(heldObjectCollider);
			placementObjectCollider.isTrigger = true;
		} else {
			print($"{gameObject.name} doesn't have a box collider");
		}
	}

	private void CheckForRaycastHit() {
        var cameraToMouseRay = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(cameraToMouseRay, out hitInformation, Mathf.Infinity, layerMasks["Terrain"])) {
			Vector3 pointHit = hitInformation.point;

			if (heldObjectTrapController != null) {
				if (heldObjectTrapController.GetSurfaceType().ToString().ToLower() == hitInformation.collider.gameObject.tag.ToLower()) {
					validPlacement = true;
				} else {
					validPlacement = false;
				}
			} else {
				validPlacement = true;
			}
	
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
