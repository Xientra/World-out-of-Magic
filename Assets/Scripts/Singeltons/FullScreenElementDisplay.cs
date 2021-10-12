using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullScreenElementDisplay : MonoBehaviour
{
	public static FullScreenElementDisplay singelton;

	public ElementDisplay elementDisplay;

	private ElementContainer previousElementContainer = null;

	private void Awake()
	{
		singelton = this;
	}

	public void Display(ElementContainer ec)
	{
		if (elementDisplay.isActiveAndEnabled)
			previousElementContainer = elementDisplay.ElementContainer;

		elementDisplay.ElementContainer = ec;
		elementDisplay.SetActive(true);
	}

	public void Display(Element e)
	{
		Display(new ElementContainer(e));
	}

	public void Btn_Close()
	{
		if (previousElementContainer != null && previousElementContainer.element != null)
			elementDisplay.ElementContainer = previousElementContainer;
		else
			elementDisplay.SetActive(false);

		previousElementContainer = null;
	}
}
