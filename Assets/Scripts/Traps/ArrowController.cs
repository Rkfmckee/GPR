using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    #region Properties

    private float speed = 100;
    private new Rigidbody rigidbody;

    #endregion

    #region Events

    private void Awake() {
        float speedPerFrame = speed * Time.deltaTime;
        rigidbody = gameObject.GetComponent<Rigidbody>();

        rigidbody.velocity = transform.forward * speedPerFrame;

        print(rigidbody.velocity);
    }

    private void OnCollisionEnter(Collision collision) {
        Destroy(gameObject);
    }

    #endregion

    #region Methods

    public void SetSpeed(float value) {
        speed = value;
    }

    #endregion
}
