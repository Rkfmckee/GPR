using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageRoomController : MonoBehaviour
{
    #region Properties

    public float doorTimeToOpen;

    private bool playerInStorageRoom;
    private DoorState doorOpenState;
    private float doorCurrentMovingTime;
    private Quaternion openedRotation;
    private Quaternion closedRotation;
    private GameObject storageRoomDoor;
    private Collider storageRoomCollider;

    #endregion

    #region Events

    private void Awake() {
        storageRoomCollider = GetComponent<Collider>();

        References.storageRoom = gameObject;
        storageRoomDoor = transform.Find("StorageRoomDoor").gameObject;
        doorOpenState = DoorState.Open;

        openedRotation = Quaternion.Euler(new Vector3(0, 180, 0));
        closedRotation = Quaternion.Euler(new Vector3(0, 270, 0));
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            playerInStorageRoom = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "Player") {
            playerInStorageRoom = false;
        }
    }

    #endregion

    #region Methods

    public void Open() {
        storageRoomCollider.isTrigger = true;
        StartCoroutine(OpeningDoor());
    }

    public void Close() {
        storageRoomCollider.isTrigger = false;
        StartCoroutine(ClosingDoor());
    }

    public bool IsDoorOpen() {
        return doorOpenState == DoorState.Open;
    }

    public bool IsPlayerInside() {
        return playerInStorageRoom;
    }

    #endregion

    #region Coroutines

    private IEnumerator OpeningDoor() {
        print("Opening Storage Room");
        doorCurrentMovingTime = 0;

        while (doorCurrentMovingTime < doorTimeToOpen) {
            storageRoomDoor.transform.localRotation = Quaternion.Lerp(closedRotation, openedRotation, doorCurrentMovingTime / doorTimeToOpen);
            doorCurrentMovingTime += Time.deltaTime;

            yield return null;
        }

        doorOpenState = DoorState.Open;
    }

    private IEnumerator ClosingDoor() {
        print("Closing Storage Room");
        doorCurrentMovingTime = 0;

        while (doorCurrentMovingTime < doorTimeToOpen) {
            storageRoomDoor.transform.localRotation = Quaternion.Lerp(openedRotation, closedRotation, doorCurrentMovingTime / doorTimeToOpen);
            doorCurrentMovingTime += Time.deltaTime;

            yield return null;
        }

        doorOpenState = DoorState.Closed;
    }

    #endregion

    #region Enums

    public enum DoorState {
        Open,
        Closed
    }

    #endregion
}
