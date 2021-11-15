using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class HintManager
{
	// these two categories have the most combinations between them

	// this element has the most combinations remaining

	// enable/disable only sub element frame

	// enable/disable isDone frame

	// this category has the most combinations remaining


	public static void ShowRandomHint()
	{


	}

	public struct CategoryPair
	{
		string category1;
		string category2;
	}

	public static CategoryPair GetCategoriesWithTheMostCombinationsBetweenThem()
	{
		throw new System.NotImplementedException();

		CategoryPair result = new CategoryPair();

		string[] categories = GameData.singelton.categories;

		// TODO: i beg you paul you HAVE TO STORE THE ELEMENTS FOR EACH CATEGORY FIRST

		for (int i = 0; i < categories.Length; i++)
			for (int j = 0; j < categories.Length; j++)
			{
				List<Element> c1Elements = GameData.singelton.GetUnlockedElementContainerOfCategory(categories[i]).ConvertAll(ec => ec.e);
				List<Element> c2Elements = GameData.singelton.GetUnlockedElementContainerOfCategory(categories[j]).ConvertAll(ec => ec.e);
			
				
			}

		GameData.singelton.GetUnlockedElementContainerOfCategory(categories[0]);

		return result;
	}

	public static Element GetElementWithMostCombinationsRemaining()
	{
		List<Element> unlockedE = GameData.singelton.UnlockedElements;

		Element result = null;
		int mostCombinations = -1;

		foreach (Element e in unlockedE)
		{
			List<Recipe> rs = GameData.RecipiesWithElement(e);
			int combinations = rs.ConvertAll(rec => rec.result).Distinct().ToList().FindAll(res => !unlockedE.Contains(res)).Count;

			if (combinations > mostCombinations)
			{
				mostCombinations = combinations;
				result = e;
			}
		}

		return result;
	}
}
