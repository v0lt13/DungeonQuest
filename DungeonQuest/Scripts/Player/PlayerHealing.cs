using UnityEngine;
using UnityEngine.UI;

namespace DungeonQuest.Player
{
	public class PlayerHealing : MonoBehaviour
	{
		[SerializeField] private float defaultCooldown;

		[SerializeField] private AudioClip healingSFX;
		[SerializeField] private AudioClip noHealingSFX;

		[HideInInspector] public int healingPotions;
		public const int POTION_CAP = 15;

		private float cooldown = 0f;

		private PlayerManager playerManager;
		private Animator noHealinganim;
		private Text healingPotionsAmount;
		private Slider cooldownSlider;

		void Awake()
		{
			cooldownSlider = GameObject.Find("CooldownSlider").GetComponent<Slider>();
			healingPotionsAmount = GameObject.Find("PotionAmountText").GetComponent<Text>();
			noHealinganim = GameObject.Find("HealIconHighlight").GetComponent<Animator>();

			playerManager = GetComponent<PlayerManager>();

			cooldownSlider.maxValue = defaultCooldown;
		}

		void Update()
		{
			cooldownSlider.value = cooldown;
			healingPotionsAmount.text = healingPotions.ToString();

			if (cooldown > 0f) cooldown -= Time.deltaTime;

			if (healingPotions > POTION_CAP) healingPotions = POTION_CAP;

			if (GameManager.INSTANCE.CurrentGameState == GameManager.GameState.Paused) return;

			if (Input.GetButtonDown("Heal"))
			{
				if (cooldown <= 0f && healingPotions != 0)
				{
					cooldown = defaultCooldown;
					playerManager.audioSource2D.pitch = 1f;
					healingPotions--;

					playerManager.HealPlayer(playerManager.defaultPlayerHealth);
					playerManager.audioSource2D.PlayOneShot(healingSFX);
				}
				else
				{
					noHealinganim.Play("NoHealing", -1, 0f);

					playerManager.audioSource2D.PlayOneShot(noHealingSFX);
				}
			}
		}

		public void AddPotions(int amount)
		{
			healingPotions += amount;
		}
	}
}
