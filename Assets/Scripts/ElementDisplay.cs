﻿using UnityEngine;
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
	[Space(5)]
	public RawImage rawImage = null;
	public Image image = null;
	[Space(5)]
	public TMP_Text noImgLabel = null;

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
			image.sprite = null;
		}
		if (noImgLabel != null)
		{
			noImgLabel.text = "";
			noImgLabel.gameObject.SetActive(false);
		}
	}

	// Update the UI everytime there is a change in the inspector
	private void OnValidate()
	{
		UpdateUI();
	}
}
