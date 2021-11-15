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

	/// <summary>
	/// If a Element has this string as it's category it is regarded as a Sub Element.
	/// This category is also not returned in GetCurrentCategories.
	/// </summary>
	public static readonly string subElementCategoryFlagName = "_";

	public bool loadSaveData = true;

	[Header("General:")]

	public Element originElement;

	//public Recipe[] recipes;

	public Element[] allElements;

	public string[] categories;

	[Header("Progress:")]

	[SerializeField]
	private List<Element> unlockedElements;
	public List<Element> UnlockedElements { get => unlockedElements; }

	public event EventHandler<Element> ElementDiscovered;

	public List<ElementContainer> unlockedElementContainer;

	private void Awake()
	{
		unlockedElements = unlockedElements ?? new List<Element>();
		singelton = this;


		bool loadSuccess = Load();
		if (loadSuccess == false || loadSaveData == false)
			LoadNoSave();

		// create an array of all categories
		List<string> categories = new List<string>();
		for (int i = 0; i < allElements.Length; i++)
			if (!categories.Contains(allElements[i].category))
				categories.Add(allElements[i].category);
		this.categories = categories.ToArray();

		unlockedElementContainer = SetElementFlags(unlockedElements);
	}

	private void Start()
	{
		// updates ui
		OnElementDiscovered(null);
	}

	private void OnDestroy()
	{
		Save();
	}

	public List<ElementContainer> SetElementFlags(List<Element> elements)
	{
		List<ElementContainer> result = new List<ElementContainer>();
		for (int i = 0; i < elements.Count; i++)
		{
			ElementContainer ec = new ElementContainer(elements[i], ElementContainer.ElementCombinationStatus(elements[i]));
			result.Add(ec);
		}

		return result;
	}

	public List<ElementContainer> GetUnlockedElementContainerOfCategory(string category)
	{
		if (string.IsNullOrEmpty(category))
			return unlockedElementContainer;


		List<ElementContainer> r = unlockedElementContainer.FindAll(ec => ec.e.category == category);
		r.Sort((ec1, ec2) => ec1.e.importance - ec2.e.importance);
		return r;
	}

	public HashSet<string> GetCurrentCategories()
	{
		HashSet<string> categories = new HashSet<string>(unlockedElementContainer.ConvertAll<string>(ec => ec.e.category).FindAll(s => s != subElementCategoryFlagName));
		return categories;
	}


	/// <summary>
	/// Returns the element that is created by the combination of e1 and e2. Returns null if no such combination exists.
	/// newCombination will be set to true if the resulting combination has been done for the first time
	/// </summary>
	/// <param name="newCombination"> Will be set to true if the resulting combination has been done for the first time</param>
	public Element CombineElements(Element e1, Element e2, out bool newCombination)
	{
		for (int i = 0; i < allElements.Length; i++)
			for (int j = 0; j < allElements[i].recipes.Length; j++)
			{
				Recipe r = allElements[i].recipes[j];

				if (r.ingredient1 == e1 && r.ingredient2 == e2 || r.ingredient1 == e2 && r.ingredient2 == e1)
				{
					if (!unlockedElements.Contains(allElements[i]))
					{
						DiscoverElement(allElements[i]);
						newCombination = true;
					}
					else
						newCombination = false;

					return allElements[i];
				}
			}

		Debug.Log("No Combination");
		newCombination = false;
		return null;
	}

	private void DiscoverElement(Element e)
	{
		unlockedElements.Add(e);

		ElementContainer newEc = new ElementContainer(e, ElementContainer.ElementCombinationStatus(e));
		newEc.glow = true;
		if (newEc.e.parentElement != null)
			unlockedElementContainer.Find(ec => ec.e == newEc.e.parentElement).glow = true;

		// checks if isDone has to be set on any of the elements that make this element
		for (int i = 0; i < e.recipes.Length; i++)
		{
			unlockedElementContainer.Find(ec => ec.e == e.recipes[i].ingredient1)?.UpdateState();
			unlockedElementContainer.Find(ec => ec.e == e.recipes[i].ingredient2)?.UpdateState();
		}

		if (newEc.element == null)
			Debug.LogError("hat2: " + e.name);

		unlockedElementContainer.Add(newEc);


		// TODO: update sub elements


		// update ui
		OnElementDiscovered(e);

		Save();
	}

	public void UnlockAllElements()
	{
		Debug.Log("All Elements have been unlocked.");

		unlockedElements.Clear();
		unlockedElements.AddRange(allElements);
		unlockedElementContainer = SetElementFlags(unlockedElements);
		OnElementDiscovered(null);
		Save();
	}

	public void SendUIUpdateSignal()
	{
		OnElementDiscovered(null);
	}
	/// <summary> A Observable. UI subscribes to this to update themselfs once new stuff exists. </summary>
	private void OnElementDiscovered(Element e)
	{
		ElementDiscovered?.Invoke(this, e);
	}

	#region static stuff

	/// <summary> O(#combinations) </summary>
	public static List<Recipe> RecipiesWithElement(Element e)
	{
		GameData gd = GameData.singelton != null ? GameData.singelton : GameObject.FindGameObjectWithTag("GameController").GetComponent<GameData>();

		List<Recipe> result = new List<Recipe>();

		for (int i = 0; i < gd.allElements.Length; i++)
			for (int j = 0; j < gd.allElements[i].recipes.Length; j++)
				if (gd.allElements[i].recipes[j].ingredient1 == e || gd.allElements[i].recipes[j].ingredient2 == e)
					result.Add(gd.allElements[i].recipes[j]);

		return result;
	}

	/// <summary> Avoid using at runtime pls. O(#combinations). </summary>
	public static Element ResultOfRecipe(Recipe r)
	{
		GameData gd = GameData.singelton != null ? GameData.singelton : GameObject.FindGameObjectWithTag("GameController").GetComponent<GameData>();

		for (int i = 0; i < gd.allElements.Length; i++)
			for (int j = 0; j < gd.allElements[i].recipes.Length; j++)
				if (gd.allElements[i].recipes[j] == r)
					return gd.allElements[i];

		return null;
	}
	#endregion


	// -========== Save and Load ==========- //
	#region Save and Load

	private void LoadNoSave()
	{
		unlockedElements.Clear();
		unlockedElements.Add(originElement);
		unlockedElementContainer.Add(new ElementContainer(originElement, 0));
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

	#endregion
}
