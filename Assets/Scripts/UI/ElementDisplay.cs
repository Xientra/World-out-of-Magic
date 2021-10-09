using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ElementDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	[Header("Element")]
	[SerializeField]
	private ElementContainer elementContainer;
	public ElementContainer ElementContainer
	{
		get => elementContainer;
		set
		{
			elementContainer = value;
			element = elementContainer.e;
			UpdateUI();
		}
	}

	[SerializeField]
	private Element element;
	public Element Element
	{
		get => element;
		set
		{
			element = value;
			if (element == null)
				elementContainer = null;
			UpdateUI();
		}
	}

	[Header("Settings:")]

	[SerializeField]
	[Tooltip("Update the name of the GameObject with the name of the held element.")]
	private bool updateName = true;

	[SerializeField]
	private bool onlyShowNameOnHover = false;
	[SerializeField]
	[Tooltip("If elementContainer is not set should the ElementDisplay fallback onto the expensive calculation of those informations.")]
	private bool useElementStateFallback = true;

	[Header("Frame Colors:")]

	public Color defaultColor = new Color(0, 0, 0, 0);
	public Color finalElement = Color.yellow;
	public Color elementDone = Color.cyan;

	[Header("UI:")]

	public TMP_Text nameLabel = null;
	public TMP_Text categoryLabel = null;
	public TMP_Text descriptionLabel = null;
	[Space(5)]
	public Image image = null;
	[Space(5)]
	public Image frame = null;
	[Space(5)]
	public TMP_Text noImgLabel = null;
	[Space(5)]
	public Button infoButton;

	public void UpdateUI()
	{
		if (element == null)
		{
			ClearUI();
			return;
		}

		if (nameLabel != null)
		{
			nameLabel.text = element.name;
			nameLabel.gameObject.SetActive(!onlyShowNameOnHover);
		}
		if (categoryLabel != null)
			categoryLabel.text = "(" + element.category + ")";
		if (descriptionLabel != null)
			descriptionLabel.text = element.description;

		if (element.image != null)
		{
			if (image != null)
			{
				image.enabled = true;
				image.sprite = element.image;
			}

			if (noImgLabel != null)
				noImgLabel.gameObject.SetActive(false);
		}
		else if (noImgLabel != null)
		{
			noImgLabel.gameObject.SetActive(true);
			noImgLabel.text = element.name;

			if (image != null)
				image.enabled = false;
		}
		else
		{
			if (image != null)
			{
				image.enabled = false;
				image.sprite = null;
			}
		}

		if (frame != null)
		{
			int elementState = -1;
			if (elementContainer.element != null)
				elementState = elementContainer.State;
			else if (useElementStateFallback)
			{
				elementState = GameData.ElementCombinationStatus(element);
				Debug.LogWarning("Used Fallback to GameData.ElementCombinationStatus for element display with element: " + element.name);
			}

			if (elementState != -1)
				switch (elementState)
				{
					case 1:
						frame.color = elementDone;
						break;
					case 2:
						frame.color = finalElement;
						break;
					default:
						frame.color = defaultColor;
						break;
				}
		}

		if (updateName == true)
			gameObject.name = (element != null ? element.name : "Empty") + " Display";
	}

	public void Clear()
	{
		element = null;
		ClearUI();
	}

	private void ClearUI()
	{
		if (nameLabel != null)
			nameLabel.text = "";

		if (categoryLabel != null)
			categoryLabel.text = "";

		if (descriptionLabel != null)
			descriptionLabel.text = "";


		if (image != null)
		{
			image.enabled = false;
			image.sprite = null;
		}
		if (noImgLabel != null)
		{
			noImgLabel.text = "";
			noImgLabel.gameObject.SetActive(false);
		}

		if (frame != null)
			frame.color = defaultColor;
	}

	public void SetActive(bool value)
	{
		if (value == true)
			UpdateUI();
		this.gameObject.SetActive(value);
	}

	// Update the UI everytime there is a change in the inspector
	private void OnValidate()
	{
		UpdateUI();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		OnHover(true);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		OnHover(false);
	}

	private void OnHover(bool value)
	{
		if (onlyShowNameOnHover == true)
			if (nameLabel != null)
				nameLabel.gameObject.SetActive(value);

		if (infoButton != null)
			infoButton.gameObject.SetActive(value);
	}

	public void Btn_InfoButton()
	{
		FullScreenElementDisplay.singelton.Display(element);
	}
}
