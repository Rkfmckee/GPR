using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    #endregion

    #region Methods

    public EnemyState(GameObject enemyObj) {
        enemyObject = enemyObj;
        SetupProperties();
    }

    protected abstract Vector3? FindMovementTarget();

    public virtual void MoveTowardsTarget() {
        Move();
    }

    protected virtual void SetupProperties() {
        transform = enemyObject.transform;
        rigidbody = enemyObject.GetComponent<Rigidbody>();
        behaviour = enemyObject.GetComponent<EnemyBehaviour>();
        fieldOfView = enemyObject.GetComponent<FieldOfView>();

        movementSpeed = behaviour.movementSpeed;
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
    }

    #endregion
}