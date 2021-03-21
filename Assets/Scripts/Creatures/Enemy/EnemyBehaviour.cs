using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    #region Properties

    public float movementSpeed;

    private EnemyState currentState;
    private NavMeshAgent navMeshAgent;

    #endregion

    #region Events

    private void Awake() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        SetCurrentState(new EnemyStateLookAround(gameObject, 3));
    }
    
    private void Update() {
        currentState.StateUpdate();
    }

    private void FixedUpdate() {
        currentState.StateFixedUpdate();
    }

    private void OnDestroy() {
        if (References.Enemy.enemies.Contains(gameObject)) {
            References.Enemy.enemies.Remove(gameObject);
        }

        if (References.Enemy.enemies.Count <= 0) {
            if (References.GameController.roundStage != null) {
                References.GameController.roundStage.SetCurrentStage(new PreparingStage());
            }
        }
    }

    #endregion

    #region Methods

    public EnemyState GetCurrentState() {
        return currentState;
    }

    public void SetCurrentState(EnemyState state) {
        currentState = state;
    }

    #endregion
}
