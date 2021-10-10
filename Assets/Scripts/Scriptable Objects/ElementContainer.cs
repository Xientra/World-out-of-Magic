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

	public int State
	{
		get => isFinal ? 2 : (isDone ? 1 : 0);
		set
		{
			isFinal = value == 2;
			isDone = value == 1;
		}
	}

	[Tooltip("If there are subElements to this element they are referenced here.")]
	public Element[] unlockedSubElements = new Element[0];

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
		State = GameData.ElementCombinationStatus(element);
	}

	public void Clear() {
		element = null;
		isFinal = false;
		isDone = false;
	}
}