using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrapDetection : MonoBehaviour
{
    private SpikeTrapController parentController;

    private void Awake() {
        parentController = transform.parent.GetComponent<SpikeTrapController>();
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Enemy") {
            if (parentController != null) {
                parentController.CollisionDetected(collision);
            }
        }
    }
}
