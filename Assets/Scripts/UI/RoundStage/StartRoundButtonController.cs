using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartRoundButtonController : MonoBehaviour
{
    #region Properties

    private Sprite buttonSpriteUnpressed;
    private Sprite buttonSpritePressed;

    private Image buttonImage;
    private Button startRoundButton;

    #endregion

    #region Events

    private void Awake() {
        buttonImage = GetComponent<Image>();
        startRoundButton = GetComponent<Button>();
        buttonSpriteUnpressed = buttonImage.sprite;
        buttonSpritePressed = Resources.Load<Sprite>("Images/UI/RoundStage/StartRoundButtonPressed");
        startRoundButton.onClick.AddListener(StartRoundButtonClicked);
    }

    #endregion

    #region Methods

    public void SetStartButtonPressed(bool pressed) {
        if (pressed) {
            buttonImage.sprite = buttonSpritePressed;
            startRoundButton.enabled = false;
        } else {
            buttonImage.sprite = buttonSpriteUnpressed;
            startRoundButton.enabled = true;
        }
    }

    private void StartRoundButtonClicked() {
        if (!References.storageRoom.GetComponent<StorageRoomController>().IsPlayerInside()) {
            References.GameController.roundStage.SetCurrentStage(new DefendingAgainstHeroStage());
        } else {
            print("Can't start round with player in Storage Room");
        }
    }

    #endregion
}
