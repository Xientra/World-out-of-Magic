using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
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
	private bool showHoverDisplay = true;

	[SerializeField]
	[Tooltip("If elementContainer is not set should the ElementDisplay fallback onto the expensive calculation of those informations.")]
	private bool useElementStateFallback = true;

	[Header("Frame Colors:")]

	public Color defaultColor = new Color(0, 0, 0, 0);
	public Color finalElement = Color.yellow;
	public Color elementDone = Color.cyan;
	public Color onlySubElementRemain = Color.green;

	[Header("UI:")]

	public TMP_Text nameLabel = null;
	public TMP_Text categoryLabel = null;
	public TMP_Text descriptionLabel = null;
	[Space(5)]
	public Image image = null;
	public Image frame = null;
	public TMP_Text noImgLabel = null;
	[Space(5)]
	public Button infoButton;
	[Space(5)]
	public RecipeListDisplay recipeListDisplay = null;
	public SubElementListDisplay subElementListDisplay = null;

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
			categoryLabel.text = "(" + (element.parentElement == null ? element.category : element.parentElement.name) + ")";
		if (descriptionLabel != null)
			descriptionLabel.text = element.description;
		if (recipeListDisplay != null)
			recipeListDisplay.SetRecipes(Recipe.FilterUnlocked(element.recipes));
		if (subElementListDisplay != null)
			subElementListDisplay.SetRecipes(Element.FilterUnlockedSubElements(element.subElements));

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
					case 3:
						frame.color = onlySubElementRemain;
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
		OnHover(false);

		if (nameLabel != null)
			nameLabel.text = "";

		if (categoryLabel != null)
			categoryLabel.text = "";

		if (descriptionLabel != null)
			descriptionLabel.text = "";

		if (recipeListDisplay != null)
			recipeListDisplay.SetRecipes(null);


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

		if (showHoverDisplay && element != null)
			if (value == true)
				HoverDisplay.singelton.Display(element.name, (RectTransform)transform);
			else
				HoverDisplay.singelton.Hide();
	}

	public void Btn_InfoButton()
	{
		if (elementContainer != null && elementContainer.element != null)
			FullScreenElementDisplay.singelton.Display(elementContainer);
		else
			FullScreenElementDisplay.singelton.Display(element);
		OnHover(false);
	}
}
