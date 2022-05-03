using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
	#region Properties

	public Sprite regularSprite;
	public Sprite highlightSprite;

	private Image image;

	#endregion
	
	#region Events

	protected virtual void Awake() {
		image = GetComponent<Image>();
	}

	public virtual void OnPointerEnter(PointerEventData eventData) {
		image.sprite = highlightSprite;
	}

	public virtual void OnPointerExit(PointerEventData eventData) {
		image.sprite = regularSprite;
	}

	#endregion
}
