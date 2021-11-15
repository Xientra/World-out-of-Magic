using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class Recipe
{
	[HideInInspector]
	public Element result;

	public Element ingredient1;
	public Element ingredient2;

	[TextArea(1, 20)]
	public string explanation;

	public static Recipe[] FilterUnlocked(Recipe[] recipes)
	{
		List<Recipe> result = new List<Recipe>();

		for (int i = 0; i < recipes.Length; i++)
			if (GameData.singelton.UnlockedElements.Contains(recipes[i].ingredient1) && GameData.singelton.UnlockedElements.Contains(recipes[i].ingredient2))
				result.Add(recipes[i]);

		return result.ToArray();
	}
}
