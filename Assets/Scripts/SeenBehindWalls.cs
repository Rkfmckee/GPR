using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeenBehindWalls : MonoBehaviour
{
    public bool drawDebugLines;

    private new Camera camera;
    private List<GameObject> hiddenObjects;
    private int hiddenLayers;

    void Awake() {
        camera = Camera.main;
        hiddenObjects = new List<GameObject>();

        int wallMask = 1 << LayerMask.NameToLayer("Wall");
        int wallDecMask = 1 << LayerMask.NameToLayer("WallDecoration");

        hiddenLayers = wallMask | wallDecMask;
    }

    void Update() {
        seeObjectThroughWalls();
    }

    private void seeObjectThroughWalls() {
        Vector3 direction = transform.position - camera.transform.position;
        float distance = direction.magnitude;

        RaycastHit[] hits = Physics.RaycastAll(camera.transform.position, direction, distance, hiddenLayers);

        foreach(RaycastHit hit in hits) {
            GameObject currentHit = hit.transform.gameObject;

            if (!hiddenObjects.Contains(currentHit)) {
                hiddenObjects.Add(currentHit);
                currentHit.GetComponent<Renderer>().enabled = false;
            }
        }

        CleanInappropriateMembers(hits);
    }

    private void CleanInappropriateMembers(RaycastHit[] hits) {
        for (int i = 0; i < hiddenObjects.Count; i++) {
            bool isHit = false;

            for (int j = 0; j < hits.Length; j++) {
                if (hits[j].transform.gameObject == hiddenObjects[i]) {
                    isHit = true;
                    break;
                }
            }

            if (!isHit) {
                GameObject wasHidden = hiddenObjects[i];
                wasHidden.GetComponent<Renderer>().enabled = true;
                hiddenObjects.RemoveAt(i);
                i--;
            }
        }
    }
}
