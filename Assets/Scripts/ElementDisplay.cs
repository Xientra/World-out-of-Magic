using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ElementDisplay : MonoBehaviour
{
	[Space(10)]
	public Element element;

	[Header("UI:")]

	public TMP_Text nameLabel;
	public TMP_Text descriptionLabel;
	public RawImage image;

	public void DisplayElement(Element element)
	{
		this.element = element;
	}

	public void UpdateUI()
	{
		if (element == null)
		{
			Clear();
			return;
		}
		nameLabel.text = element.name;
		descriptionLabel.text = element.description;
		image.enabled = true;
		image.texture = element.image;
	}

	public void Clear()
	{
		nameLabel.text = "";
		descriptionLabel.text = "";
		image.enabled = false;
		image.texture = null;
	}

	// Update the UI everytime there is a change in the inspector
	private void OnValidate()
	{
		UpdateUI();
	}
}
