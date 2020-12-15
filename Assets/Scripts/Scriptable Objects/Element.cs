using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Element", menuName = "ScriptableObjects/Element")]
public class Element : ScriptableObject
{
	//public string name; // allready exists

	public int importance = 0;

	//public Texture2D texture;
	public Sprite image;

	public string category = "Core";

	[TextArea(1, 20)]
	public string description = "This is a new Element";

	[HideInInspector]
	[SerializeField]
	private string id;
	public string ID { get => id; }
	[HideInInspector]


#if UNITY_EDITOR
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
