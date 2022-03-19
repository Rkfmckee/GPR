﻿using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyState {
    #region Properties

    protected Vector3 movementDirection;
    protected float movementSpeed;

    protected GameObject gameObject;
    protected Transform transform;
    protected Rigidbody rigidbody;
    protected EnemyBehaviour behaviour;
    protected FieldOfView fieldOfView;

    #endregion

    #region Constructor

    public EnemyState(GameObject gameObj) {
        gameObject = gameObj;
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
        transform = gameObject.transform;
        rigidbody = gameObject.GetComponent<Rigidbody>();
        behaviour = gameObject.GetComponent<EnemyBehaviour>();
        fieldOfView = gameObject.GetComponent<FieldOfView>();

        movementSpeed = behaviour.movementSpeed;
    }

	protected void ChaseTargetIfInFieldOfView() {
		if (fieldOfView.visibleTargets.Count > 0) {
            behaviour.SetCurrentState(new EnemyStateChase(gameObject));
        }
	}

    #endregion
}