using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    private void Awake() {
        References.UI.canvas = gameObject;
    }
}
