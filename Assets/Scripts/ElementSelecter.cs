using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ElementSelecter : MonoBehaviour
{
	[Header("Content Objects")]

	public GameObject elementContent;
	public GameObject categoryContent;

	[Header("Sample Objects")]

	public ElementDisplay sampeElement;
	public GameObject sampleCategory;

	[Header("Other")]

	public Button returnButton;

	private bool elementView = false;


	public event EventHandler<Element> ElementPressed;

	// there should allways be the same number of ElementDisplays exist as there are unlocked elements
	private List<ElementDisplay> elementDisplays;
	private List<TMP_Text> categoryButtons;


	private void Awake()
	{
		sampeElement.gameObject.SetActive(false);
		sampleCategory.gameObject.SetActive(false);

		elementDisplays = new List<ElementDisplay>();
		categoryButtons = new List<TMP_Text>();
	}

	void Start()
	{
		GameData.singelton.ElementDiscovered += GameData_ElementDiscovered;

		//DisplayElements();

		SetElementView(false);
		DisplayCategories();
	}

	private void OnDestroy()
	{
		GameData.singelton.ElementDiscovered -= GameData_ElementDiscovered;
	}

	private void GameData_ElementDiscovered(object sender, Element e)
	{
		DisplayElements();
	}

	private void UpdateUI()
	{
		DisplayElements();
	}

	private void SetElementView(bool value)
	{
		categoryContent.SetActive(!value);
		elementContent.SetActive(value);

		returnButton.gameObject.SetActive(value);

		elementView = value;
	}

	private void DisplayCategories()
	{
		HashSet<string> categories = GameData.singelton.GetCurrentCategories();

		foreach (TMP_Text c in categoryButtons)
			Destroy(c.gameObject);

		foreach (string c in categories)
		{
			TMP_Text ct = Instantiate(sampleCategory, categoryContent.transform).GetComponentInChildren<TMP_Text>();
			ct.text = c;
			ct.transform.parent.gameObject.name = c + " Category";
			categoryButtons.Add(ct);
			ct.transform.parent.gameObject.SetActive(true);
		}

		/*
		// create more category objects if there are not enought
		for (int i = categoryButtons.Count; i < categories.Count; i++)
		{
			TMP_Text ct = Instantiate(sampleCategory.gameObject, elementContent.transform).GetComponent<TMP_Text>();
			ct.gameObject.SetActive(false);
			categoryButtons.Add(ct);
		}

		
		int j = 0;
		// display categories
		foreach (string c in categories)
		{
			if (j < categories.Count) ;
			else ;

			j++;
		}
		*/
	}

	private void DisplayElements()
	{
		DisplayElements(GameData.singelton.UnlockedElements);
	}
	private void DisplayElements(List<Element> elementsToDisplay)
	{
		SetElementView(true);

		// create more Element Displays if some are missing
		for (int i = elementDisplays.Count; i < elementsToDisplay.Count; i++)
		{
			ElementDisplay ed = Instantiate(sampeElement.gameObject, elementContent.transform).GetComponent<ElementDisplay>();
			ed.gameObject.SetActive(false);
			elementDisplays.Add(ed);
		}

		// set all Element Displays. If some are not needed turn them off
		for (int i = 0; i < elementDisplays.Count; i++)
		{
			ElementDisplay ed = elementDisplays[i];

			if (i < elementsToDisplay.Count)
			{
				ed.Element = elementsToDisplay[i];
				ed.gameObject.SetActive(true);
			}
			else
			{
				ed.Element = null;
				ed.SetActive(false);
			}
		}
	}


	// -========== UI Methods ==========- //

	public void Btn_ElementPressed(ElementDisplay source)
	{
		OnElementPressed(source.Element);
	}

	private void OnElementPressed(Element e)
	{
		ElementPressed?.Invoke(this, e);
	}

	public void Btn_CategoryPressed(TMP_Text source)
	{
		DisplayElements(GameData.singelton.GetCategory(source.text));
	}

	public void Btn_Return()
	{
		SetElementView(false);
	}
}
