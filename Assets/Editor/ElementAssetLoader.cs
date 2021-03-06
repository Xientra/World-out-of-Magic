﻿using System.Collections;
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
