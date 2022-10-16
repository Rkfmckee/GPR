using System.ComponentModel;
using UnityEngine;
using static NotificationController;

public class ResourceController : MonoBehaviour
{
	#region Fields

	[SerializeField]
	private int physicalMaterialQuantity;
	[SerializeField]
	private int magicalMaterialQuantity;
	[SerializeField]
	private int valuableQuantity;

	private int physicalMaterialMaximum;
	private int magicalMaterialMaximum;
	private int valuableMaximum;

	private ResourcesUI resourcesUI;

	#endregion

	#region Properties

	public int PhysicalMaterialQuantity { get => physicalMaterialQuantity; set => physicalMaterialQuantity = value; }

	public int MagicalMaterialQuantity { get => magicalMaterialQuantity; set => magicalMaterialQuantity = value; }

	public int ValuableQuantity { get => valuableQuantity; set => valuableQuantity = value; }

	public int PhysicalMaterialMaximum { get => physicalMaterialMaximum; set => physicalMaterialMaximum = value; }

	public int MagicalMaterialMaximum { get => magicalMaterialMaximum; set => magicalMaterialMaximum = value; }

	public int ValuableMaximum { get => valuableMaximum; set => valuableMaximum = value; }

	#endregion

	#region Events

	private void Awake()
	{
		References.Game.resources = this;

		physicalMaterialMaximum = 100;
		magicalMaterialMaximum  = 100;
		valuableMaximum         = 100;
	}

	private void Start()
	{
		resourcesUI = References.UI.resources;
	}

	#endregion

	#region Methods

	#region Add/Remove Methods

	public int AddResourcesUpToMaximumAmount(ResourceType type, int amountToAdd)
	{
		int currentAmount = 0, maxAmount = 0;

		switch (type)
		{
			case ResourceType.PhysicalMaterials:
				currentAmount = physicalMaterialQuantity;
				maxAmount     = physicalMaterialMaximum;
				break;
			case ResourceType.MagicalMaterials:
				currentAmount = magicalMaterialQuantity;
				maxAmount     = magicalMaterialMaximum;
				break;
			case ResourceType.Valuables:
				currentAmount = valuableQuantity;
				maxAmount     = valuableMaximum;
				break;
		}

		if (currentAmount + amountToAdd >= maxAmount)
		{
			References.UI.notifications.AddNotification($"You can't store any more {type.GetDescription()}", NotificationType.Info);
			return maxAmount;
		}

		resourcesUI.UpdateResourcesUI();

		return currentAmount += amountToAdd;
	}

	public bool RemoveResourcesIfHaveEnough(ResourceType type, int amountToRemove)
	{
		int currentAmount = 0;
		int finalAmount;
		bool enoughResources;

		switch (type)
		{
			case ResourceType.PhysicalMaterials:
				currentAmount = physicalMaterialQuantity;
				break;
			case ResourceType.MagicalMaterials:
				currentAmount = magicalMaterialQuantity;
				break;
			case ResourceType.Valuables:
				currentAmount = valuableQuantity;
				break;
		}

		if (HaveEnoughResources(currentAmount, amountToRemove))
		{
			finalAmount     = currentAmount -= amountToRemove;
			enoughResources = true;
		}
		else
		{
			References.UI.notifications.AddNotification($"You don't have enough {type.GetDescription()}", NotificationType.Error);

			finalAmount     = currentAmount;
			enoughResources = false;
		}

		switch (type)
		{
			case ResourceType.PhysicalMaterials:
				physicalMaterialQuantity = finalAmount;
				break;
			case ResourceType.MagicalMaterials:
				magicalMaterialQuantity = finalAmount;
				break;
			case ResourceType.Valuables:
				valuableQuantity = finalAmount;
				break;
		}

		resourcesUI.UpdateResourcesUI();

		return enoughResources;
	}

	#endregion

	public bool HaveEnoughResources(int currentAmount, int amountToRemove)
	{
		return currentAmount - amountToRemove >= 0;
	}

	#endregion

	#region Enums

	public enum ResourceType
	{
		[Description("Physical Materials")]
		PhysicalMaterials,
		[Description("Magical Materials")]
		MagicalMaterials,
		[Description("Valuables")]
		Valuables
	}

	#endregion
}
