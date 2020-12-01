using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementSelecter : MonoBehaviour
{
	public GameObject content;

	public ElementDisplay sampeElement;

	public event EventHandler<Element> ElementPressed;

	private void Awake()
	{
		sampeElement.gameObject.SetActive(false);
	}

	void Start()
	{
		ClearContent();
		CreateElementDisplays();

		GameData.singelton.ElementDiscovered += GameData_ElementDiscovered;
	}

	private void OnDestroy()
	{
		GameData.singelton.ElementDiscovered -= GameData_ElementDiscovered;
	}

	private void GameData_ElementDiscovered(object sender, Element e)
	{
		CreateElementDisplay(e);
	}

	private void ClearContent()
	{
		foreach (Transform child in content.transform)
			if (child.gameObject.activeSelf == true)
				Destroy(child.gameObject);
	}

	private void CreateElementDisplays()
	{
		List<Element> uElements = GameData.singelton.UnlockedElements;

		foreach (Element e in uElements)
			CreateElementDisplay(e);
	}

	private void CreateElementDisplay(Element e)
	{
		ElementDisplay ed = Instantiate(sampeElement, content.transform);
		ed.Element = e;
		ed.gameObject.SetActive(true);
	}

	public void Btn_ElementPressed(ElementDisplay source)
	{
		OnElementPressed(source.Element);
	}

	private void OnElementPressed(Element e)
	{
		ElementPressed?.Invoke(this, e);
	}
}
