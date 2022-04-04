using UnityEngine;
using UnityEngine.UI;

namespace DungeonQuest.Player
{
	public class PlayerManager : MonoBehaviour
	{
		[Header("Player Config:")]
		[SerializeField] private int defaultPlayerHealth;
		[SerializeField] private int defaultPlayerArmor;

		[Header("UI Config:")]
		public Slider healthBar;
		public Slider armorBar;
		[SerializeField] private Text coinsAmountText;

		[Header("Audio Config:")]
		[SerializeField] private AudioSource audioSource;
		[SerializeField] private AudioClip deathSFX;

		[HideInInspector] public PlayerMovement playerMovement;
		[HideInInspector] public PlayerLeveling playerLeveling;
		[HideInInspector] public PlayerHealing playerHealing;
		[HideInInspector] public PlayerAttack playerAttack;

		private PlayerFootsteps playerFootsteps;

		public bool Invisible { get; set; }
		public bool IsDead { get; private set; }
		public bool GodMode { private get; set; }

		public int CoinsAmount { get; private set; }
		public int PlayerHealth { get; private set; }
		public int PlayerArmor { get; private set; }

		public int GetDefaultPlayerHealth { get { return defaultPlayerHealth; } }
		public int GetDefaultPlayerArmor { get { return defaultPlayerArmor; } }

		void Awake()
		{
			playerFootsteps = GetComponent<PlayerFootsteps>();
			playerMovement = GetComponent<PlayerMovement>();
			playerLeveling = GetComponent<PlayerLeveling>();
			playerHealing = GetComponent<PlayerHealing>();
			playerAttack = GetComponent<PlayerAttack>();

			PlayerHealth = defaultPlayerHealth;
			PlayerArmor = defaultPlayerArmor;
			healthBar.maxValue = defaultPlayerHealth;
			armorBar.maxValue = defaultPlayerArmor;
		}

		void Update()
		{
			healthBar.value = PlayerHealth;
			armorBar.value = PlayerArmor;
			coinsAmountText.text = CoinsAmount.ToString();

			if (PlayerHealth < 0)
			{
				PlayerHealth = 0;
				Die();
			}
		}

		public void DamagePlayer(int damage)
		{
			if (GodMode) return;

			if (PlayerArmor > 0)
			{
				var absorbedDamage = damage / 4;

				if (PlayerArmor > absorbedDamage)
				{
					PlayerArmor -= absorbedDamage; // Absorbs 25% of the damage
					PlayerHealth -= absorbedDamage * 3; // Only 75% of the damage is decreased from the health
				}
				else
				{
					var remainingDamage = absorbedDamage - PlayerArmor;

					PlayerArmor = 0;
					PlayerHealth -= remainingDamage;
				}
			}
			else if (PlayerHealth > 0)
			{
				PlayerHealth -= damage;
			}
		}

		public void HealPlayer(int amount)
		{
			PlayerHealth += amount;

			if (PlayerHealth > defaultPlayerHealth) PlayerHealth = defaultPlayerHealth;
		}

		public void ArmorPlayer(int amount)
		{
			PlayerArmor += amount;

			if (PlayerArmor > defaultPlayerArmor) PlayerArmor = defaultPlayerArmor;
		}

		public void GiveCoins(int amount)
		{
			CoinsAmount += amount;

			if (CoinsAmount < 0) CoinsAmount = 0;
		}

		public void IncreaseMaxHealth(int amount)
		{
			defaultPlayerHealth += amount;

			PlayerHealth = defaultPlayerHealth;
			healthBar.maxValue = defaultPlayerHealth;
		}

		public void IncreaseMaxArmor(int amount)
		{
			defaultPlayerArmor += amount;

			PlayerArmor = defaultPlayerArmor;
			armorBar.maxValue = defaultPlayerArmor;
		}

		private void Die()
		{
			IsDead = true;
			rigidbody2D.isKinematic = true;

			audioSource.clip = deathSFX;
			audioSource.Play();

			Destroy(collider2D);
			Destroy(playerAttack);
			Destroy(playerHealing);
			Destroy(playerMovement);
			Destroy(playerFootsteps);
			Destroy(this);
		}
	}
}
