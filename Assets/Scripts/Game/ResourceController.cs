using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class ResourceController : MonoBehaviour
{
    #region Properties

    public int physicalMaterialQuantity;
    public int valuableQuantity;

    private int physicalMaterialMaximum;
    private int valuableMaximum;

    #endregion

    #region Events

    private void Awake() {
        References.GameController.resources = this;

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

    public void AddPhysicalMaterials(int amount) {
        physicalMaterialQuantity = AddUpToMaximumAmount(ResourceType.PhysicalMaterial, amount);
    }

    private int AddUpToMaximumAmount(ResourceType type, int amountToAdd) {
        int currentAmount = 0, maxAmount = 0;
        
        switch(type) {
            case ResourceType.PhysicalMaterial :
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

    #endregion

    #endregion

    #region Enums

    public enum ResourceType {
        PhysicalMaterial,
        Valuable
    }

    #endregion
}
