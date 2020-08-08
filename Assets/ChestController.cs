using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    #region Properties

    private ChestState currentState;

    #endregion

    #region Events

    private void Awake() {
        currentState = ChestState.Closed;
    }

    #endregion

    #region Methods

    public ChestState GetCurrentState() {
        return currentState;
    }

    public void SetState(ChestState state) {
        currentState = state;
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
}
