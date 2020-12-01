using UnityEngine;

[CreateAssetMenu(fileName = "New Element", menuName = "ScriptableObjects/Element")]
public class Element : ScriptableObject
{
	//public string name; // allready exists

	public int importance = 0;

	public Texture2D image;

	public string category = "Core";

	[TextArea(1, 10)]
	public string description = "This is a new Element";
}
