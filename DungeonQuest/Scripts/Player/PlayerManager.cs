using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace DungeonQuest.Player
{
	public class PlayerManager : MonoBehaviour
	{
		[Header("Player Config:")]
		public int defaultPlayerHealth;
		public int defaultPlayerArmor;
		public int lifeCount;
		[Space(10f)]
		[SerializeField] private Text lifeCountText;

		[Header("Audio Config:")]
		[SerializeField] private AudioClip deathSFX;
		[SerializeField] private AudioClip hitSFX;

		[HideInInspector] public int playerHealth = 100;
		[HideInInspector] public int playerArmor = 100;
		[HideInInspector] public int coinsAmount;
		[HideInInspector] public bool noClip;
		[HideInInspector] public bool isDead;

		[HideInInspector] public PlayerMovement playerMovement;
		[HideInInspector] public PlayerLeveling playerLeveling;
		[HideInInspector] public PlayerHealing playerHealing;
		[HideInInspector] public PlayerAttack playerAttack;

		[HideInInspector] public Slider healthBar;
		[HideInInspector] public Slider armorBar;

		private float chillTimer;
		private bool playerChilled;

		private SpriteRenderer spriteRenderer;
		private Text coinsAmountText;
		private Image vignette;

		public bool GodMode { private get; set; }
		public bool Invisible { get; private set; }

		void Awake()
		{
			healthBar = GameObject.Find("PlayerHealthBar").GetComponent<Slider>();
			armorBar = GameObject.Find("PlayerArmorBar").GetComponent<Slider>();
			coinsAmountText = GameObject.Find("CoinsAmountText").GetComponent<Text>();
			vignette = GameObject.Find("Vignette").GetComponent<Image>();

			spriteRenderer = GetComponent<SpriteRenderer>();
			playerMovement = GetComponent<PlayerMovement>();
			playerLeveling = GetComponent<PlayerLeveling>();
			playerHealing = GetComponent<PlayerHealing>();
			playerAttack = GetComponent<PlayerAttack>();
		}

		void Start()
		{
			if (Application.loadedLevelName != "Lobby") GameManager.INSTANCE.gameData.LoadPlayerData();

			healthBar.maxValue = defaultPlayerHealth;
			armorBar.maxValue = defaultPlayerArmor;
			playerMovement.playerSpeed = playerMovement.defaultPlayerSpeed;
		}

		void Update()
		{
			healthBar.value = playerHealth;
			armorBar.value = playerArmor;
			coinsAmountText.text = coinsAmount.ToString();
			collider2D.enabled = !noClip;
			
			var healthFraction = defaultPlayerHealth / 4;

			// We check if the player's health is less then 25%
			if (playerHealth < healthFraction)
			{
				vignette.color = new Color(0.35f, 0f, 0f, 0.7f); // Set the color to a dark red
			}
			else
			{
				vignette.color = new Color(0f, 0f, 0f, 0.7f); // Sethe color to black
			}

			if (playerHealth <= 0)
			{
				playerHealth = 0;
				Die();
			}

			if (chillTimer > 0f)
			{
				chillTimer -= Time.deltaTime;
			}
			else if (playerChilled)
			{
				UnchillPlayer();
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

			audio.clip = hitSFX;
			audio.pitch = Random.Range(1f, 1.3f);
			audio.Play();
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

		public void IncreaseSpeed(float amount)
		{
			playerMovement.defaultPlayerSpeed += amount;

			playerMovement.playerSpeed = playerMovement.defaultPlayerSpeed;
		}

		public void ToogleInvisibility(bool value)
		{
			Invisible = value;

			// Make the player transparent if he is invisible
			if (Invisible)
			{
				spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f);
			}
			else
			{
				spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
			}
		}

		private void Die()
		{
			isDead = true;
			rigidbody2D.isKinematic = true;

			if (lifeCount > 0)
			{
				lifeCount--;
				lifeCountText.text = lifeCount.ToString();
			}

			audio.clip = deathSFX;
			audio.pitch = 1f;
			audio.Play();

			collider2D.enabled = false;
			playerAttack.enabled = false;
			playerHealing.enabled = false;
			playerMovement.enabled = false;
			enabled = false;
		}

		public void ChillPlayer()
		{
			chillTimer = 2f;
			playerChilled = true;
			spriteRenderer.color = new Color(0.85f, 0.95f, 1f);

			if (playerMovement.playerSpeed == playerMovement.defaultPlayerSpeed) playerMovement.playerSpeed /= 1.5f;
		}

		private void UnchillPlayer()
		{
			chillTimer = 0f;
			playerChilled = false;
			spriteRenderer.color = Color.white;

			playerMovement.playerSpeed = playerMovement.defaultPlayerSpeed;
		}
	}
}
