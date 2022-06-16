using UnityEngine;
using UnityEngine.UI;

namespace DungeonQuest.Player
{
	public class PlayerHealing : MonoBehaviour
	{
		[SerializeField] private float defaultCooldown;

		[SerializeField] private AudioClip healingSFX;

		[HideInInspector] public int healingPotions;
		private float cooldown = 0f;
		
		private PlayerManager playerManager;
		private Text healingPotionsAmount;
		private Slider cooldownSlider;

		void Awake()
		{
			cooldownSlider = GameObject.Find("CooldownSlider").GetComponent<Slider>();
			healingPotionsAmount = GameObject.Find("PotionAmountText").GetComponent<Text>();

			playerManager = GetComponent<PlayerManager>();

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
				audio.pitch = 1f;
				healingPotions--;

				playerManager.HealPlayer(playerManager.defaultPlayerHealth);
				audio.PlayOneShot(healingSFX);
			}
		}

		public void AddPotions(int amount)
		{
			healingPotions += amount;
		}
	}
}
