using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    #region Properties

    public bool canBePickedUp;
    [HideInInspector]
    public State currentState;

    private Vector3 verticalPositionOffset;

    private new Rigidbody rigidbody;

    #endregion

    #region Events

    private void Awake() {
        rigidbody = gameObject.GetComponent<Rigidbody>();

        canBePickedUp = true;
        currentState = State.Idle;
        verticalPositionOffset = new Vector3(0, 3, 0);
    }

    private void Update() {
        if (currentState == State.Held) {
            transform.localPosition = verticalPositionOffset;
            transform.eulerAngles = Vector3.zero;
            rigidbody.velocity = Vector3.zero;
        }

        print(rigidbody.velocity);
        print(currentState);
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.collider.gameObject.tag == "Floor") {
            currentState = State.Idle;
        }
    }

    #endregion

    #region Methods

    public void SetCurrentState(State state) {
        currentState = state;

        if (currentState == State.Held) {
            rigidbody.useGravity = false;
            References.currentPlayer.GetComponent<PlayerBehaviour>().SetHeldObject(gameObject);
        } else {
            rigidbody.useGravity = true;
        }
    }

    #endregion

    #region Enums

    public enum State {
        Idle,
        Held,
        THROWN
    }

    #endregion
}
