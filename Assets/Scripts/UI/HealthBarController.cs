using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    #region Properties

    private Image healthBarAmount;

    #endregion

    #region Events

    private void Awake() {
        healthBarAmount = transform.GetChild(1).GetComponent<Image>();
    }

    #endregion

    #region Methods

    public void ShowHealthFraction(float fraction) {
        healthBarAmount.transform.localScale = new Vector3(fraction, 1, 1);
    }

    #endregion
}
