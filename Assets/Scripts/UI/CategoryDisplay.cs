using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CategoryDisplay : MonoBehaviour
{
	private string text = "";
	public string Text
	{
		get => text;
		set
		{
			text = value;
			UpdateUI();
		}
	}

	public TMP_Text label;

	private void UpdateUI()
	{
		gameObject.name = text + " Category";
		label.text = text;
	}
}
