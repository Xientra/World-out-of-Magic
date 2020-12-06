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

	public Element[] allElements;

	[Header("Progress:")]

	[SerializeField]
	private List<Element> unlockedElements;
	public List<Element> UnlockedElements { get => unlockedElements; }

	public event EventHandler<Element> ElementDiscovered;

	public static readonly string saveFileFolder = "Data";
	public static readonly string saveFileName = "SaveData";
	public static readonly string saveFileExtension = ".woom";

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

		Save();

		// some animation
		Debug.Log("Created " + e);
	}

	private void OnElementDiscovered(Element e)
	{
		ElementDiscovered?.Invoke(this, e);
	}

	// -========== Save and Load ==========- //

	private void Awake()
	{
		singelton = this;

		Load();
	}

	private void OnDestroy()
	{
		Save();
	}


	private void Save()
	{
		//string path = Application.dataPath + Path.DirectorySeparatorChar + saveFileFolder + Path.DirectorySeparatorChar + saveFileName + saveFileExtension;
		//string path = Application.dataPath + "/" + saveFileFolder + "/" + saveFileName + saveFileExtension;
		string path = Application.dataPath + "/" + saveFileName + saveFileExtension;


		// get all IDs from the unlocked elements
		int i = 0;
		string[] toSaveIDs = new string[unlockedElements.Count];
		foreach (Element e in unlockedElements)
		{
			if (e != null)
				toSaveIDs[i] = e.ID;
			i++;
		}

		SaveData charData = new SaveData(toSaveIDs);

		BinaryFormatter formatter = new BinaryFormatter();
		FileStream stream = new FileStream(path, FileMode.Create);
		formatter.Serialize(stream, charData);
		stream.Close();
	}

	private void Load()
	{
		//string path = Application.dataPath + Path.DirectorySeparatorChar + saveFileFolder + Path.DirectorySeparatorChar + saveFileName + saveFileExtension;
		//string path = Application.dataPath + "/" + saveFileFolder + "/" + saveFileName + saveFileExtension;
		string path = Application.dataPath + "/" + saveFileName + saveFileExtension;

		if (File.Exists(path))
		{
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream stream = new FileStream(path, FileMode.Open);

			SaveData data = formatter.Deserialize(stream) as SaveData;

			stream.Close();


			string[] loadedIDs = data.unlockedElementIDs;

			unlockedElements.Clear();

			for (int i = 0; i < loadedIDs.Length; i++)
				unlockedElements.Add(Array.FindLast<Element>(allElements, e => e.ID == loadedIDs[i]));
		}
		else
		{
			Debug.LogWarning("No Save file found in: " + path);
		}
	}
}

[System.Serializable]
public class SaveData
{
	public string[] unlockedElementIDs;
	public SaveData(string[] unlockedElementIDs)
	{
		this.unlockedElementIDs = unlockedElementIDs;
	}
}
