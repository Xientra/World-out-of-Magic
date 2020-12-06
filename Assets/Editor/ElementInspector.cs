using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;

[CustomEditor(typeof(Element))]
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