using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotificationController : MonoBehaviour {
    #region Properties

    public float notificationDisplayTime;
    public float notificationFadeTime;

    private GameObject currentNotification;
    private List<GameObject> notificationQueue;
    private GameObject notificationPrefab;

    private float currentDisplayTime;
    private float currentFadeTime;

    private TextMeshProUGUI currentTextComponent;

    #endregion

    #region Events

    private void Awake() {
        References.UI.notifications = this;

        notificationQueue = new List<GameObject>();
        notificationPrefab = Resources.Load<GameObject>("Prefabs/UI/NotificationText");
        currentDisplayTime = 0;
        currentFadeTime = 0;
    }

    private void Update() {
        if (currentNotification == null) {
            if (notificationQueue.Count > 0) {
                GetNextNotificationFromQueue();
            }
        } else {
            currentDisplayTime += Time.deltaTime;

            if (currentDisplayTime >= notificationDisplayTime) {
                FadeOutCurrentNotification();
            }
        }
    }

    #endregion

    #region Methods

    public void AddNotification(string notificationText, NotificationType type) {
        Color notificationColor = Color.white;
		
		switch(type) {
			case NotificationType.Info:
				notificationColor = Color.white;
				break;
			case NotificationType.Success:
				notificationColor = Color.green;
				break;
			case NotificationType.Error:
				notificationColor = Color.red;
				break;
		}
		
		var newNotif = Instantiate(notificationPrefab, transform);
		var newNotifTextComponent = newNotif.GetComponent<TextMeshProUGUI>();
        newNotif.transform.localPosition = new Vector2(0, 0);
        newNotifTextComponent.text = notificationText;
		newNotifTextComponent.color = notificationColor;
        newNotif.SetActive(false);
        notificationQueue.Add(newNotif);
    }

    private void GetNextNotificationFromQueue() {
        currentNotification = notificationQueue[0];
        currentNotification.SetActive(true);
        currentTextComponent = currentNotification.GetComponent<TextMeshProUGUI>();

        print(currentTextComponent.text);
        notificationQueue.Remove(currentNotification);
    }

    private void FadeOutCurrentNotification() {
        currentFadeTime += Time.deltaTime;

        if (currentTextComponent != null) {
			var notificationColor = currentTextComponent.color;

            notificationColor.a = 1 - (currentFadeTime / notificationFadeTime);
            currentTextComponent.color = notificationColor;
            currentNotification.transform.position += Vector3.up / 5;
        }

        if (currentFadeTime >= notificationFadeTime) {
            Destroy(currentNotification);
            currentDisplayTime = 0;
            currentFadeTime = 0;
        }
    }

    #endregion

	#region Enums

	public enum NotificationType {
		Info,
		Success,
		Error
	}

	#endregion
}
