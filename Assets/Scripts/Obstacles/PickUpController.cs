using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour
{
    #region Properties

    [HideInInspector]
    public State currentState;
    public bool canBeThrown;

    private Vector3 heldHeight;

    private new Rigidbody rigidbody;

    #endregion

    #region Events

    private void Awake() {
        rigidbody = gameObject.GetComponent<Rigidbody>();

        currentState = State.Idle;
        heldHeight = new Vector3(0, 3, 0);

        if (gameObject.tag == "Trap" || gameObject.tag == "Trigger") {
            canBeThrown = false;
        } else {
            canBeThrown = true;
        }
    }

    private void Update() {
        if (currentState == State.Held) {
            transform.localPosition = heldHeight;
            transform.eulerAngles = Vector3.zero;
            if (rigidbody != null) rigidbody.velocity = Vector3.zero;
        }
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
             if (rigidbody != null) rigidbody.useGravity = false;
            References.currentPlayer.GetComponent<HoldObjectController>().SetHeldObject(gameObject);
        } else {
            if (rigidbody != null) rigidbody.useGravity = true;
        }
    }

    #endregion

    #region Enums

    public enum State {
        Idle,
        Held,
        Thrown
    }

    #endregion
}
