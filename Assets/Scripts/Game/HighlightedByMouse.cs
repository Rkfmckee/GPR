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

    private Outline outline;
    private Dictionary<string, Action> methodsForEachType;
    private GameTrapsController gameTraps;

    #endregion

    #region Events

    private void Awake() { 
        outline = gameObject.AddComponent<Outline>();
        outline.OutlineMode = Outline.Mode.OutlineAndSilhouette;
        outline.OutlineColor = Color.yellow;
        outline.OutlineWidth = 5f;
        outline.enabled = false;

        currentlyHightlightingMe = false;

        methodsForEachType = new Dictionary<string, Action>();
        methodsForEachType.Add("Player", playerAllowSwitching);
        methodsForEachType.Add("Obstacle", obstacleCanBePickedUp);
        methodsForEachType.Add("Trap", obstacleCanBePickedUp);
        methodsForEachType.Add("Trigger", obstacleCanBePickedUp);
        methodsForEachType.Add("Chest", chestCanBeOpened);
    }

    private void Start() {
        gameTraps = References.GameController.gameTraps;
    }

    private void Update() {
        if (currentlyHightlightingMe) {
            if (!outline.enabled) {
                outline.enabled = true;
            }

            if (!gameTraps.IsTrapLinkingLineActive()) {
                if (Input.GetButtonDown("Fire1")) {
                    if (DontSelect()) return;
                    methodsForEachType[gameObject.tag]();
                }

                if (Input.GetButtonDown("Fire2")) {
                    if (DontSelect()) return;

                    if (tag == "Trap" || tag == "Trigger") {
                        gameTraps.CreateTrapLinkingLine(transform);
                    } else {
                        print($"Objects with tag {tag} can't be linked");
                    }
                }
            }
        } else {
            if (outline.enabled) {
                outline.enabled = false;
            }
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
        ChestController chest = GetComponentInParent<ChestController>();

        if (chest.GetCurrentState() == ChestController.ChestState.Closed) {
            chest.Open();

            gameTraps.ShouldShowCaveInventory(true);
        }
    }

    #endregion
}
