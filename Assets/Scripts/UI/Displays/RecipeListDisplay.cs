using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeListDisplay : MonoBehaviour
{
	[SerializeField]
	private Transform parentTransform;

	[SerializeField]
	private RecipeDisplay recipeDisplaySampe;

	[SerializeField]
	private List<RecipeDisplay> recipeDisplays;


	private void Start()
	{
		recipeDisplaySampe.gameObject.SetActive(false);
	}

	public void SetRecipes(Recipe[] recipes)
	{
		Clear();
		if (recipes == null)
			return;

		for (int i = 0; i < recipes.Length; i++)
		{
			RecipeDisplay rd = Instantiate(recipeDisplaySampe, parentTransform != null ? parentTransform : transform).GetComponent<RecipeDisplay>();
			rd.Recipe = recipes[i];
			rd.gameObject.SetActive(true);
			recipeDisplays.Add(rd);
		}
	}

	private void Clear()
	{
		foreach (RecipeDisplay rd in recipeDisplays)
			Destroy(rd.gameObject);

		recipeDisplays.Clear();
	}
}
