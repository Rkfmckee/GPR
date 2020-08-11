using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveInventoryController : MonoBehaviour
{
    #region Properties

    private GameObject itemSelected;

    #endregion

    #region Methods

    public GameObject GetSelectedItem() {
        return itemSelected;
    }

    public void SetSelectedItem(GameObject item) {
        itemSelected = item;
    }

    #endregion
}
