using System.ComponentModel;
using UnityEngine;
using static NotificationController;

public class ResourceController : MonoBehaviour {
	#region Properties

	public int physicalMaterialQuantity;
	public int magicalMaterialQuantity;
	public int valuableQuantity;

	private int physicalMaterialMaximum;
	private int magicalMaterialMaximum;
	private int valuableMaximum;

	private ResourcesUI resourcesUI;

	#endregion

	#region Events

	private void Awake() {
		References.Game.resources = this;

		physicalMaterialMaximum = 100;
		magicalMaterialMaximum = 100;
		valuableMaximum = 100;
	}

	private void Start() {
		resourcesUI = References.UI.resources;
	}

	private void Update() {
		print(magicalMaterialQuantity);
	}

	#endregion

	#region Methods

	#region Get/Set Methods

	public int GetPhysicalMaterialAmount() {
		return physicalMaterialQuantity;
	}

	public int GetMagicalMaterialAmount() {
		return magicalMaterialQuantity;
	}

	public int GetValuableAmount() {
		return valuableQuantity;
	}

	public int GetPhysicalMaterialMaximum() {
		return physicalMaterialMaximum;
	}

	public int GetMagicalMaterialMaximum() {
		return magicalMaterialMaximum;
	}

	public int GetValuableMaximum() {
		return valuableMaximum;
	}

	public void SetPhysicalMaterialAmount(int amount) {
		physicalMaterialQuantity = amount;
	}

	public void SetMagicalMaterialAmount(int amount) {
		magicalMaterialQuantity = amount;
	}

	public void SetValuableAmount(int amount) {
		valuableQuantity = amount;
	}

	public void SetPhysicalMaterialMaximum(int amount) {
		physicalMaterialMaximum = amount;
	}

	public void SetMagicalMaterialMaximum(int amount) {
		magicalMaterialMaximum = amount;
	}

	public void SetValuableMaximum(int amount) {
		valuableMaximum = amount;
	}

	#endregion

	#region Add/Remove Methods

	public int AddResourcesUpToMaximumAmount(ResourceType type, int amountToAdd) {
		int currentAmount = 0, maxAmount = 0;

		switch (type) {
			case ResourceType.PhysicalMaterial:
				currentAmount = physicalMaterialQuantity;
				maxAmount = physicalMaterialMaximum;
				break;
			case ResourceType.MagicalMaterial:
				currentAmount = magicalMaterialQuantity;
				maxAmount = magicalMaterialMaximum;
				break;	
			case ResourceType.Valuable:
				currentAmount = valuableQuantity;
				maxAmount = valuableMaximum;
				break;
		}

		if (currentAmount + amountToAdd >= maxAmount) {
			References.UI.notifications.AddNotification($"You can't store any more {type.GetDescription()}", NotificationType.Info);
			return maxAmount;
		}

		resourcesUI.UpdateResourcesUI();

		return currentAmount += amountToAdd;
	}

	public bool RemoveResourcesIfHaveEnough(ResourceType type, int amountToRemove) {
		int currentAmount = 0;
		int finalAmount;
		bool enoughResources;

		switch (type) {
			case ResourceType.PhysicalMaterial:
				currentAmount = physicalMaterialQuantity;
				break;
			case ResourceType.MagicalMaterial:
				currentAmount = magicalMaterialQuantity;
				break;
			case ResourceType.Valuable:
				currentAmount = valuableQuantity;
				break;
		}

		if (HaveEnoughResources(currentAmount, amountToRemove)) {
			finalAmount = currentAmount -= amountToRemove;
			enoughResources = true;
		} else {
			References.UI.notifications.AddNotification($"You don't have enough {type.GetDescription()}", NotificationType.Error);

			finalAmount = currentAmount;
			enoughResources = false;
		}

		switch (type) {
			case ResourceType.PhysicalMaterial:
				physicalMaterialQuantity = finalAmount;
				break;
			case ResourceType.MagicalMaterial:
				magicalMaterialQuantity = finalAmount;
				break;
			case ResourceType.Valuable:
				valuableQuantity = finalAmount;
				break;
		}

		resourcesUI.UpdateResourcesUI();

		return enoughResources;
	}

	#endregion

	public bool HaveEnoughResources(int currentAmount, int amountToRemove) {
		bool haveEnough;

		if (currentAmount - amountToRemove < 0) {
			haveEnough = false;
		} else {
			haveEnough = true;
		}

		return haveEnough;
	}

	#endregion

	#region Enums

	public enum ResourceType {
		[Description("Physical Materials")]
		PhysicalMaterial,
		[Description("Magical Materials")]
		MagicalMaterial,
		[Description("Valuables")]
		Valuable
	}

	#endregion
}
