﻿using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    #region Properties
    public float timeToOpen;
    public float openAngle;

    private ChestState currentState;
    private Quaternion openedRotation;
    private float currentOpeningTime;
    private GameObject chestLid;

    private GameController gameController;

    #endregion

    #region Events

    private void Awake() {
        currentState = ChestState.Closed;
        openedRotation = Quaternion.Euler(-openAngle, 0, 0);

        chestLid = transform.Find("ChestTop").gameObject;
    }

    private void Start() {
        gameController = References.gameController.GetComponent<GameController>();
    }

    private void Update() {
        if (currentState == ChestState.Open) {
            if (!gameController.IsInventoryOpen()) {
                Close();
            }
        }
    }

    #endregion

    #region Methods

    public ChestState GetCurrentState() {
        return currentState;
    }

    public void Open() {
        currentState = ChestState.Opening;
        StartCoroutine(OpeningChest());
    }

    private void Close() {
        currentState = ChestState.Closing;
        StartCoroutine(ClosingChest());
    }

    #endregion

    #region Enums

    public enum ChestState {
        Open,
        Opening,
        Closed,
        Closing
    }

    #endregion

    #region Coroutines

    private IEnumerator OpeningChest() {
        print("Opening Chest");
        currentOpeningTime = 0;

        while(currentOpeningTime < timeToOpen) {
            chestLid.transform.localRotation = Quaternion.Lerp(Quaternion.Euler(Vector3.zero), openedRotation, currentOpeningTime / timeToOpen);
            currentOpeningTime += Time.deltaTime;

            yield return null;
        }

        currentState = ChestState.Open;
    }

    private IEnumerator ClosingChest() {
        // Use openingTime here, since the closing time will always be the same as opening time
        print("Closing Chest");
        currentOpeningTime = 0;

        while (currentOpeningTime < timeToOpen) {
            chestLid.transform.localRotation = Quaternion.Lerp(openedRotation, Quaternion.Euler(Vector3.zero), currentOpeningTime / timeToOpen);
            currentOpeningTime += Time.deltaTime;

            yield return null;
        }

        currentState = ChestState.Closed;
    }

    #endregion
}
