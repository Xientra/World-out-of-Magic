using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameData))]
public class ElementAssetLoader : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI(); // draws original inspector

		GameData gd = (GameData)target;

		EditorGUILayout.Space();


		if (GUILayout.Button("Load Elements from Assetbase"))
		{
			string[] assetsGUID = AssetDatabase.FindAssets("t:Element", new[] { "Assets/ScriptableObjects/Elements" });

			if (assetsGUID.Length != 0)
			{
				List<Element> loadInto = new List<Element>();
				foreach (string guid in assetsGUID)
					loadInto.Add(AssetDatabase.LoadAssetAtPath<Element>(AssetDatabase.GUIDToAssetPath(guid)));

				gd.allElements = loadInto.ToArray();
			}
		}
	}
}
