using System.Collections.Generic;
using UnityEngine;

public static class References {
    public static List<GameObject> enemies = new List<GameObject>();
    public static GameObject enemySpawnArea;
    public static GameObject storageRoom;

    public static class Player {
        public static List<GameObject> players = new List<GameObject>();
        public static GameObject currentPlayer;
        public static HoldObject playerHoldController;
    }

    public static class GameController {
        public static GameObject gameControllerObject;
        public static GameTrapsController gameTraps;
        public static RoundStageController roundStage;
        public static ResourceController resources;
    }

    public static class UI {
        public static GameObject canvas;
        public static NotificationController notifications;
    }
}
