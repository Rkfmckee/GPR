using UnityEngine;

public class CraftingItem : MonoBehaviour {
	#region Properties

	public Sprite inventoryIcon;
	public int resourceCost;
	public ResourceController.ResourceType resourceType;

	#endregion

	#region Methods

	public bool RemoveResourcesToSpawnItem() {
		ResourceController resources = References.GameController.resources;

		return resources.RemoveResourcesIfHaveEnough(resourceType, resourceCost);
	}

	#endregion
}
