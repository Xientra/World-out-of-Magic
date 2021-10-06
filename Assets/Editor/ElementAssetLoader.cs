using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameData))]
public class ElementAssetLoader : Editor
{
	public override void OnInspectorGUI()
	{
		GameData gd = (GameData)target;

		if (GUILayout.Button("Send UI Update Signal"))
			gd.SendUIUpdateSignal();


		if (GUILayout.Button("Unlock Everthing"))
		{
			gd.UnlockAllElements();
			gd.SendUIUpdateSignal();
		}


		if (GUILayout.Button("Load Elements from Assetbase"))
		{
			string[] assetsGUID = AssetDatabase.FindAssets("t:Element", new[] { "Assets/ScriptableObjects/Elements" });

			if (assetsGUID.Length != 0)
			{
				List<Element> loadInto = new List<Element>();
				foreach (string guid in assetsGUID)
				{
					Element e = AssetDatabase.LoadAssetAtPath<Element>(AssetDatabase.GUIDToAssetPath(guid));
					if (e.recipes.Length != 0)
						if (e.recipes[0].ingredient1 != null && e.recipes[0].ingredient2 != null)
							loadInto.Add(e);
					if (e.name == "Magic")
						loadInto.Add(e);
				}

				gd.allElements = loadInto.ToArray();
			}
		}

		if (GUILayout.Button("Load Approved Elements from Assetbase"))
		{
			string[] assetsGUID = AssetDatabase.FindAssets("t:Element", new[] { "Assets/ScriptableObjects/Elements" });

			if (assetsGUID.Length != 0)
			{
				List<Element> loadInto = new List<Element>();
				foreach (string guid in assetsGUID)
				{
					Element e = AssetDatabase.LoadAssetAtPath<Element>(AssetDatabase.GUIDToAssetPath(guid));
					if (e.approved)
						loadInto.Add(e);
				}

				gd.allElements = loadInto.ToArray();
			}
		}

		EditorGUILayout.Space(15);

		base.OnInspectorGUI(); // draws original inspector
	}
}
