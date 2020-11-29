using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combiner : MonoBehaviour
{
	public Recipe[] recipes;

	public List<Element> unlockedElements;

	public void Combine(Element e1, Element e2)
	{
		for (int i = 0; i < recipes.Length; i++)
		{
			if (recipes[i].ingredient1 == e1 && recipes[i].ingredient2 == e2)
			{
				unlockedElements.Add(recipes[i].result);
			}
		}
	}
}
