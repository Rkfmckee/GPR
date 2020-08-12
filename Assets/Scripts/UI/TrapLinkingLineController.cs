using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapLinkingLineController : MonoBehaviour
{
    #region Properties

    private LineRenderer lineRenderer;
    private new Camera camera;
    private RaycastHit hit;
    private int trapLinkingLineLayerMask;

    #endregion

    #region Events

    private void Awake() {
        lineRenderer = GetComponent<LineRenderer>();
        camera = Camera.main;

        lineRenderer.SetPosition(0, Vector3.zero);
        lineRenderer.SetPosition(1, Vector3.zero);

        int highlightableObjectLayerMask = 1 << LayerMask.NameToLayer("Highlightable");
        int obstacleLayerMask = 1 << LayerMask.NameToLayer("Obstacle");
        int wallLayerMask = 1 << LayerMask.NameToLayer("Wall");
        int floorLayerMask = 1 << LayerMask.NameToLayer("Floor");
        trapLinkingLineLayerMask = highlightableObjectLayerMask | obstacleLayerMask | wallLayerMask | floorLayerMask;
    }

    private void Update() {
        Ray cameraToMouse = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(cameraToMouse, out hit, Mathf.Infinity, trapLinkingLineLayerMask)) {
            lineRenderer.SetPosition(1, hit.point);
        }
    }

    #endregion

    #region Methods

    public void SetStartValue(Transform startTransform) {
        lineRenderer.SetPosition(0, startTransform.position + (Vector3.up * (startTransform.GetComponent<Collider>().bounds.size.y / 2)));
    }

    #endregion
}
