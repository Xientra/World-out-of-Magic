using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "ScriptableObjects/Recipe")]
public class Recipe : ScriptableObject
{
	[Header("Ingredients:")]

	public Element ingredient1;
	public Element ingredient2;

	[Header("Result:")]

	public Element result;

	private void OnValidate()
	{
		string i1 = ingredient1 != null ? ingredient1.name : "[i1]";
		string i2 = ingredient2 != null ? ingredient2.name : "[i2]";
		string r = result != null ? result.name : "[result]";

		name = i1 + " + " + i2 + " = " + r;
	}
}
