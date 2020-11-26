using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "ScriptableObjects/Recipe")]
public class Recipe : ScriptableObject
{
	[Header("Ingredients:")]

	public Element ingredient1;
	public Element ingredient2;

	[Header("Result:")]

	public Element result;
}
