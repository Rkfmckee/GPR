using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditorInternal;
using UnityEngine;

public class HighlightedByMouse : MonoBehaviour
{
    #region Properties

    [HideInInspector]
    public bool currentlyHightlightingMe;
    public float maxDistanceFromPlayer;

    private new Renderer renderer;
    private Shader startShader;
    private Shader highlightShader;

    private Dictionary<string, Action> methodsForEachType;

    #endregion

    #region Events

    private void Awake() {
        renderer = gameObject.GetComponent<Renderer>();
        currentlyHightlightingMe = false;

        startShader = renderer.material.shader;
        highlightShader = Shader.Find("Outlined/Custom");

        methodsForEachType = new Dictionary<string, Action>();
        methodsForEachType.Add("Player", playerAllowSwitching);
        methodsForEachType.Add("Obstacle", obstacleCanBePickedUp);
        methodsForEachType.Add("Trap", obstacleCanBePickedUp);
        methodsForEachType.Add("Trigger", obstacleCanBePickedUp);
        methodsForEachType.Add("Chest", chestCanBeOpened);
    }

    private void Update() {
        if (currentlyHightlightingMe) {
            if (renderer.material.shader == startShader) {
                renderer.material.shader = highlightShader;
            }

            if (Input.GetButtonDown("Fire1")) {
                if (DontSelect()) return;
                methodsForEachType[gameObject.tag]();
            }
        } else {
            renderer.material.shader = startShader;
        }
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
            case "Trigger":
                dontSelect = DontSelectObstacle();
                break;
            case "Chest":
                dontSelect = DontSelectChest();
                break;
        }

        return dontSelect;
    }

    private bool DontSelectPlayer() {
        // Dont select a player if they are the currently controlled player

        PlayerBehaviour behaviour = gameObject.GetComponentInParent<PlayerBehaviour>();

        bool dontSelect = behaviour.currentlyBeingControlled;

        return dontSelect;
    }

    private bool DontSelectObstacle() {
        // Dont select an obstacle if they are being held

        PickUpController pickup = gameObject.GetComponentInParent<PickUpController>();

        bool dontSelect = pickup.currentState == PickUpController.State.Held;

        return dontSelect;
    }

    private bool DontSelectTrap() {
        bool dontSelect = false;
        SpikeTrapController spikeTrap = gameObject.GetComponentInParent<SpikeTrapController>();

        if (spikeTrap != null) {
            // If the type of trap we're picking up is a spike trap
            if (spikeTrap.currentState != SpikeTrapController.SpikeState.SpikesDown) {
                dontSelect = true;
            }
        }

        return dontSelect || DontSelectObstacle();
    }

    private bool DontSelectChest() {
        bool dontSelect = false;
        ChestController controller = GetComponentInParent<ChestController>();

        if (controller.GetCurrentState() == ChestController.ChestState.Open) {
            dontSelect = true;
        }

        return dontSelect;
    }

    private void playerAllowSwitching() {
        PlayerBehaviour behaviour = gameObject.GetComponentInParent<PlayerBehaviour>();

        if (behaviour == null) {
            print(gameObject + "doesn't have a PlayerBehaviour");
            return;
        }

        foreach(var player in References.players) {
            player.GetComponent<PlayerBehaviour>().SetCurrentlyBeingControlled(false);
        }

        behaviour.SetCurrentlyBeingControlled(true);
    }

    private void obstacleCanBePickedUp() {
        PickUpController pickup = gameObject.GetComponentInParent<PickUpController>();

        if (pickup == null) {
            print(gameObject + "can't be picked up");
            return;
        }

        pickup.SetCurrentState(PickUpController.State.Held);
    }

    private void chestCanBeOpened() {
        ChestController controller = GetComponentInParent<ChestController>();

        if (controller.GetCurrentState() == ChestController.ChestState.Closed) {
            controller.SetState(ChestController.ChestState.Open);

            References.gameController.GetComponent<GameController>().ShouldShowCaveInventory(true);
        }
    }

    #endregion
}
