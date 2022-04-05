using UnityEngine;
using UnityEngine.UI;

namespace DungeonQuest.Player
{
	public class PlayerHealing : MonoBehaviour
	{
		[Header ("Healing Config;")]
		[SerializeField] private float defaultCooldown;

		[Header("UI Config:")]
		[SerializeField] private Slider cooldownSlider;
		[SerializeField] private Text healingPotionsAmount;

		[Header("Audio Config:")]
		[SerializeField] private AudioSource audioSource;
		[SerializeField] private AudioClip healingSFX;

		private PlayerManager playerManager;

		public int HealingPotions { get; private set; }
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
