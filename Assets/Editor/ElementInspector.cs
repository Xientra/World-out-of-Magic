using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Collections.Generic;

[CustomEditor(typeof(Element)), CanEditMultipleObjects]
public class ElementInspector : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI(); // draws original inspector

		// get the element this inspector draws for:
		Element e = (Element)target;

		// creates image input
		//e.image = (Texture2D)EditorGUILayout.ObjectField("Image", e.image, typeof(Texture2D), false);

		// create category text input
		//e.category = EditorGUILayout.TextField("Category:", e.category);

		// create description text box
		//GUILayout.Label("Description:");
		//e.description = GUILayout.TextArea(e.description, new[] { GUILayout.MaxHeight(48), GUILayout.ExpandHeight(true) });

		if (GUILayout.Button("I this in a recipe?"))
		{
			List<Recipe> rs = GameData.RecipiesWithElement(e);
			Debug.Log(rs.Count == 0 ? ("No \"" + e.name + "\" is not in a recipe.") : ("Yes \"" + e.name + "\" is in a recipe."));
			foreach (Recipe r in rs)
				Debug.Log(r.ingredient1.name + " + " + r.ingredient2.name + " give something.");
		}
	}

	// shows the image in the object preview
	public override Texture2D RenderStaticPreview(string assetPath, UnityEngine.Object[] subAssets, int width, int height)
	{

		Element e = (Element)target;
		if (e.image == null || e.image.texture == null)
			return base.RenderStaticPreview(assetPath, subAssets, width, height);

		Texture2D tex = new Texture2D(width, height);
		EditorUtility.CopySerialized(e.image.texture, tex);
		return tex;
	}

	/*
	public override GUIContent GetPreviewTitle()
	{
		Element e = (Element)target;
		return new GUIContent(e.image);
	}
	*/
}