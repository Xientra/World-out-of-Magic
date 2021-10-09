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

	private string currentCategory = "";


	public event EventHandler<Element> ElementPressed;

	// there should allways be the same number of ElementDisplays exist as there are unlocked elements
	private List<ElementDisplay> elementDisplays;
	private List<CategoryDisplay> categoryDisplay;


	private void Awake()
	{
		sampeElement.gameObject.SetActive(false);
		sampleCategory.gameObject.SetActive(false);

		elementDisplays = new List<ElementDisplay>();
		categoryDisplay = new List<CategoryDisplay>();
	}

	void Start()
	{
		GameData.singelton.ElementDiscovered += GameData_ElementDiscovered;

		UpdateUI();
	}

	private void OnDestroy()
	{
		GameData.singelton.ElementDiscovered -= GameData_ElementDiscovered;
	}

	private void GameData_ElementDiscovered(object sender, Element e)
	{
		UpdateUI();
	}

	private void UpdateUI()
	{
		if (string.IsNullOrEmpty(currentCategory))
			SetCategoryView();
		else
			SetElementView(currentCategory);
	}

	private void SetElementView(string category)
	{
		categoryContent.SetActive(false);
		elementContent.SetActive(true);

		returnButton.gameObject.SetActive(true);

		currentCategory = category;

		DisplayElements(category);
	}
	private void SetCategoryView()
	{
		categoryContent.SetActive(true);
		elementContent.SetActive(false);

		returnButton.gameObject.SetActive(false);

		currentCategory = "";

		DisplayCategories();
	}

	private void DisplayCategories()
	{
		HashSet<string> categories = GameData.singelton.GetCurrentCategories();

		foreach (CategoryDisplay cd in categoryDisplay)
			Destroy(cd.gameObject);

		categoryDisplay.Clear();

		foreach (string c in categories)
		{
			CategoryDisplay cd = Instantiate(sampleCategory, categoryContent.transform).GetComponentInChildren<CategoryDisplay>();
			cd.Text = c;
			categoryDisplay.Add(cd);
			cd.gameObject.SetActive(true);
		}
	}

	private void DisplayElements()
	{
		DisplayElements("");
	}
	private void DisplayElements(string category)
	{
		List<ElementContainer> elementsToDisplay = GameData.singelton.GetUnlockedElementContainerOfCategory(category);


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
				ed.ElementContainer = elementsToDisplay[i];
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

	public void Btn_CategoryPressed(CategoryDisplay source)
	{
		SetElementView(source.Text);
	}

	public void Btn_Return()
	{
		SetCategoryView();
	}
}
