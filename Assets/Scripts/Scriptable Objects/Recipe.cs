using System;
using UnityEngine;

[Serializable]
public class Recipe
{
	public Element ingredient1;
	public Element ingredient2;

	[TextArea(1, 20)]
	public string explanation;
}
