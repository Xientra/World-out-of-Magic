using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;


[Serializable]
public class ElementContainer
{
	public Element e { get => element; set => element = value; }
	public Element element;

	[Tooltip("True if this element is not part in any combination.")]
	public bool isFinal = false;
	[Tooltip("True if all combination with this element have been exhausted.")]
	public bool isDone = false;
	[Tooltip("True if all remaining combinations with this element yield sub elements.")]
	public bool onlySubElementCombinationsRemaining = false;

	/// <summary>
	/// 0: Element is not done <br/>
	/// 1: Element is final, meaning there are no combinations with this element <br/>
	/// 2: Element is done, meaning all combinations with it have been exhausted <br/>
	/// 3: Element is globaly done, meaning all remaining combinations only result in sub elements <br/>
	/// </summary>
	public int State
	{
		get => isFinal ? 2 : onlySubElementCombinationsRemaining ? 3 : (isDone ? 1 : 0);
		set
		{
			isFinal = value == 2;
			isDone = value == 1;
			onlySubElementCombinationsRemaining = value == 3;
		}
	}

	//[Tooltip("If there are subElements to this element they are referenced here.")]
	//public Element[] unlockedSubElements = new Element[0];

	public ElementContainer(Element element)
	{
		this.element = element;
	}

	public ElementContainer(Element element, int state)
	{
		this.element = element;
		this.State = state;
	}

	public void UpdateState()
	{
		State = ElementCombinationStatus(element);
	}

	public void Clear()
	{
		element = null;
		isFinal = false;
		isDone = false;
		onlySubElementCombinationsRemaining = false;
	}

	/// <summary>
	/// Checks the status of combinations based on the unlocked elements. O(#combinations * #unlockedElements)
	/// </summary>
	/// <returns>
	/// 0: There are still undiscovered elements with this element in the recipe. <br/>
	/// 1: Every combination with this element has been found (opposite of 0). <br/>
	/// 2: This element is not part of any combination to begin with. <br/>
	/// 3: This element still has combination, but all those combinations only yield sub elements. <br/>
	/// </returns>
	public static int ElementCombinationStatus(Element e)
	{
		bool two = true;
		bool zeroOrOne = true; // true means 1
		bool three = true;

		GameData gd = GameData.singelton != null ? GameData.singelton : GameObject.FindGameObjectWithTag("GameController").GetComponent<GameData>();

		for (int i = 0; i < gd.allElements.Length; i++)
			for (int j = 0; j < gd.allElements[i].recipes.Length; j++)
				if (gd.allElements[i].recipes[j].ingredient1 == e || gd.allElements[i].recipes[j].ingredient2 == e)
				{
					if (GameData.singelton.UnlockedElements.Contains(gd.allElements[i]) == false)
					{
						zeroOrOne = false;
						if (gd.allElements[i].parentElement == null)
							three = false;
					}

					two = false;
				}

		return two ? 2 : ((three && zeroOrOne == false ? 3 : (zeroOrOne ? 1 : 0)));
	}
}