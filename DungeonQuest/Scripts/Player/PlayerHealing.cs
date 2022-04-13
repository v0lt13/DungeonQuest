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

		[HideInInspector] public int healingPotions;
		private float cooldown;
		
		private PlayerManager playerManager;
		private Text healingPotionsAmount;
		private Slider cooldownSlider;

		void Awake()
		{
			cooldownSlider = GameObject.Find("CooldownSlider").GetComponent<Slider>();
			healingPotionsAmount = GameObject.Find("PotionAmountText").GetComponent<Text>();

			playerManager = GetComponent<PlayerManager>();

			cooldown = defaultCooldown;
			cooldownSlider.maxValue = defaultCooldown;
		}

		void Update()
		{
			cooldownSlider.value = cooldown;
			healingPotionsAmount.text = healingPotions.ToString();

			if (cooldown > 0f)
			{
				cooldown -= Time.deltaTime;
			}

			if (GameManager.INSTANCE.CurrentGameState == GameManager.GameState.Paused) return;

			if (Input.GetButtonDown("Heal") && cooldown <= 0f && healingPotions != 0)
			{
				cooldown = defaultCooldown;
				healingPotions--;

				playerManager.HealPlayer(playerManager.defaultPlayerHealth);
				audioSource.PlayOneShot(healingSFX);
			}
		}

		public void AddPotions(int amount)
		{
			healingPotions += amount;
		}
	}
}
