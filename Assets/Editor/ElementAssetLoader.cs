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
					if (e == gd.originElement)
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

		if (GUILayout.Button("Set Parent and Sub Elements"))
		{
			string[] assetsGUID = AssetDatabase.FindAssets("t:Element", new[] { "Assets/ScriptableObjects/Elements" });
			if (assetsGUID.Length != 0)
			{
				// get all elements
				List<Element> allElementAssets = new List<Element>();
				foreach (string guid in assetsGUID)
					allElementAssets.Add(AssetDatabase.LoadAssetAtPath<Element>(AssetDatabase.GUIDToAssetPath(guid)));


				// set sub elements based on parents
				List<Element> setSubElementBasedOnThis = allElementAssets.FindAll(e => e.parentElement != null);

				foreach (Element e in allElementAssets)
				{
					List<Element> subElements = new List<Element>();
					foreach (Element potentialSubElement in setSubElementBasedOnThis)
						if (potentialSubElement.parentElement == e)
							subElements.Add(potentialSubElement);

					subElements.Sort((e1, e2) => e1.importance - e2.importance);
					e.subElements = subElements.ToArray();
				}


				// set parent elements based on parent elements
				List<Element> setParentElementBasedOnThis = allElementAssets.FindAll(e => e.subElements.Length > 0);

				foreach (Element e in setParentElementBasedOnThis)
					foreach (Element subE in e.subElements)
						subE.parentElement = e;
			}
		}

		EditorGUILayout.Space(15);

		base.OnInspectorGUI(); // draws original inspector
	}
}
