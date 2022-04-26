using System.Collections;
using UnityEngine;
using static TrapTriggerBase;

public class CraftingStation : MonoBehaviour {
	#region Properties

	private GameObject progressBarPrefab;
	private bool isCurrentlyCrafting;

	private new Camera camera;
	private Vector3 craftingAreaMidPoint;
	private int creaturesAndObstaclesMask;

	#endregion

	#region Events

	private void Awake() {
		progressBarPrefab = Resources.Load<GameObject>("Prefabs/UI/ProgressBar");
		craftingAreaMidPoint = transform.Find("Area").position;

		var layerMasks = GeneralHelper.GetLayerMasks();
		creaturesAndObstaclesMask = layerMasks["Creature"] | layerMasks["Obstacle"];
	}

	private void Start() {
		camera = References.Camera.camera;
	}

	#endregion

	#region Methods

		#region Get/Set

		public bool IsCurrentlyCrafting() {
			return isCurrentlyCrafting;
		}

		#endregion

	public bool CheckCraftingAreaIsClear() {
		var objectsInCraftArea = Physics.OverlapSphere(craftingAreaMidPoint, 2, creaturesAndObstaclesMask);
		return objectsInCraftArea.Length == 0;
	}
	
	public ProgressBar CreateProgressBar(float timeInSeconds) {
		var parent = References.UI.canvas.transform;

		var progressBarObject = Instantiate(progressBarPrefab, parent);
		var progressBar = progressBarObject.GetComponent<ProgressBar>();
		progressBar.SetProgressBar("Crafting", timeInSeconds, transform);

		return progressBar;
	}

	public void CraftItem(GameObject itemToCraft) {
		StartCoroutine(CraftingItem(itemToCraft));
	}

	public IEnumerator CraftingItem(GameObject itemToCraft) {
		isCurrentlyCrafting = true;

		References.Game.globalObstacles.ShouldShowCraftingMenu(false);
		var craftingItemController = itemToCraft.GetComponent<CraftingItem>();
		var trapTriggerBase = itemToCraft.GetComponent<TrapTriggerBase>();
		var spawnPosition = transform.Find("Area").position;

		var progressBar = CreateProgressBar(craftingItemController.resourceCost);
		
		while(!progressBar.IsProgressFinished()) {
			yield return null;
		}
		
		GameObject newItem = Instantiate(itemToCraft);
		newItem.name = itemToCraft.name;
		newItem.transform.position = spawnPosition;

		if (trapTriggerBase.GetSurfaceType() == SurfaceType.Wall) {
			newItem.transform.rotation = Quaternion.Euler(90, 0, 0);
			newItem.transform.position += Vector3.up * 0.1f;
		}

		AddNotificationOfCraftedItem(trapTriggerBase.GetName());
		
		isCurrentlyCrafting = false;
	}

	private void AddNotificationOfCraftedItem(string itemName) {
		var determiner = GeneralHelper.GetDeterminer(itemName);	
		References.UI.notifications.AddNotification($"Crafted {determiner} {itemName}");
	}

	#endregion
}
