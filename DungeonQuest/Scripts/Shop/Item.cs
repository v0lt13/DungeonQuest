using UnityEngine;

namespace DungeonQuest.Shop
{
	[CreateAssetMenu(fileName = "ShopItem", menuName = "ScriptableObjects/ShopItem", order = 1)]
	public class Item : ScriptableObject
	{
		public int itemPrice;
		public string itemName;
		public string itemDescription;
		public Sprite itemIcon;
	}
}
