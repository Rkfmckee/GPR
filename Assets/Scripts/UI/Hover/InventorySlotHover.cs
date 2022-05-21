using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlotHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
	#region Properties

	private InventorySlot inventorySlot;

	#endregion
	
	#region Events
	
	private void Awake() {
		inventorySlot = GetComponent<InventorySlot>();
	}

	public void OnPointerEnter(PointerEventData eventData) {
		if (inventorySlot == null) {
			return;
		}

		References.UI.craftingMenu.EnableObstacleDetails(inventorySlot.itemInSlot.GetComponent<ObstacleController>());
	}

	public void OnPointerExit(PointerEventData eventData) {
		References.UI.craftingMenu.DisableObstacleDetails();
	}

	#endregion
}
