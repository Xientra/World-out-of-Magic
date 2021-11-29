using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(RectTransform))]
public class ElementNameTooltip : MonoBehaviour
{
	public static ElementNameTooltip singelton;

	public TMP_Text label;

	[Space(5)]

	public float verticalSpace = 25;

	private Camera cam;

	private bool active = false;

	private RectTransform hoverOver;

	private void Awake()
	{
		if (singelton == null)
			singelton = this;
		else
			Debug.LogError("There was more than one HoverDisplay in the Scene!");
	}

	private void Start()
	{
		cam = Camera.main;
		gameObject.SetActive(false);
	}

	public void Display(string text, RectTransform hoverOver)
	{
		label.text = text;
		this.hoverOver = hoverOver;
		SetPosition();

		active = true;

		gameObject.SetActive(true);

		// force update the content size fitter for the new text
		LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)transform);
	}

	public void Hide()
	{
		gameObject.SetActive(false);
		active = false;
	}

	private void Update()
	{
		if (active)
			SetPosition();
	}

	private void SetPosition()
	{
		Vector3 newPos = cam.ScreenToWorldPoint(Input.mousePosition);
		newPos.z = 0;

		RectTransform trans = ((RectTransform)transform);

		// position under whatever hovering over
		if (newPos.y + trans.sizeDelta.y / 2 > hoverOver.position.y - hoverOver.sizeDelta.y / 2 - verticalSpace)
			newPos.y = hoverOver.position.y - hoverOver.sizeDelta.y / 2 - trans.sizeDelta.y / 2 - verticalSpace;

		// clamp tranform to screen size (or rather the direct parent, which is the canvas)
		RectTransform parentTransform = ((RectTransform)transform.parent.transform);
		if (newPos.x + trans.sizeDelta.x / 2 > parentTransform.position.x + parentTransform.sizeDelta.x / 2)
			newPos.x = parentTransform.position.x + parentTransform.sizeDelta.x / 2 - trans.sizeDelta.x / 2;
		if (newPos.x - trans.sizeDelta.x / 2 < parentTransform.position.x - parentTransform.sizeDelta.x / 2)
			newPos.x = parentTransform.position.x - parentTransform.sizeDelta.x / 2 + trans.sizeDelta.x / 2;

		// if the tranform is clipping throught the screen at the bottom, put the tooltip on top instead
		if (newPos.y - trans.sizeDelta.y / 2 < parentTransform.position.y - parentTransform.sizeDelta.y / 2)
			newPos.y = hoverOver.position.y + hoverOver.sizeDelta.y / 2 + trans.sizeDelta.y / 2 + verticalSpace;

		transform.position = newPos;
	}
}
