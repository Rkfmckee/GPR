using UnityEngine;
using static ResourceController;

public class CraftingItem : MonoBehaviour {
	#region Properties

	public Sprite inventoryIcon;
	public int resourceCost;
	public ResourceType resourceType;

	#endregion

	#region Methods

		#region Get/Set

		public ResourceType GetResourceType() {
			return resourceType;
		}

		#endregion

	public bool RemoveResourcesToSpawnItem() {
		ResourceController resources = References.Game.resources;

		return resources.RemoveResourcesIfHaveEnough(resourceType, resourceCost);
	}

	#endregion
}
