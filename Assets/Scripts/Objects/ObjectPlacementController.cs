using System.Collections.Generic;
using UnityEngine;

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

	private new Camera camera;
	private GameObject placementObject;
	private MeshRenderer placementObjectRenderer;
	private new MeshRenderer renderer;

	#endregion

	#region Events

	private void Awake() {
		camera = Camera.main;
		renderer = GetComponent<MeshRenderer>();

		validMaterial = Resources.Load("Materials/Objects/Placement/ValidPlacement") as Material;
		invalidMaterial = Resources.Load("Materials/Objects/Placement/InvalidPlacement") as Material;
		renderer.material = validMaterial;

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

	#endregion

	#region Methods

	public void SetHeldObject(string objectName) {
		if (!placementPrefabs.ContainsKey(objectName)) {
			print($"Placement prefab not defined for {objectName}");
			return;
		}
		
		var placementPrefab = placementPrefabs[objectName];
		placementObject = Instantiate(Resources.Load($"Prefabs/Objects/Placement/{placementPrefab}")) as GameObject;
		placementObject.transform.parent = transform;
		placementObject.transform.localPosition = Vector3.zero;

		if (placementObject.transform.childCount > 1) {
			print($"{objectName} has more than 1 child. It may not display properly.");
		}

		placementObjectRenderer = placementObject.transform.GetComponentInChildren<MeshRenderer>();
	}

	private void SetPlacementPrefabs() {
		placementPrefabs = new Dictionary<string, string> {
			{"Crate", "CratePlacement"}
		};
	}

	private void CheckForRaycastHit() {
        var cameraToMouseRay = camera.ScreenPointToRay(Input.mousePosition);
        validPlacement = true;

        if (Physics.Raycast(cameraToMouseRay, out hitInformation, Mathf.Infinity, terrainMask)) {
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
