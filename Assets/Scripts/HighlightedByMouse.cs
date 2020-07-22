using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightedByMouse : MonoBehaviour
{
    #region Properties

    private new Renderer renderer;
    private Shader startShader;
    private Shader hightlightShader;
    private bool currentlyHightlightingMe;

    private Dictionary<string, Action> methodsForEachType;
    private List<GameObject> allPlayers;

    #endregion

    #region Events

    private void Awake() {
        renderer = gameObject.GetComponent<Renderer>();
        startShader = renderer.material.shader;
        hightlightShader = Shader.Find("Outline");
        currentlyHightlightingMe = false;

        methodsForEachType = new Dictionary<string, Action>();
        methodsForEachType.Add("Player", playerAllowSwitching);
        methodsForEachType.Add("Obstacle", obstacleCanBePickedUp);
    }

    private void Start() {
        allPlayers = References.players;
    }

    private void LateUpdate() {
        if (currentlyHightlightingMe) {
            methodsForEachType[gameObject.tag]();
        }
    }

    private void OnMouseEnter() {
        currentlyHightlightingMe = true;
        renderer.material.shader = hightlightShader;
    }

    private void OnMouseExit() {
        currentlyHightlightingMe = false;
        renderer.material.shader = startShader;
    }

    #endregion

    #region Methods

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
