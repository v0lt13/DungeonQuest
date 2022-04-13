using UnityEngine;
using UnityEngine.UI;
using DungeonQuest.Data;

namespace DungeonQuest.Player
{
	public class PlayerManager : MonoBehaviour
	{
		[Header("Player Config:")]
		public int defaultPlayerHealth;
		public int defaultPlayerArmor;

		[Header("Audio Config:")]
		[SerializeField] private AudioSource audioSource;
		[SerializeField] private AudioClip deathSFX;

		[HideInInspector] public int playerHealth = 100;
		[HideInInspector] public int playerArmor = 100;
		[HideInInspector] public int coinsAmount;
		[HideInInspector] public bool invisible;
		[HideInInspector] public bool noClip;

		[HideInInspector] public PlayerMovement playerMovement;
		[HideInInspector] public PlayerLeveling playerLeveling;
		[HideInInspector] public PlayerHealing playerHealing;
		[HideInInspector] public PlayerAttack playerAttack;

		[HideInInspector] public Slider healthBar;
		[HideInInspector] public Slider armorBar;

		private PlayerFootsteps playerFootsteps;
		private SpriteRenderer spriteRenderer;
		private Text coinsAmountText;

		public bool IsDead { get; private set; }
		public bool GodMode { private get; set; }

		void Awake()
		{
			healthBar = GameObject.Find("PlayerHealthBar").GetComponent<Slider>();
			armorBar = GameObject.Find("PlayerArmorBar").GetComponent<Slider>();
			coinsAmountText = GameObject.Find("CoinsAmountText").GetComponent<Text>();

			playerFootsteps = GetComponent<PlayerFootsteps>();
			spriteRenderer = GetComponent<SpriteRenderer>();
			playerMovement = GetComponent<PlayerMovement>();
			playerLeveling = GetComponent<PlayerLeveling>();
			playerHealing = GetComponent<PlayerHealing>();
			playerAttack = GetComponent<PlayerAttack>();
		}

		void Start()
		{
			GameManager.INSTANCE.gameData.LoadPlayerData();

			healthBar.maxValue = defaultPlayerHealth;
			armorBar.maxValue = defaultPlayerArmor;
		}

		void Update()
		{
			healthBar.value = playerHealth;
			armorBar.value = playerArmor;
			coinsAmountText.text = coinsAmount.ToString();
			collider2D.enabled = !noClip;

			if (invisible || noClip)
			{
				spriteRenderer.color = new Color(255f, 255f, 255f, 0.5f);
			}
			else
			{
				spriteRenderer.color = new Color(255f, 255f, 255f, 1f);
			}

			if (playerHealth < 0)
			{
				playerHealth = 0;
				Die();
			}	
		}

		public void DamagePlayer(int damage)
		{
			if (GodMode) return;

			if (playerArmor > 0)
			{
				var absorbedDamage = damage / 4;

				if (playerArmor > absorbedDamage)
				{
					playerArmor -= absorbedDamage; // Absorbs 25% of the damage
					playerHealth -= absorbedDamage * 3; // Only 75% of the damage is decreased from the health
				}
				else
				{
					var remainingDamage = absorbedDamage - playerArmor;

					playerArmor = 0;
					playerHealth -= remainingDamage;
				}
			}
			else if (playerHealth > 0)
			{
				playerHealth -= damage;
			}
		}

		public void HealPlayer(int amount)
		{
			playerHealth += amount;

			if (playerHealth > defaultPlayerHealth) playerHealth = defaultPlayerHealth;
		}

		public void ArmorPlayer(int amount)
		{
			playerArmor += amount;

			if (playerArmor > defaultPlayerArmor) playerArmor = defaultPlayerArmor;
		}

		public void GiveCoins(int amount)
		{
			coinsAmount += amount;

			if (coinsAmount < 0) coinsAmount = 0;
		}

		public void IncreaseMaxHealth(int amount)
		{
			defaultPlayerHealth += amount;

			playerHealth = defaultPlayerHealth;
			healthBar.maxValue = defaultPlayerHealth;
		}

		public void IncreaseMaxArmor(int amount)
		{
			defaultPlayerArmor += amount;

			playerArmor = defaultPlayerArmor;
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
