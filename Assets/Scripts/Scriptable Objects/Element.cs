using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Element", menuName = "ScriptableObjects/Element")]
public class Element : ScriptableObject
{
	//public string name; // allready exists

	public int importance = 0;

	public Texture2D texture;
	public Sprite image;

	public string category = "Core";

	[TextArea(1, 20)]
	public string description = "This is a new Element";

	public string ID { get; private set; }

#if UNITY_EDITOR
	private void OnValidate()
	{
		// creates unique id if there is not one allready
		if (string.IsNullOrEmpty(ID))
			ID = GUID.Generate().ToString();

		UnityEditor.EditorUtility.SetDirty(this);
	}
#endif

}
