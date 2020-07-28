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
        arrowPrefab = Resources.Load("Prefabs/Traps/Arrow") as GameObject;
        trapType = Type.Wall;

        arrowSlots = new List<GameObject>();
        foreach(Transform child in transform) {
            arrowSlots.Add(child.gameObject);
        }
    }

    #endregion

    public override void TriggerTrap(Collider triggeredBy) {
        foreach(GameObject arrowSlot in arrowSlots) {
            GameObject arrow = Instantiate(arrowPrefab);
            arrow.transform.eulerAngles = new Vector3(0, arrowSlot.transform.rotation.y + 180, 0);
            arrow.transform.position = arrowSlot.transform.position + (arrow.transform.forward);
            arrow.transform.LookAt(-arrowSlot.transform.forward*3);
            arrow.GetComponent<ArrowController>().SetSpeed(10);
        }
    }
}
