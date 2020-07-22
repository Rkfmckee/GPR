using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class References {
    public static List<GameObject> players = new List<GameObject>();
    public static GameObject currentPlayer;
    public static GameObject gameController;

    public static class UI {
        public static GameObject canvas;
    }
}
