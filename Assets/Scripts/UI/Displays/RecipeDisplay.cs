using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecipeDisplay : MonoBehaviour
{
	[SerializeField]
	private Recipe recipe;

	[SerializeField]
	private TMP_Text ingredient1Label;
	[SerializeField]
	private TMP_Text ingredient2Label;
	[SerializeField]
	private TMP_Text explanationLabel;

	public Recipe Recipe
	{
		get => recipe;
		set
		{
			recipe = value;
			UpdateUI();
		}
	}

	public void UpdateUI()
	{
		if (recipe == null)
		{
			ClearUI();
			return;
		}

		ingredient1Label.text = recipe.ingredient1.name;
		ingredient2Label.text = recipe.ingredient2.name;
		if (explanationLabel != null)
			explanationLabel.text = recipe.explanation;
	}

	public void ClearUI()
	{
		ingredient1Label.text = "";
		ingredient2Label.text = "";
		if (explanationLabel != null)
			explanationLabel.text = "";
	}
}
