using System.Collections.Generic;
using UnityEngine;

public static class References {
    public static GameObject storageRoom;

	public static class Camera {
		public static UnityEngine.Camera camera;
		public static CameraController cameraController;
	}

    public static class FriendlyCreature {
        public static List<GameObject> goblins = new List<GameObject>();
    }

	public static class HostileCreature {
		public static List<GameObject> enemies = new List<GameObject>();
    	public static GameObject spawnArea;
	}

    public static class Game {
        public static GameObject globalGameObject;
        public static GlobalObstaclesController globalObstacles;
        public static RoundStageController roundStage;
        public static ResourceController resources;
		public static CursorController cursor;
    }

    public static class UI {
		public static class Controllers {
			public static CanvasController canvasController;
			public static FriendlyListeningUIController friendlyListeningUIController;
		}
        public static GameObject canvas;
        public static NotificationController notifications;
    }
}
