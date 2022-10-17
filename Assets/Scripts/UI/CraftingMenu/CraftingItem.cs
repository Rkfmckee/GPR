using UnityEngine;
using static ResourceController;

public class CraftingItem : MonoBehaviour
{
	#region Fields

	public Sprite inventoryIcon;
	public int resourceCost;
	public ResourceType resourceType;
	public Vector3 spawnRotation;

	#endregion

	#region Properties

	public ResourceType ResourceType { get => resourceType; }

	#endregion

	#region Methods

	public bool RemoveResourcesToSpawnItem()
	{
		ResourceController resources = References.Game.resources;

		return resources.RemoveResourcesIfHaveEnough(resourceType, resourceCost);
	}

	#endregion
}
