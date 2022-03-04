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

    #region Constructor

    public EnemyState(GameObject enemyObj) {
        enemyObject = enemyObj;
        SetupProperties();
    }

    #endregion

    #region Events

    public abstract void Update();

    public abstract void FixedUpdate();

    public abstract void OnCollisionEnter(Collision collision);

    #endregion

    #region Methods

    protected virtual void SetupProperties() {
        transform = enemyObject.transform;
        rigidbody = enemyObject.GetComponent<Rigidbody>();
        behaviour = enemyObject.GetComponent<EnemyBehaviour>();
        fieldOfView = enemyObject.GetComponent<FieldOfView>();

        movementSpeed = behaviour.movementSpeed;
    }

    #endregion
}