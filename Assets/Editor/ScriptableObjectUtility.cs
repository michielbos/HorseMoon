using UnityEngine;
using UnityEditor;
using System.IO;
using HorseMoon.Inventory.ItemTypes;
using HorseMoon.Speech;

public static class ScriptableObjectUtility
{
	[MenuItem ("Assets/Create/Item/Food")]
	public static void CreateFoodInfo () {
		CreateAsset <FoodInfo> ();
	}

	[MenuItem("Assets/Create/Item/Seed")]
	public static void CreateSeedInfo() {
		CreateAsset<FoodInfo>();
	}

	[MenuItem("Assets/Create/Item/Tool")]
	public static void CreateToolInfo() {
		CreateAsset<ToolInfo>();
	}

	[MenuItem("Assets/Create/SpeechCharacterSprite")]
	public static void CreateSpeechCharacterSprite() {
		CreateAsset<SpeechCharacterData>();
	}

	/// <summary>
	///	This makes it easy to create, name and place unique new ScriptableObject asset files.
	/// </summary>
	public static void CreateAsset<T> () where T : ScriptableObject
	{
		T asset = ScriptableObject.CreateInstance<T> ();
 
		string path = AssetDatabase.GetAssetPath (Selection.activeObject);
		if (path == "") 
		{
			path = "Assets";
		} 
		else if (Path.GetExtension (path) != "") 
		{
			path = path.Replace (Path.GetFileName (AssetDatabase.GetAssetPath (Selection.activeObject)), "");
		}
 
		string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath (path + "/New " + typeof(T).ToString() + ".asset");
 
		AssetDatabase.CreateAsset (asset, assetPathAndName);
 
		AssetDatabase.SaveAssets ();
        	AssetDatabase.Refresh();
		EditorUtility.FocusProjectWindow ();
		Selection.activeObject = asset;
	}
}