using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrapController : TrapController
{
    [HideInInspector]
    public SpikeState currentState;
    public float extendingTime;
    public float retractingTime;
    public float timeToStayExtended;

    private float currentTimeExtending;
    private float currentTimeRetracting;
    private float currentTimeExtended;

    private Vector3 spikesUpPosition;
    private Vector3 spikesDownPosition;

    private Transform spikeChild;
    private GameObject spikeChildPrefab;

    #region Events

    private void Awake() {
        setupInstanceVariables();
    }

    private void Update() {
        if (currentState == SpikeState.Extending) {
            currentTimeExtending += Time.deltaTime;

            MoveSpikes();
        } else if (currentState == SpikeState.SpikesUp) {
            currentTimeExtended += Time.deltaTime;

            if (currentTimeExtended >= timeToStayExtended) {
                currentState = SpikeState.Retracting;
                currentTimeExtended = 0;
            }
        } else if (currentState == SpikeState.Retracting) {
            currentTimeRetracting += Time.deltaTime;

            MoveSpikes();
        } else if (currentState == SpikeState.SpikesDown) {
            if (spikeChild != null) {
                Destroy(spikeChild.gameObject);
            }
        }
    }

    #endregion

    #region Methods
    public override void TriggerTrap(Collider triggeredBy) {
        if (currentState == SpikeState.SpikesDown) {
            spikeChild = Instantiate(spikeChildPrefab, transform).transform;
            spikeChild.localPosition = spikesDownPosition;

            currentState = SpikeState.Extending;
        }
    }

    private void setupInstanceVariables() {
        trapType = Type.Floor;
        maxHealth = 20;
        trapHealth = maxHealth;

        spikesUpPosition = new Vector3(0, 0, 0);
        spikesDownPosition = new Vector3(0, -0.5f, 0);

        spikeChildPrefab = Resources.Load("Prefabs/Traps/Spikes") as GameObject;

        currentTimeExtending = 0;
        currentTimeRetracting = 0;
        currentTimeExtended = 0;

        currentState = SpikeState.SpikesDown;
    }

    private void MoveSpikes() {
        if (currentState == SpikeState.Extending) {
            spikeChild.localPosition = Vector3.Lerp(spikesDownPosition, spikesUpPosition, currentTimeExtending / extendingTime);

            if (spikeChild.localPosition == spikesUpPosition) {
                currentState = SpikeState.SpikesUp;
                currentTimeExtending = 0;
            }
        } else if (currentState == SpikeState.Retracting) {
            spikeChild.localPosition = Vector3.Lerp(spikesUpPosition, spikesDownPosition, currentTimeRetracting / retractingTime);

            if (spikeChild.localPosition == spikesDownPosition) {
                currentState = SpikeState.SpikesDown;
                currentTimeRetracting = 0;
            }
        }
    }

    #endregion

    #region Enums

    public enum SpikeState {
        SpikesDown,
        SpikesUp,
        Extending,
        Retracting
    }

    #endregion
}
