using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullScreenElementDisplay : MonoBehaviour
{
	public static FullScreenElementDisplay singelton;

	public bool darkmode = false;

	public ElementDisplay lightmodeDisplay;
	public ElementDisplay darkmodeDisplay;

	private void Awake()
	{
		singelton = this;
	}

	public void Display(Element e)
	{
		if (darkmode)
			darkmodeDisplay.Element = e;
		else
			lightmodeDisplay.Element = e;

		UpdateUI();
	}

	private void UpdateUI()
	{
		if (darkmode)
		{
			darkmodeDisplay?.SetActive(true);
			lightmodeDisplay?.SetActive(false);
		}
		else
		{
			lightmodeDisplay?.SetActive(true);
			darkmodeDisplay?.SetActive(false);
		}
	}

	public void SetDarkmode(bool value)
	{
		darkmode = value;
		UpdateUI();
	}

	public void Btn_Close()
	{
		lightmodeDisplay?.SetActive(false);
		darkmodeDisplay?.SetActive(false);
	}
}
