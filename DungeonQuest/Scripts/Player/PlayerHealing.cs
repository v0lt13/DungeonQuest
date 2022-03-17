using UnityEngine;
using UnityEngine.UI;

namespace DungeonQuest.Player
{
	public class PlayerHealing : MonoBehaviour
	{
		[Header ("Healing Config;")]
		[SerializeField] private float defaultCooldown;
		[Space(10f)]
		[SerializeField] private Slider cooldownSlider;
		[SerializeField] private Text healingPotionsAmount;

		private PlayerManager playerManager;

		public int HealingPotions { get; set; }
		public float Cooldown { get; private set; }

		void Awake()
		{
			playerManager = GetComponent<PlayerManager>();

			Cooldown = defaultCooldown;
			cooldownSlider.maxValue = defaultCooldown;
		}

		void Update()
		{

			cooldownSlider.value = Cooldown;
			healingPotionsAmount.text = HealingPotions.ToString();

			if (Cooldown > 0f)
			{
				Cooldown -= Time.deltaTime;
			}

			if (Input.GetButtonDown("Heal") && Cooldown <= 0f && HealingPotions != 0 && !playerManager.IsDead)
			{
				Cooldown = defaultCooldown;
				HealingPotions--;

				playerManager.HealPlayer(playerManager.defaultPlayerHealth);
			}
		}
	}
}
