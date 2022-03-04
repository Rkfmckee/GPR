using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyState {
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

    public abstract void StateUpdate();

    public abstract void StateFixedUpdate();

    public EnemyState(GameObject enemyObj) {
        enemyObject = enemyObj;
        SetupProperties();
    }

    protected virtual void SetupProperties() {
        transform = enemyObject.transform;
        rigidbody = enemyObject.GetComponent<Rigidbody>();
        behaviour = enemyObject.GetComponent<EnemyBehaviour>();
        fieldOfView = enemyObject.GetComponent<FieldOfView>();

        movementSpeed = behaviour.movementSpeed;
    }

    public void ChangeDirectionAfterHittingWall(Collision collision) {
        var direction = collision.contacts[0].normal;

        direction = Quaternion.AngleAxis(Random.Range(-70.0f, 70.0f), Vector3.up) * direction;

        movementDirection = direction;
        transform.rotation = Quaternion.LookRotation(movementDirection);
    }

    #endregion
}