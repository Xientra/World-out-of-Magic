using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Element", menuName = "Element")]
public class Element : ScriptableObject
{
	//public string name; // allready exists

	[Tooltip("The elements in the selector will be sorted from low to high.")]
	public int importance = 999;

	//public Texture2D texture;
	public Sprite image;

	[Space(5)]
	[Tooltip("The category of this element. \nShould the category simply be \"_\" (an underscore) it is regarded as a sub element of the first ingridient in its first recipe.")]
	public string category = "[No Category Yet]";

	[Tooltip("If this element is a subElement, parent element is set to the element this element is the subElement of.\nSubElements cannot be combined with other elements.\n Category will be set to \"_\" is this is not null.")]
	public Element parentElement = null;

	[TextArea(1, 20)]
	public string description = "[No Description Yet]";

	[Space(5)]
	public Recipe[] recipes;

	[Space(10)]
	[Tooltip("If there are subElements to this element they are referenced here.\nGameData should have a button to automatically assign all sub elements to all elements.")]
	public Element[] subElements = new Element[0];


	[HideInInspector]
	[SerializeField]
	private string id;
	public string ID { get => id; }


	public static Element[] FilterUnlockedSubElements(Element[] elements)
	{
		List<Element> result = new List<Element>();

		for (int i = 0; i < elements.Length; i++)
			if (GameData.singelton.UnlockedElements.Contains(elements[i]))
				result.Add(elements[i]);

		return result.ToArray();
	}

#if UNITY_EDITOR
	[Space(15)]
	public bool approved = false;

	[HideInInspector]
	public string id_inspection;

	private void OnValidate()
	{
		// ----- ID stuff ----- //
		id_inspection = id;

		// creates unique id if there is not one allready
		if (string.IsNullOrWhiteSpace(id) || string.IsNullOrEmpty(id))
			id = GUID.Generate().ToString();

		UnityEditor.EditorUtility.SetDirty(this);

		// ----- parent/sub elements ----- //
		if (parentElement != null)
			category = "_";
	}
#endif
}
