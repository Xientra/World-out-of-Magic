using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Element", menuName = "Element")]
public class Element : ScriptableObject
{
	//public string name; // allready exists

	[Tooltip("The elements in the selector will be sorted from low to high.")]
	public int importance = 999;

	//public Texture2D texture;
	public Sprite image;

	public string category = "Core";

	[TextArea(1, 20)]
	public string description = "This is a new Element";


	//[Tooltip("A final element is not part of any recipe.")]
	//public bool final;

	public Recipe[] recipes;


	[HideInInspector]
	[SerializeField]
	private string id;
	public string ID { get => id; }


#if UNITY_EDITOR
	[HideInInspector]
	public string id_inspection;

	private void OnValidate()
	{
		id_inspection = id;

		// creates unique id if there is not one allready
		if (string.IsNullOrWhiteSpace(id) || string.IsNullOrEmpty(id))
			id = GUID.Generate().ToString();

		UnityEditor.EditorUtility.SetDirty(this);
	}
#endif
}
