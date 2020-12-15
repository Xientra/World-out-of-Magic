using UnityEngine;
using UnityEditor;

[CustomPreview(typeof(Element))]
public class ElementPreview : ObjectPreview
{
    public override bool HasPreviewGUI()
    {
        return true;
    }

    public override void OnPreviewGUI(Rect r, GUIStyle background)
    {
        base.OnPreviewGUI(r, background);

        Element e = (Element)target;

        //GUI.Label(r, target.name + " is being previewed");
        GUI.Label(r, e.image.texture);
    }
}