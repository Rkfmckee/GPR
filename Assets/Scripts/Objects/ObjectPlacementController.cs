using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacementController : MonoBehaviour {
	#region Properties

	public bool validPlacement;
	public RaycastHit hitInformation;

	private GameObject placementObject;
	private MeshRenderer placementObjectRenderer;
	private Material validMaterial;
	private Material invalidMaterial;
	private Dictionary<string, string> placementPrefabs;
	private new MeshRenderer renderer;
	private MeshFilter meshFilter;
	private Mesh heldObjectMesh;

	#endregion

	#region Events

	private void Awake() {
		renderer = GetComponent<MeshRenderer>();
		meshFilter = GetComponent<MeshFilter>();

		validPlacement = true;

		validMaterial = Resources.Load("Materials/Objects/Placement/ValidPlacement") as Material;
		invalidMaterial = Resources.Load("Materials/Objects/Placement/InvalidPlacement") as Material;

		renderer.material = validMaterial;

		SetPlacementPrefabs();
	}

	private void Update() {
		Ray cameraToMouse = Camera.main.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(cameraToMouse, out hitInformation, Mathf.Infinity)) {
			transform.position = hitInformation.point;
		}

		if (placementObjectRenderer != null) {
			if (validPlacement) {
				placementObjectRenderer.material = validMaterial;
			} else {
				placementObjectRenderer.material = invalidMaterial;
			}
		}
			
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

	#endregion
}
