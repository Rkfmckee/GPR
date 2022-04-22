using UnityEngine;

public class ResourceController : MonoBehaviour {
	#region Properties

	public int physicalMaterialQuantity;
	public int valuableQuantity;

	private int physicalMaterialMaximum;
	private int valuableMaximum;

	#endregion

	#region Events

	private void Awake() {
		References.Game.resources = this;

		physicalMaterialMaximum = 100;
		valuableMaximum = 100;
	}

	#endregion

	#region Methods

	#region Get/Set Methods

	public int GetPhysicalMaterialAmount() {
		return physicalMaterialQuantity;
	}

	public int GetValuableAmount() {
		return valuableQuantity;
	}

	public int GetPhysicalMaterialMaximum() {
		return physicalMaterialMaximum;
	}

	public int GetValuableMaximum() {
		return valuableMaximum;
	}

	public void SetPhysicalMaterialAmount(int amount) {
		physicalMaterialQuantity = amount;
	}

	public void SetValuableAmount(int amount) {
		valuableQuantity = amount;
	}

	public void SetPhysicalMaterialMaximum(int amount) {
		physicalMaterialQuantity = amount;
	}

	public void SetValuableMaximum(int amount) {
		valuableQuantity = amount;
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
			case ResourceType.Valuable:
				currentAmount = valuableQuantity;
				maxAmount = valuableMaximum;
				break;
		}

		if (currentAmount + amountToAdd >= maxAmount) {
			print($"You have MAX {type}");
			return maxAmount;
		}

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
			case ResourceType.Valuable:
				currentAmount = valuableQuantity;
				break;
		}

		if (HaveEnoughResources(currentAmount, amountToRemove)) {
			finalAmount = currentAmount -= amountToRemove;
			enoughResources = true;
		} else {
			print($"You don't have enough {type}");
			finalAmount = currentAmount;
			enoughResources = false;
		}

		switch (type) {
			case ResourceType.PhysicalMaterial:
				physicalMaterialQuantity = finalAmount;
				break;
			case ResourceType.Valuable:
				valuableQuantity = finalAmount;
				break;
		}

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
		PhysicalMaterial,
		Valuable
	}

	#endregion
}
