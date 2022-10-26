using UnityEngine;
using UnityEngine.UI;

namespace DungeonQuest
{
	public class TipGenerator : MonoBehaviour
	{
		[SerializeField] private Text tipsText;

		private int lastTip = -1;

		private string[] tips = {
			"Dont rush, be strategic",
			"Dying to frequently? Buy some upgrades if possible",
			"Explore as much as you can for a lot of loot",
			"Look for secrets",
			"Need more gold or xp? Go and rebeat some levels",
			"The tougher the enemy the better the loot",
			"Make sure you read the How to Play section",
			"Is fine if you take some hits, you have potions",
			"Triggered a trap? RUN!!!",
			"Some objects are breakeble",
			"Timing is key"
		};

		void OnEnable()
		{
			var tipNumber = Random.Range(0, tips.Length);

			// Makeing sure we dont repeat the same tip the next time
			while (tipNumber == lastTip)
			{
				tipNumber = Random.Range(0, tips.Length);
			}

			lastTip = tipNumber;
			tipsText.text = "TIP: " + tips[tipNumber];			
		}
	}
}
