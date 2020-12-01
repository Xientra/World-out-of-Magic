using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ElementDisplay : MonoBehaviour
{
	[Space(10)]
	[SerializeField]
	private Element element;
	public Element Element
	{
		get => element;
		set
		{
			element = value;
			UpdateUI();
		}
	}

	[Header("UI:")]

	public TMP_Text nameLabel = null;
	public TMP_Text categoryLabel = null;
	public TMP_Text descriptionLabel = null;
	public RawImage image = null;

	public void UpdateUI()
	{
		if (element == null)
		{
			ClearUI();
			return;
		}

		if (nameLabel != null)
			nameLabel.text = element.name;

		if (categoryLabel != null)
			categoryLabel.text = "(" + element.category + ")";

		if (descriptionLabel != null)
			descriptionLabel.text = element.description;

		if (image != null)
		{
			image.enabled = true;
			image.texture = element.image;
		}
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
			image.texture = null;
		}
	}

	// Update the UI everytime there is a change in the inspector
	private void OnValidate()
	{
		UpdateUI();
	}
}
