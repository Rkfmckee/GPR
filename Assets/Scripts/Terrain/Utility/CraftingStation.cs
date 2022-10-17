using System.Collections;
using UnityEngine;
using static NotificationController;
using static ObstacleController;

public class CraftingStation : MonoBehaviour {
	#region Properties

	private GameObject progressBarPrefab;
	private bool isCurrentlyCrafting;

	private new Camera camera;
	private Vector3 craftingAreaMidPoint;
	private int creaturesAndObstaclesMask;
	private float ceilingHeight;

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
		ceilingHeight = References.Game.globalObstacles.MaxObstacleHeight;
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
		var spawnPosition = transform.Find("Area").position;
		var spawnRotation = Quaternion.Euler(craftingItemController.spawnRotation);
		var itemName = itemToCraft.name;

		var progressBar = CreateProgressBar(craftingItemController.resourceCost);
		
		while(!progressBar.IsProgressFinished()) {
			yield return null;
		}

		var newItem = Instantiate(itemToCraft, References.Obstacles.parentGroup);
		var newItemPickup = newItem.GetComponent<PickUpObject>();
		var newItemObstacleController = newItem.GetComponent<ObstacleController>();

		var newItemHeld = Instantiate(newItemPickup.HeldPrefab, References.Obstacles.parentGroup);
		newItemHeld.GetComponent<ObstacleHeld>().obstacle = newItem;

		if (newItemObstacleController != null) {
			itemName = newItemObstacleController.Name;
			newItemObstacleController.ObstacleDisabled = true;

			if (itemToCraft.GetComponent<Rigidbody>()) {
				// If it is a physics object, it's pivot point is likely in the center of it's collider
				// So adjust it's position to account for that
				spawnPosition += MoveColliderVertically(newItem);
			}
			
		} else {
			spawnPosition += MoveColliderVertically(newItem);
		}
		
		newItem.name = itemToCraft.name;
		newItem.transform.position = spawnPosition;
		newItem.transform.rotation = spawnRotation;
		newItemPickup.DisableComponents();

		newItemHeld.transform.position = spawnPosition;
		newItemHeld.transform.rotation = spawnRotation;

		AddNotificationOfCraftedItem(itemName);
		isCurrentlyCrafting = false;
	}

	private Vector3 MoveColliderVertically(GameObject newItem) {
		var collider = newItem.GetComponent<Collider>();
		return Vector3.up * collider.bounds.extents.y;
	}

	private void AddNotificationOfCraftedItem(string itemName) {
		var determiner = GeneralHelper.GetDeterminer(itemName);	
		References.UI.notifications.AddNotification($"Crafted {determiner} {itemName}", NotificationType.Success);
	}

	#endregion
}
