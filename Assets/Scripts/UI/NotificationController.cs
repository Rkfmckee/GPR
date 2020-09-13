using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationController : MonoBehaviour {
    #region Properties

    public float notificationDisplayTime;
    public float notificationFadeTime;

    private GameObject currentNotification;
    private List<GameObject> notificationQueue;
    private GameObject notificationPrefab;
    private Color notificationColor;

    private float currentDisplayTime;
    private float currentFadeTime;

    private Text currentTextComponent;

    #endregion

    #region Events

    private void Awake() {
        References.UI.notifications = this;

        notificationQueue = new List<GameObject>();
        notificationPrefab = Resources.Load<GameObject>("Prefabs/UI/NotificationText");
        currentDisplayTime = 0;
        currentFadeTime = 0;
        notificationColor = notificationPrefab.GetComponent<Text>().color;
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

    public void AddNotification(string notificationText) {
        GameObject newNotif = Instantiate(notificationPrefab, transform);
        newNotif.transform.localPosition = new Vector2(0, 0);
        newNotif.GetComponent<Text>().text = notificationText;
        newNotif.SetActive(false);
        notificationQueue.Add(newNotif);
    }

    private void GetNextNotificationFromQueue() {
        currentNotification = notificationQueue[0];
        currentNotification.SetActive(true);
        currentTextComponent = currentNotification.GetComponent<Text>();

        print(currentTextComponent.text);
        notificationQueue.Remove(currentNotification);
    }

    private void FadeOutCurrentNotification() {
        currentFadeTime += Time.deltaTime;

        if (currentTextComponent != null) {
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
}
