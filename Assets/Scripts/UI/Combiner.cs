using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Combiner : MonoBehaviour
{
	[Header("Selecter:")]

	public ElementSelecter selecter1;
	public ElementSelecter selecter2;

	[Header("Display:")]

	public ElementDisplay ingredientDisplay1;
	public ElementDisplay ingredientDisplay2;

	[Header("Output:")]

	public ElementDisplay outputDisplay;

	private void Start()
	{
		selecter1.ElementPressed += Selecter_ElementPressed;
		selecter2.ElementPressed += Selecter_ElementPressed;
	}

	private void OnDestroy()
	{
		selecter1.ElementPressed -= Selecter_ElementPressed;
		selecter2.ElementPressed -= Selecter_ElementPressed;
	}

	private void Selecter_ElementPressed(object sender, Element e)
	{
		if ((ElementSelecter)sender == selecter1)
		{
			if (ingredientDisplay1.Element == null)
				ingredientDisplay1.Element = e;
			else if (ingredientDisplay2.Element == null)
				ingredientDisplay2.Element = e;
			else
				ingredientDisplay1.Element = e;
		}
		else
		{
			if (ingredientDisplay2.Element == null)
				ingredientDisplay2.Element = e;
			else if (ingredientDisplay1.Element == null)
				ingredientDisplay1.Element = e;
			else
				ingredientDisplay2.Element = e;
		}
	}


	public void Btn_Combine()
	{
		if (ingredientDisplay1.Element != null && ingredientDisplay2.Element != null)
		{
			bool newDiscovery = false;
			Element e = GameData.singelton.CombineElements(ingredientDisplay1.Element, ingredientDisplay2.Element, out newDiscovery);
			
			if (e != null)
			{
				if (newDiscovery == true)
				{
					AudioManager.singelton.PlayElementDiscoveredSound();

					outputDisplay.Element = e;
					outputDisplay.SetActive(true);

					ingredientDisplay1.Clear();
					ingredientDisplay2.Clear();

				}
				else
				{
					AudioManager.singelton.PlayElementDiscoveredSound();

					outputDisplay.Element = e;
					outputDisplay.SetActive(true);

					ingredientDisplay1.Clear();
					ingredientDisplay2.Clear();
				}
			}
		}
	}


	public void Btn_ClearIngredientDisplay(ElementDisplay source)
	{
		source.Clear();
	}

	public void Btn_CloseOutputDisplay()
	{
		outputDisplay.SetActive(false);
	}
}
