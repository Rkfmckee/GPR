using UnityEngine;
using UnityEngine.UI;

public class FriendlyListeningUIController : MonoBehaviour {
	#region Properties
	
	private GameObject listeningCommand;
	private GameObject listeningCommandPrefab;

	#endregion

	#region Events

	private void Awake() {
		References.UI.Controllers.friendlyListeningUIController = this;
		listeningCommandPrefab = Resources.Load<GameObject>("Prefabs/UI/FriendlyListeningCommand");
	}

	private void LateUpdate() {
		ListeningCommandFollowMouse();
	}

	#endregion

	#region Methods

	public void EnableListeningCommand() {
		listeningCommand = Instantiate(listeningCommandPrefab, transform);
		listeningCommand.transform.position = ListeningCommandFollowMouse().Value;
	}

	public void DisableListeningCommand() {
		if (listeningCommand == null)
			return;

		Destroy(listeningCommand);
	}

	public void ChangeListeningCommandText(string command) {
		listeningCommand.GetComponentInChildren<Text>().text = command;
	}

	private Vector3? ListeningCommandFollowMouse() {
		if (listeningCommand == null)
			return null;

		Vector3 position = Input.mousePosition;
		listeningCommand.transform.position = position;
		return position;
	}

	#endregion
}
