using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyState
{
    #region Properties

    protected Vector3 movementDirection;
    protected float movementSpeed;

    protected GameObject enemyObject;
    protected Transform transform;
    protected Rigidbody rigidbody;
    protected EnemyBehaviour behaviour;
    protected FieldOfView fieldOfView;
    protected NavMeshAgent navMeshAgent;

    #endregion

    #region Methods

    public EnemyState(GameObject enemyObj) {
        enemyObject = enemyObj;
        SetupProperties();
    }

    public virtual void StateUpdate() { 
        if (fieldOfView.visibleTargets.Count > 0) {
            behaviour.SetCurrentState(new EnemyStateChase(enemyObject));
        }
    }

    public abstract void StateFixedUpdate();

    protected abstract Vector3? FindMovementTarget();

    protected virtual void SetupProperties() {
        transform = enemyObject.transform;
        rigidbody = enemyObject.GetComponent<Rigidbody>();
        behaviour = enemyObject.GetComponent<EnemyBehaviour>();
        fieldOfView = enemyObject.GetComponent<FieldOfView>();
        navMeshAgent = enemyObject.GetComponent<NavMeshAgent>();

        movementSpeed = behaviour.movementSpeed;
        navMeshAgent.speed = movementSpeed;
    }

    protected Vector3 DirectionToMovementTarget() {
        Vector3? movementTarget = FindMovementTarget();

        if (movementTarget.HasValue) {
            float xDirectionToTarget = movementTarget.Value.x - transform.position.x;
            float zDirectionToTarget = movementTarget.Value.z - transform.position.z;

            var directionToTarget = new Vector3(xDirectionToTarget, 0, zDirectionToTarget);
            directionToTarget = directionToTarget.normalized;

            return directionToTarget;
        } else {
            return Vector3.zero;
        }
    }

    protected virtual void Move() {
        movementDirection = DirectionToMovementTarget();

        Vector3 movementAmount = movementDirection * (movementSpeed * Time.deltaTime);
        Vector3 newPosition = transform.position + movementAmount;

        rigidbody.MovePosition(newPosition);
        transform.LookAt(newPosition);
        Debug.Log(movementAmount);
    }

    #endregion
}