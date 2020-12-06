using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameData : MonoBehaviour
{
	public static GameData singelton;

	[Header("General:")]

	public Element originElement;

	public Recipe[] recipes;

	public Element[] elements;

	[Header("Progress:")]

	[SerializeField]
	private List<Element> unlockedElements;
	public List<Element> UnlockedElements { get => unlockedElements; }

	public event EventHandler<Element> ElementDiscovered;

	private void Awake()
	{
		singelton = this;

		/*
		if (unlockedElements == null)
			unlockedElements = new List<Element>();

		unlockedElements.Add(originElement);
		*/
	}


	public List<Element> GetCategory(string category)
	{
		return unlockedElements.FindAll(e => e.category == category);
	}


	public void Sort()
	{
		unlockedElements.Sort((e1, e2) => e1.importance - e2.importance);
	}


	public Element CombineElements(Element e1, Element e2)
	{
		for (int i = 0; i < recipes.Length; i++)
		{
			Recipe r = recipes[i];
			if (r != null)
			{
				if (r.ingredient1 == e1 && r.ingredient2 == e2 || r.ingredient1 == e2 && r.ingredient2 == e1)
				{
					if (!unlockedElements.Contains(r.result))
						DiscoverElement(r.result);
					return r.result;
				}
			}
		}

		Debug.Log("No Combination");
		return null;
	}

	private void DiscoverElement(Element e)
	{
		unlockedElements.Add(e);

		// update ui
		OnElementDiscovered(e);

		// some animation
		Debug.Log("Created " + e);
	}

	private void OnElementDiscovered(Element e)
	{
		ElementDiscovered?.Invoke(this, e);
	}

	// -========== Save and Load ==========- //

	private void Start()
	{
		//Load();
	}

	private void OnDestroy()
	{
		//Save();
	}


	private void Save()
	{
		BinaryFormatter formatter = new BinaryFormatter();
		string path = Application.persistentDataPath + "\\WorldOutOfMagicSaveData.woom";

		FileStream stream = new FileStream(path, FileMode.Create);

		SaveData charData = new SaveData(unlockedElements);

		formatter.Serialize(stream, charData);
		stream.Close();
	}

	private void Load()
	{
		string path = Application.persistentDataPath + "\\WorldOutOfMagicSaveData.woom";

		if (File.Exists(path))
		{
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream stream = new FileStream(path, FileMode.Open);

			SaveData data = formatter.Deserialize(stream) as SaveData;

			stream.Close();

			unlockedElements = data.unlockedElements;
		}
		else
		{
			Debug.LogError("Error: Save file not found in " + path);
		}
	}
}

[System.Serializable]
public class SaveData
{
	public List<Element> unlockedElements;
	public SaveData(List<Element> unlockedElements)
	{
		this.unlockedElements = unlockedElements;
	}
}
