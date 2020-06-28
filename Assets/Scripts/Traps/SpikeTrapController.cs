using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrapController : MonoBehaviour
{
    public SpikeState currentState;
    public float extendingTime;
    public float retractingTime;
    public float timeToStayExtended;

    private float currentTimeExtending;
    private float currentTimeRetracting;
    private float currentTimeExtended;

    private Vector3 spikesUpPosition;
    private Vector3 spikesDownPosition;

    private Vector3 velocity;

    private Transform spikeChild;

    #region Events

    private void Awake() {
        setupInstanceVariables();

        spikeChild.position = spikesDownPosition;
    }

    private void Update() {
        if (currentState == SpikeState.EXTENDING) {
            currentTimeExtending += Time.deltaTime;
            
            MoveSpikes();
        } else if (currentState == SpikeState.SPIKESUP) {
            currentTimeExtended += Time.deltaTime;

            if (currentTimeExtended >= timeToStayExtended) {
                currentState = SpikeState.RETRACTING;
                currentTimeExtended = 0;
            }
        } else if (currentState == SpikeState.RETRACTING) {
            currentTimeRetracting += Time.deltaTime;

            MoveSpikes();
        }

        print(currentState);
    }

    public void CollisionDetected(Collision collision) {
        if (currentState == SpikeState.SPIKESDOWN) {
            currentState = SpikeState.EXTENDING;
        }
    }

    #endregion

    #region Methods

    private void setupInstanceVariables() {
        spikeChild = transform.GetChild(0);
        spikesUpPosition = new Vector3(transform.position.x, 0, transform.position.z);
        spikesDownPosition = new Vector3(transform.position.x, -0.5f, transform.position.z);
        velocity = Vector3.one;

        currentTimeExtending = 0;
        currentTimeRetracting = 0;
        currentTimeExtended = 0;

        currentState = SpikeState.SPIKESDOWN;
    }

    private void MoveSpikes() {
        if (currentState == SpikeState.EXTENDING) {
            spikeChild.position = Vector3.Lerp(spikesDownPosition, spikesUpPosition, currentTimeExtending / extendingTime);

            if (spikeChild.position == spikesUpPosition) {
                currentState = SpikeState.SPIKESUP;
                currentTimeExtending = 0;
            }
        } else if (currentState == SpikeState.RETRACTING) {
            spikeChild.position = Vector3.Lerp(spikesUpPosition, spikesDownPosition, currentTimeRetracting / retractingTime);

            if (spikeChild.position == spikesDownPosition) {
                currentState = SpikeState.SPIKESDOWN;
                currentTimeRetracting = 0;
            }
        }
    }

    #endregion

    #region Enums

    public enum SpikeState {
        SPIKESDOWN,
        SPIKESUP,
        EXTENDING,
        RETRACTING
    }

    #endregion
}
