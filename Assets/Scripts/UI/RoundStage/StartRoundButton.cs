using UnityEngine;
using UnityEngine.UI;

public class StartRoundButton : MonoBehaviour {
	#region Properties

	private bool buttonPressed;
	private Sprite buttonSpriteUnpressed;
	private Sprite buttonSpritePressed;

	private Image buttonImage;
	private Button startRoundButton;

	#endregion

	#region Events

	private void Awake() {
		buttonImage = GetComponent<Image>();
		startRoundButton = GetComponent<Button>();

		buttonPressed = false;
		buttonSpriteUnpressed = buttonImage.sprite;
		buttonSpritePressed = Resources.Load<Sprite>("Images/UI/RoundStage/StartRoundButtonPressed");
		startRoundButton.onClick.AddListener(StartRoundButtonClicked);
	}

	#endregion

	#region Methods

		#region Get/Set

		public bool IsButtonPressed() {
			return buttonPressed;
		}

		public void SetStartButtonPressed(bool pressed) {
			buttonPressed = pressed;
			
			if (pressed) {
				buttonImage.sprite = buttonSpritePressed;
				startRoundButton.enabled = false;
			} else {
				buttonImage.sprite = buttonSpriteUnpressed;
				startRoundButton.enabled = true;
			}
		}

		#endregion

	private void StartRoundButtonClicked() {
		if (!References.storageRoom.GetComponent<StorageRoom>().IsPlayerInside()) {
			References.Game.roundStage.SetCurrentStage(new DefendingAgainstHeroStage());
		} else {
			print("Can't start round with player in Storage Room");
		}
	}

	#endregion
}
