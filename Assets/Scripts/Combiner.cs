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

	[Space(5)]

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
			if (ingredientDisplay1.Element != e)
				ingredientDisplay1.Element = e;
			else
				ingredientDisplay2.Element = e;
		}
		else
		{
			if (ingredientDisplay2.Element != e)
				ingredientDisplay2.Element = e;
			else
				ingredientDisplay1.Element = e;
		}
	}



	public void Btn_Combine()
	{
		if (ingredientDisplay1.Element != null && ingredientDisplay2.Element != null)
		{
			Element e = GameData.singelton.CombineElements(ingredientDisplay1.Element, ingredientDisplay2.Element);
			if (e != null)
			{
				outputDisplay.Element = e;
				ingredientDisplay1.Clear();
				ingredientDisplay2.Clear();
			}
		}
	}

	public void Btn_ClearIngredientDisplay(ElementDisplay source)
	{
		source.Clear();
	}
}
