using UnityEngine;

namespace DungeonQuest.Shop
{
	public class Item : ScriptableObject
	{
		public int itemPrice;
		public string itemName;
		public string itemDescription;
		public Sprite itemIcon;

		#if UNITY_EDITOR
		[UnityEditor.MenuItem("ScriptableObjects/NewShopItem")]
		private static void CreateItem()
		{
			var item = ScriptableObject.CreateInstance<Item>();
			UnityEditor.AssetDatabase.CreateAsset(item, "Assets/ShopItems/Item.asset");
		}
		#endif
	}
}
