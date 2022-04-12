using UnityEngine;
using UnityEngine.UI;

namespace DungeonQuest.Player
{
	public class PlayerHealing : MonoBehaviour
	{
		[SerializeField] private float defaultCooldown;

		[Header("Audio Config:")]
		[SerializeField] private AudioSource audioSource;
		[SerializeField] private AudioClip healingSFX;

		private PlayerManager playerManager;
		private Text healingPotionsAmount;
		private Slider cooldownSlider;

		public int HealingPotions { get; private set; }
		private float Cooldown { get; set; }

		void Awake()
		{
			cooldownSlider = GameObject.Find("CooldownSlider").GetComponent<Slider>();
			healingPotionsAmount = GameObject.Find("PotionAmountText").GetComponent<Text>();

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

			if (GameManager.INSTANCE.CurrentGameState == GameManager.GameState.Paused) return;

			if (Input.GetButtonDown("Heal") && Cooldown <= 0f && HealingPotions != 0)
			{
				Cooldown = defaultCooldown;
				HealingPotions--;

				playerManager.HealPlayer(playerManager.GetDefaultPlayerHealth);
				audioSource.PlayOneShot(healingSFX);
			}
		}

		public void AddPotions(int amount)
		{
			HealingPotions += amount;
		}
	}
}
