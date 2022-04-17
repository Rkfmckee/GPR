using System.Collections.Generic;
using UnityEngine;

public class ArrowTrap : TrapController {

    #region Properties

	public float arrowScale;

    private GameObject arrowPrefab;
    private List<GameObject> arrowSlots;
	private Vector3 arrowRotationOffset;

    #endregion

    #region Events

    private void Awake() {
        trapType = SurfaceType.WALL;

        arrowPrefab = Resources.Load("Prefabs/Obstacles/Traps/ArrowTrap/Arrow") as GameObject;
		arrowRotationOffset = new Vector3(0, 90, 0);

        arrowSlots = new List<GameObject>();
        foreach(Transform child in transform) {
            arrowSlots.Add(child.gameObject);
        }
    }

    #endregion

    public override void TriggerTrap(Collider triggeredBy) {
        foreach(GameObject arrowSlot in arrowSlots) {
            GameObject arrow = Instantiate(arrowPrefab);
            Arrow arrowController = arrow.GetComponent<Arrow>();
            Rigidbody arrowRigidbody = arrow.GetComponent<Rigidbody>();

            arrow.transform.parent = transform;
			arrow.transform.localRotation = Quaternion.Euler(Vector3.zero);
			arrow.transform.localScale = Vector3.one * arrowScale;
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
