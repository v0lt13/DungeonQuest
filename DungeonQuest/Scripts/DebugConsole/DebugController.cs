using UnityEngine;
using DungeonQuest.Menus;
using System.Collections.Generic;

namespace DungeonQuest.DebugConsole
{
	public class DebugController : MonoBehaviour
	{
		public static bool IS_CONSOLE_ON;

		private const float OUTPUT_WINDOW_HEIGHT = 150f;
		private string input;

		private static DebugCommand<int> SET_HP;
		private static DebugCommand<int> SET_DAMAGE;
		private static DebugCommand<bool> SHOW_FPS;
		private static DebugCommand<bool> GOD_MODE;
		private static DebugCommand<bool> NOCLIP;
		private static DebugCommand KILL_ENEMIES;
		private static DebugCommand SPAWN_ENEMY;
		private static DebugCommand DIE;
		private static DebugCommand CLEAR;
		private static DebugCommand HELP;

		private List<string> outputList = new List<string>();
		private List<object> commandList;

		private Vector2 scroll;

		[SerializeField] private GameObject enemyPrefab;

		void Awake()
		{
			outputList.Add("Type help to view the list of available commands");

			SET_HP = new DebugCommand<int>("sethp", "Sets the player health", "sethp <amount>", (health) =>
			{
				GameManager.INSTANCE.playerManager.playerHealth = health;

				outputList.Add("Player has health set to " + health);
			});

			SET_DAMAGE = new DebugCommand<int>("setdamage", "Sets the player damage", "setdamage <amount>", (damage) =>
			{
				GameManager.INSTANCE.playerManager.playerAttack.damage = damage;

				outputList.Add("Player has damage set to " + damage);
			});

			SHOW_FPS = new DebugCommand<bool>("showfps", "Toogles FPS counter", "showfps <true or false>", (toogle) =>
			{
				var framerate = GameObject.Find("Framerate").GetComponent<UnityEngine.UI.Text>();

				if (framerate != null) framerate.enabled = toogle;

				outputList.Add("Showfps has been set to " + toogle);
			});

			GOD_MODE = new DebugCommand<bool>("godmode", "Makes the player invincible", "godmode <true/false>", (toogle) =>
			{
				GameManager.INSTANCE.playerManager.GodMode = toogle;

				outputList.Add("Godmode has been set to " + toogle);
			});

			NOCLIP = new DebugCommand<bool>("noclip", "Makes the player able to go trough objects", "noclip <true/false>", (toogle) =>
			{
				GameManager.INSTANCE.playerManager.boxCollider.enabled = !toogle;

				outputList.Add("Noclip has been set to " + toogle);
			});

			KILL_ENEMIES = new DebugCommand("killenemies", "Kills all enemies in the level", "killenemies", () =>
			{
				var enemyList = GameManager.INSTANCE.enemyList;

				for (int i = 0; i < enemyList.Count; i++)
				{
					enemyList[i].GetComponent<Enemy.EnemyManager>().DamageEnemy(int.MaxValue);
				}

				outputList.Add("All enemies killed");			
			});

			SPAWN_ENEMY = new DebugCommand("spawnenemy", "Spawns an enemy in the scene", "spawnenemy", () =>
			{
				Instantiate(enemyPrefab, GameManager.INSTANCE.playerManager.transform.position, Quaternion.identity);

				GameManager.INSTANCE.AddEnemies();

				outputList.Add("Enemy spawned");
			});

			DIE = new DebugCommand("die", "Kills the player", "die", () =>
			{
				GameManager.INSTANCE.playerManager.DamagePlayer(int.MaxValue);

				outputList.Add("R.I.P");
			});

			CLEAR = new DebugCommand("clear", "Clears the console", "clear", () =>
			{
				outputList.Clear();
			});

			HELP = new DebugCommand("help", "Shows a list of commands", "help", () =>
			{
				for (int i = 0; i < commandList.Count; i++)
				{
					DebugCommandBase command = commandList[i] as DebugCommandBase;

					outputList.Add(command.CommandFormat + " - " + command.CommandDescription);
				}
			});

			commandList = new List<object>
			{
				SET_HP,
				SET_DAMAGE,
				SHOW_FPS,
				GOD_MODE,
				NOCLIP,
				KILL_ENEMIES,
				SPAWN_ENEMY,
				DIE,
				CLEAR,
				HELP
			};
		}

		void Update()
		{
			if (PauseMenu.IS_GAME_PAUSED) return;

			if (Input.GetKeyDown(KeyCode.F3))
			{
				IS_CONSOLE_ON = !IS_CONSOLE_ON;
				input = "";

				Time.timeScale = IS_CONSOLE_ON ? 0f : 1f;
				GameManager.INSTANCE.EnableCursor(IS_CONSOLE_ON);
			}
		}

		void OnGUI()
		{
			if (!IS_CONSOLE_ON || PauseMenu.IS_GAME_PAUSED) return;

			var y = 0f;

			GUI.Box(new Rect(0f, y, Screen.width, OUTPUT_WINDOW_HEIGHT), ""); // Draw output window

			OutputConsoleMessage();

			y += OUTPUT_WINDOW_HEIGHT;

			GUI.Box(new Rect(0f, y, Screen.width, 30f), ""); // Draw input window
			GUI.backgroundColor = new Color(0f, 0f, 0f, 0f);

			GUI.SetNextControlName("InputField");
			input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 20f), input);

			if (Event.current.keyCode == KeyCode.Return)
			{
				GUI.FocusControl("InputField");
				HandleInput();
				input = "";
			}
		}

		private void HandleInput()
		{
			string[] proprieties = input.Split(' ');

			for (int i = 0; i < commandList.Count; i++)
			{
				var commandBase = commandList[i] as DebugCommandBase;

				if (input.Contains(commandBase.CommandID))
				{
					if (commandList[i] as DebugCommand != null)
					{
						(commandList[i] as DebugCommand).Invoke();
					}
					else if (commandList[i] as DebugCommand<string> != null)
					{
						try
						{
							(commandList[i] as DebugCommand<string>).Invoke(proprieties[1]);
						}
						catch (System.Exception)
						{
							outputList.Add("Prameter must be a string");
						}
					}
					else if (commandList[i] as DebugCommand<int> != null)
					{
						try
						{
							(commandList[i] as DebugCommand<int>).Invoke(int.Parse(proprieties[1]));
						}
						catch (System.Exception)
						{
							outputList.Add("Prameter must be an integer");
						}
					}
					else if (commandList[i] as DebugCommand<float> != null)
					{
						try
						{
							(commandList[i] as DebugCommand<float>).Invoke(float.Parse(proprieties[1]));
						}
						catch (System.Exception)
						{
							outputList.Add("Prameter must be a float");
						}
					}
					else if (commandList[i] as DebugCommand<bool> != null)
					{
						try
						{
							(commandList[i] as DebugCommand<bool>).Invoke(bool.Parse(proprieties[1]));
						}
						catch (System.Exception)
						{
							outputList.Add("Prameter must be true or false");
						}
					}
				}
			}
		}
		
		private void OutputConsoleMessage()
		{
			for (int i = 0; i < outputList.Count; i++)
			{
				string label = outputList[i];
				var viewport = new Rect(0f, 0f, Screen.width - 30f, 20f * outputList.Count);
				var labelRect = new Rect(5f, 20f * i, viewport.width - 100f, 20f);

				scroll = GUI.BeginScrollView(new Rect(0f, 0f, Screen.width, OUTPUT_WINDOW_HEIGHT), scroll, viewport);

				GUI.Label(labelRect, label);
				GUI.EndScrollView();
			}
		}
	}
}
