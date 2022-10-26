using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace DungeonQuest.Player
{
	public class PlayerManager : MonoBehaviour
	{
		[Header("Player Config:")]
		public int defaultPlayerHealth;
		public int defaultPlayerArmor;
		public int lifeCount;
		[Space]
		[SerializeField] private Text lifeCountText;
		[SerializeField] private Button goToLobbyButton;

		[Header("Audio Config:")]
		public AudioSource audioSource2D;
		public AudioSource audioSource3D;
		[SerializeField] private AudioClip deathSFX;
		[SerializeField] private AudioClip hitSFX;

		public static bool ROGUE_MODE;
		public const int COINS_CAP = 2000000000;

		[HideInInspector] public int playerHealth = 100;
		[HideInInspector] public int playerArmor = 100;
		[HideInInspector] public int coinsAmount;
		[HideInInspector] public bool noClip;
		[HideInInspector] public bool isDead;
		[HideInInspector] public bool playerChilled;

		[HideInInspector] public PlayerMovement playerMovement;
		[HideInInspector] public PlayerLeveling playerLeveling;
		[HideInInspector] public PlayerHealing playerHealing;
		[HideInInspector] public PlayerAttack playerAttack;
		[HideInInspector] public PlayerMap playerMap;

		[HideInInspector] public SpriteRenderer spriteRenderer;
		[HideInInspector] public Rigidbody2D playerRigidbody;
		[HideInInspector] public Collider2D playerCollider;

		[HideInInspector] public Slider healthBar;
		[HideInInspector] public Slider armorBar;

		private float chillTimer;

		private Text coinsAmountText;
		private Animator vignetteAnimator;

		public float LifestealAmount { get; private set; }
		public bool GodMode { private get; set; }
		public bool Invisible { get; private set; }

		void Awake()
		{
			healthBar = GameObject.Find("PlayerHealthBar").GetComponent<Slider>();
			armorBar = GameObject.Find("PlayerArmorBar").GetComponent<Slider>();
			coinsAmountText = GameObject.Find("CoinsAmountText").GetComponent<Text>();
			vignetteAnimator = GameObject.Find("Vignette").GetComponent<Animator>();

			spriteRenderer = GetComponent<SpriteRenderer>();
			playerRigidbody = GetComponent<Rigidbody2D>();
			playerCollider = GetComponent<Collider2D>();

			playerMovement = GetComponent<PlayerMovement>();
			playerLeveling = GetComponent<PlayerLeveling>();
			playerHealing = GetComponent<PlayerHealing>();
			playerAttack = GetComponent<PlayerAttack>();
			playerMap = GetComponent<PlayerMap>();
		}

		void Start()
		{
			if (SceneManager.GetActiveScene().name != "Lobby") GameManager.INSTANCE.gameData.LoadPlayerData();

			if (ROGUE_MODE)
			{
				lifeCount = 1;
				
				if (goToLobbyButton != null) goToLobbyButton.interactable = false;
			}

			lifeCountText.text = lifeCount.ToString();
			healthBar.maxValue = defaultPlayerHealth;
			armorBar.maxValue = defaultPlayerArmor;
			playerMovement.playerSpeed = playerMovement.defaultPlayerSpeed;
		}

		void Update()
		{
			healthBar.value = playerHealth;
			armorBar.value = playerArmor;
			coinsAmountText.text = coinsAmount.ToString();
			playerCollider.enabled = !noClip;

			if (coinsAmount > COINS_CAP) coinsAmount = COINS_CAP;

			// We check if the player's health is 25% or less
			if (playerHealth <= defaultPlayerHealth / 4)
			{
				vignetteAnimator.Play("LowHealth");
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

			// We check if the player's health is less then 25% then play the animation that makes the vignette red
			if (playerHealth < defaultPlayerHealth / 4) vignetteAnimator.Play("LowHealth");

			audioSource2D.clip = hitSFX;
			audioSource2D.pitch = Random.Range(1f, 1.3f);
			audioSource2D.Play();
		}

		public void HealPlayer(int amount)
		{
			playerHealth += amount;

			if (playerHealth > defaultPlayerHealth) playerHealth = defaultPlayerHealth;

			// We check if the player's health is more then 25% then play the animation that makes the vignette back to normal
			if (playerHealth > defaultPlayerHealth / 4) vignetteAnimator.Play("Default");
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

		public void IncreaseLifesteal(float amount)
		{
			LifestealAmount += amount;
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
			playerRigidbody.isKinematic = true;
			playerRigidbody.velocity = Vector2.zero;

			UnchillPlayer();

			if (lifeCount > 0)
			{
				lifeCount--;
				lifeCountText.text = lifeCount.ToString();
			}

			if (ROGUE_MODE)
			{
				File.Delete(Application.persistentDataPath + "/Data/PlayerData.dat");
				File.Delete(Application.persistentDataPath + "/Data/GameData.dat");
			}

			audioSource3D.clip = deathSFX;
			audioSource3D.pitch = 1f;

			audioSource3D.Play();
			vignetteAnimator.Play("Default");

			playerCollider.enabled = false;
			playerAttack.enabled = false;
			playerHealing.enabled = false;
			playerMovement.enabled = false;
			enabled = false;
		}

		public void ChillPlayer(float duration)
		{
			chillTimer = duration;
			playerChilled = true;
			spriteRenderer.color = new Color(0.80f, 0.95f, 1f);

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
