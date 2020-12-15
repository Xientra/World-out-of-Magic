using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementSelecter : MonoBehaviour
{
	public GameObject content;

	public ElementDisplay sampeElement;

	public event EventHandler<Element> ElementPressed;

	// there should allways be the same number of ElementDisplays exist as there are unlocked elements
	private List<ElementDisplay> elementDisplays;

	private void Awake()
	{
		sampeElement.gameObject.SetActive(false);

		elementDisplays = new List<ElementDisplay>();
	}

	void Start()
	{
		SetElementDisplays();

		GameData.singelton.ElementDiscovered += GameData_ElementDiscovered;
	}

	private void OnDestroy()
	{
		GameData.singelton.ElementDiscovered -= GameData_ElementDiscovered;
	}

	private void GameData_ElementDiscovered(object sender, Element e)
	{
		SetElementDisplays();
	}

	private void UpdateUI()
	{
		SetElementDisplays();
	}

	private void SetElementDisplays()
	{
		List<Element> uElements = GameData.singelton.UnlockedElements;

		// create more Element Displays if some are missing
		for (int i = elementDisplays.Count; i < uElements.Count; i++)
		{
			ElementDisplay ed = Instantiate(sampeElement.gameObject, content.transform).GetComponent<ElementDisplay>();
			ed.gameObject.SetActive(false);
			elementDisplays.Add(ed);
		}

		// set all Element Displays. If some are not needed turn them off
		for (int i = 0; i < elementDisplays.Count; i++)
		{
			ElementDisplay ed = elementDisplays[i];

			if (i < uElements.Count)
			{
				ed.Element = uElements[i];
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
}
