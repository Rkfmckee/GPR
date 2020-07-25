using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditorInternal;
using UnityEngine;

public class HighlightedByMouse : MonoBehaviour
{
    #region Properties

    public Material hightlightMaterial;
    public bool checkParentForInteractScript;

    private new Renderer renderer;
    private Material startMaterial;
    private bool currentlyHightlightingMe;

    private Dictionary<string, Action> methodsForEachType;
    private List<GameObject> allPlayers;

    #endregion

    #region Events

    private void Awake() {
        renderer = gameObject.GetComponent<Renderer>();
        startMaterial = renderer.material;
        currentlyHightlightingMe = false;

        methodsForEachType = new Dictionary<string, Action>();
        methodsForEachType.Add("Player", playerAllowSwitching);
        methodsForEachType.Add("Obstacle", obstacleCanBePickedUp);
        methodsForEachType.Add("Trap", obstacleCanBePickedUp);
    }

    private void Start() {
        allPlayers = References.players;
    }

    private void LateUpdate() {
        if (currentlyHightlightingMe && Input.GetButtonDown("Fire1")) {
            if (DontSelect()) return;

            methodsForEachType[gameObject.tag]();
        }
    }

    private void OnMouseEnter() {
        if (DontSelect()) return;

        currentlyHightlightingMe = true;
        renderer.material = hightlightMaterial;
    }

    private void OnMouseExit() {
        currentlyHightlightingMe = false;
        renderer.material = startMaterial;
    }

    #endregion

    #region Methods

    private bool DontSelect() {
        bool dontSelect = false;
        
        switch (gameObject.tag) {
            case "Player":
                dontSelect = DontSelectPlayer();
                break;
            case "Obstacle":
                dontSelect = DontSelectObstacle();
                break;
            case "Trap":
                dontSelect = DontSelectTrap();
                break;
        }

        return dontSelect;
    }

    private bool DontSelectPlayer() {
        // Dont select a player if they are the currently controlled player

        PlayerBehaviour behaviour;

        if (checkParentForInteractScript) {
            behaviour = gameObject.transform.parent.GetComponent<PlayerBehaviour>();
        } else {
            behaviour = gameObject.GetComponent<PlayerBehaviour>();
        }

        bool dontSelect = behaviour.currentlyBeingControlled;

        return dontSelect;
    }

    private bool DontSelectObstacle() {
        // Dont select an obstacle if they are being held

        PickUpController pickup;

        if (checkParentForInteractScript) {
            pickup = gameObject.transform.parent.GetComponent<PickUpController>();
        } else {
            pickup = gameObject.GetComponent<PickUpController>();
        }

        bool dontSelect = pickup.currentState == PickUpController.State.Held;

        return dontSelect;
    }

    private bool DontSelectTrap() {
        SpikeTrapController spikeTrap;
        bool dontSelect = false;

        if (checkParentForInteractScript) {
            spikeTrap = gameObject.transform.parent.GetComponent<SpikeTrapController>();
        } else {
            spikeTrap = gameObject.GetComponent<SpikeTrapController>();
        }

        if (spikeTrap != null) {
            // If the type of trap we're picking up is a spike trap
            if (spikeTrap.currentState != SpikeTrapController.SpikeState.SpikesDown) {
                dontSelect = true;
            }
        }

        return dontSelect || DontSelectObstacle();
    }

    private void playerAllowSwitching() {
        PlayerBehaviour behaviour;

        if (checkParentForInteractScript) {
            behaviour = gameObject.transform.parent.GetComponent<PlayerBehaviour>();
        } else {
            behaviour = gameObject.GetComponent<PlayerBehaviour>();
        }

        if (behaviour == null) {
            print(gameObject + "doesn't have a PlayerBehaviour");
            return;
        }

        foreach(var player in allPlayers) {
            player.GetComponent<PlayerBehaviour>().SetCurrentlyBeingControlled(false);
        }

        behaviour.SetCurrentlyBeingControlled(true);
    }

    private void obstacleCanBePickedUp() {
        PickUpController pickup;

        if (checkParentForInteractScript) {
            pickup = gameObject.transform.parent.GetComponent<PickUpController>();
        } else {
            pickup = gameObject.GetComponent<PickUpController>();
        }

        if (pickup == null) {
            print(gameObject + "can't be picked up");
            return;
        }

        pickup.SetCurrentState(PickUpController.State.Held);
    }

    #endregion
}
