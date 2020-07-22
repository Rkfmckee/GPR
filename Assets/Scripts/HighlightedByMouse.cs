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
    }

    private void Start() {
        allPlayers = References.players;
    }

    private void LateUpdate() {
        if (DontSelect()) return;

        if (currentlyHightlightingMe) {
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
        }

        return dontSelect;
    }

    private bool DontSelectPlayer() {
        // Dont select a player if they are the currently controlled player

        PlayerBehaviour behaviour = gameObject.GetComponent<PlayerBehaviour>();

        bool dontSelect = behaviour.currentlyBeingControlled;

        return dontSelect;
    }

    private bool DontSelectObstacle() {
        // Dont select an obstacle if they are being held

        ObstacleController controller = gameObject.GetComponent<ObstacleController>();

        bool dontSelect = controller.currentState == ObstacleController.State.Held;

        return dontSelect;
    }

    private void playerAllowSwitching() {
        PlayerBehaviour behaviour = gameObject.GetComponent<PlayerBehaviour>();
        
        if (behaviour == null) {
            print(gameObject + "doesn't have a PlayerBehaviour");
            return;
        }

        if (Input.GetButtonDown("Fire1")) {
            foreach(var player in allPlayers) {
                player.GetComponent<PlayerBehaviour>().SetCurrentlyBeingControlled(false);
            }

            behaviour.SetCurrentlyBeingControlled(true);
        }
    }

    private void obstacleCanBePickedUp() {
        ObstacleController controller = gameObject.GetComponent<ObstacleController>();

        if (controller == null) {
            print(gameObject + "doesn't have an ObstacleController");
            return;
        }

        if (Input.GetButtonDown("Fire1")) {
            if (controller.canBePickedUp) {
                controller.SetCurrentState(ObstacleController.State.Held);
            }
        }
    }

    #endregion
}
