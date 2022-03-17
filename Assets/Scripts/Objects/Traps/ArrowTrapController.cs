using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTrapController : TrapController {

    #region Properties

    private GameObject arrowPrefab;
    private List<GameObject> arrowSlots;

    #endregion

    #region Events

    private void Awake() {
        trapType = Type.WALL;

        arrowPrefab = Resources.Load("Prefabs/Traps/Arrow") as GameObject;

        arrowSlots = new List<GameObject>();
        foreach(Transform child in transform) {
            arrowSlots.Add(child.gameObject);
        }
    }

    #endregion

    public override void TriggerTrap(Collider triggeredBy) {
        foreach(GameObject arrowSlot in arrowSlots) {
            GameObject arrow = Instantiate(arrowPrefab);
            ArrowController arrowController = arrow.GetComponent<ArrowController>();
            Rigidbody arrowRigidbody = arrow.GetComponent<Rigidbody>();

            arrow.transform.rotation = Quaternion.LookRotation(transform.forward);
            arrow.transform.position = arrowSlot.transform.position;

            if (arrowController != null) {
                arrowController.SetCollidersToIgnore(Physics.OverlapSphere(arrow.transform.position, 1));
            }

            if (arrowRigidbody != null) {
                arrowRigidbody.velocity = arrow.transform.forward * 30;
            }
        }
    }
}
