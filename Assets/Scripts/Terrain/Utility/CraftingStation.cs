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
		var obstacleController = itemToCraft.GetComponent<ObstacleController>();
		var spawnPosition = transform.Find("Area").position;
		var spawnRotation = Quaternion.Euler(Vector3.zero);
		var itemName = itemToCraft.name;

		var progressBar = CreateProgressBar(craftingItemController.resourceCost);
		
		while(!progressBar.IsProgressFinished()) {
			yield return null;
		}

		GameObject newItem = Instantiate(itemToCraft, References.Obstacles.parentGroup);

		if (obstacleController != null) {
			itemName = obstacleController.GetName();

			if (obstacleController.GetSurfaceType() == SurfaceType.Wall) {
				spawnPosition += Vector3.up * 0.1f;
				spawnRotation = Quaternion.Euler(90, 0, 0);
			} else {
				if (itemToCraft.GetComponent<Rigidbody>()) {
					// If it is a physics object, it's pivot point is likely in the center of it's collider
					// So adjust it's position to account for that
					spawnPosition += MoveColliderVertically(newItem);
				}
			}
		} else {
			spawnPosition += MoveColliderVertically(newItem);
		}
		
		newItem.name = itemToCraft.name;
		newItem.transform.position = spawnPosition;
		newItem.transform.rotation = spawnRotation;

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
