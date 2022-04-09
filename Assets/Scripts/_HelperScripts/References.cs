using System.Collections.Generic;
using UnityEngine;

public static class References {
    public static GameObject storageRoom;

    public static class FriendlyCreature {
        public static List<GameObject> goblins = new List<GameObject>();
    }

	public static class HostileCreature {
		public static List<GameObject> enemies = new List<GameObject>();
    	public static GameObject spawnArea;
	}

    public static class GameController {
        public static GameObject gameControllerObject;
        public static GameTrapsController gameTraps;
        public static RoundStageController roundStage;
        public static ResourceController resources;
    }

    public static class UI {
        public static GameObject canvas;
		public static CanvasController canvasController;
        public static NotificationController notifications;
    }
}
