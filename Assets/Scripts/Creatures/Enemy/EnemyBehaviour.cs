using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    #region Properties

    public float movementSpeed;

    private new Rigidbody rigidbody;
    private FieldOfView fieldOfView;

    private List<GameObject> allPlayers;
    private GameObject targetPlayer;
    private Vector3 movementDirection;
    private EnemyState currentState;

    #endregion

    #region Events

    private void Awake() {
        rigidbody = GetComponent<Rigidbody>();
        fieldOfView = GetComponent<FieldOfView>();

        movementDirection = transform.forward;
        SetCurrentState(new EnemyStateChase(gameObject));

    }

    private void Start() {
        allPlayers = References.Player.players;
    }

    private void Update() {
        currentState.StateUpdate();
    }

    private void FixedUpdate() {
        currentState.StateFixedUpdate();
    }

    private void OnDestroy() {
        if (References.enemies.Contains(gameObject)) {
            References.enemies.Remove(gameObject);
        }

        if (References.enemies.Count <= 0) {
            if (References.GameController.roundStage != null) {
                References.GameController.roundStage.SetCurrentStage(new PreparingStage());
            }
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "WallDecoration") {
            currentState.ChangeDirectionAfterHittingWall(collision);
        }
    }

    #endregion

    #region Methods

    private Vector3 WalkForward() {
        var directionToTarget = transform.forward;
        return directionToTarget;
    }

    public EnemyState GetCurrentState() {
        return currentState;
    }

    public void SetCurrentState(EnemyState state) {
        currentState = state;
    }

    #endregion
}
