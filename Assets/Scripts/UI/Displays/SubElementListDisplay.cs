using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubElementListDisplay : MonoBehaviour
{
	[SerializeField]
	private Transform parentTransform;

	[SerializeField]
	private ElementDisplay elementDisplaySample;

	[SerializeField]
	private List<ElementDisplay> elementDisplays;

	[Space(5)]
	[SerializeField]
	public GameObject[] setActiveIfNotEmpty;

	private void Start()
	{
		elementDisplaySample.SetActive(false);
	}

	public void SetRecipes(Element[] elements)
	{
		Clear();
		if (elements == null)
			return;

		SetActiveIfNotEmpty(elements.Length > 0);

		for (int i = 0; i < elements.Length; i++)
		{
			ElementDisplay rd = Instantiate(elementDisplaySample, parentTransform != null ? parentTransform : transform).GetComponent<ElementDisplay>();
			rd.Element = elements[i];
			rd.gameObject.SetActive(true);
			elementDisplays.Add(rd);
		}
	}

	private void Clear()
	{
		foreach (ElementDisplay rd in elementDisplays)
			Destroy(rd.gameObject);

		elementDisplays.Clear();

		SetActiveIfNotEmpty(false);
	}

	private void SetActiveIfNotEmpty(bool value)
	{
		foreach (GameObject go in setActiveIfNotEmpty)
			go.SetActive(value);
	}
}
