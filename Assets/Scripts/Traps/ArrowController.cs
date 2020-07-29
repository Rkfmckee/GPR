using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    #region Properties

    private float speed;
    private new Rigidbody rigidbody;
    private Collider[] collidersToIgnore;

    #endregion

    #region Events

    private void Awake() {
        rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    private void Update() {
        transform.rotation = Quaternion.LookRotation(rigidbody.velocity);
    }

    private void OnCollisionEnter(Collision collision) {
        Destroy(gameObject);
    }

    #endregion

    #region Methods

    public void SetCollidersToIgnore(Collider[] colliders) {
        collidersToIgnore = colliders;

        foreach(Collider collider in collidersToIgnore) {
            Physics.IgnoreCollision(GetComponent<Collider>(), collider);
        }
    }

    #endregion
}
