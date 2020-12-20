using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameData : MonoBehaviour
{
	public static GameData singelton;

	//public static readonly string saveFileFolder = "Data";
	public static readonly string saveFileName = "SaveData";
	public static readonly string saveFileExtension = ".woom";


	[Header("General:")]

	public Element originElement;

	public Recipe[] recipes;

	public Element[] allElements;

	public string[] categories;

	[Header("Progress:")]

	[SerializeField]
	private List<Element> unlockedElements;
	public List<Element> UnlockedElements { get => unlockedElements; }

	public event EventHandler<Element> ElementDiscovered;


	private void Awake()
	{
		unlockedElements = unlockedElements ?? new List<Element>();
		singelton = this;

		bool loadSuccess = Load();
		if (loadSuccess == false)
			LoadNoSave();

		OnElementDiscovered(null);


		// create an array of all categories
		List<string> categories = new List<string>();
		for (int i = 0; i < allElements.Length; i++)
			if (!categories.Contains(allElements[i].category))
				categories.Add(allElements[i].category);
		this.categories = categories.ToArray();
	}

	private void OnDestroy()
	{
		Save();
	}


	public List<Element> GetCategory(string category)
	{
		if (string.IsNullOrEmpty(category))
			return UnlockedElements;

		List<Element> r = unlockedElements.FindAll(e => e.category == category);
		r.Sort((e1, e2) => e1.importance - e2.importance);
		return r;
	}

	public HashSet<string> GetCurrentCategories()
	{
		HashSet<string> categories = new HashSet<string>(unlockedElements.ConvertAll<string>(e => e.category));
		return categories;
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
	#region Save and Load

	private void LoadNoSave()
	{
		unlockedElements.Clear();
		unlockedElements.Add(originElement);
		OnElementDiscovered(originElement);
	}

	private void Save()
	{
		//string path = Application.dataPath + Path.DirectorySeparatorChar + saveFileFolder + Path.DirectorySeparatorChar + saveFileName + saveFileExtension;
		//string path = Application.dataPath + "/" + saveFileFolder + "/" + saveFileName + saveFileExtension;
		string path = Application.dataPath + "/" + saveFileName + saveFileExtension;


		// get all IDs from the unlocked elements
		int i = 0;
		string[] toSaveIDs = new string[unlockedElements.Count];
		string[] toSaveNamess = new string[unlockedElements.Count];
		foreach (Element e in unlockedElements)
		{
			if (e != null)
			{
				toSaveNamess[i] = e.name;
				toSaveIDs[i] = e.ID;
			}
			i++;
		}

		SaveData charData = new SaveData(toSaveIDs, toSaveNamess);

		BinaryFormatter formatter = new BinaryFormatter();
		FileStream stream = new FileStream(path, FileMode.Create);
		formatter.Serialize(stream, charData);
		stream.Close();
	}

	private bool Load()
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


			unlockedElements.Clear();

			for (int i = 0; i < data.unlockedElementIDs.Length; i++)
			{
				Element e = Array.FindLast<Element>(allElements, el => el.ID == data.unlockedElementIDs[i]);
				if (e != null)
					unlockedElements.Add(e);
				//Debug.Log("Load: " + data.unlockedElementNames[i] + " with ID: " + data.unlockedElementIDs[i]);
			}
		}
		else
		{
			Debug.LogWarning("No Save file found in: " + path + "\nLoading default start.");
			return false;
		}

		return true;
	}
}

[System.Serializable]
public class SaveData
{
	public string[] unlockedElementIDs;
	public string[] unlockedElementNames;
	public SaveData(string[] unlockedElementIDs, string[] unlockedElementNames)
	{
		this.unlockedElementIDs = unlockedElementIDs;
		this.unlockedElementNames = unlockedElementNames;
	}
}

#endregion