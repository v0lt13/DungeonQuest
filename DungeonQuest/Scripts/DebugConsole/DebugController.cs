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

		private static DebugCommand<int> HEAL;
		private static DebugCommand<int> ARMOR;
		private static DebugCommand<uint> SET_DAMAGE;
		private static DebugCommand<bool> SHOW_FPS;
		private static DebugCommand<bool> GOD_MODE;
		private static DebugCommand<bool> NOCLIP;
		private static DebugCommand<bool> INVISIBILITY;
		private static DebugCommand<string> SPAWN_ENEMY;
		private static DebugCommand KILL_ENEMIES;
		private static DebugCommand ENEMY_LIST;
		private static DebugCommand LEVEL_UP;
		private static DebugCommand DIE;
		private static DebugCommand CLEAR;
		private static DebugCommand HELP;

		private List<object> commandList;
		private List<string> outputList = new List<string>();
		private List<string> enemyList = new List<string>
		{
			"melee",
			"ranged"
		};

		private Vector2 scroll;

		[SerializeField] private GameObject enemyPrefab;
		[SerializeField] private GameObject rangedEnemyPrefab;

		void Awake()
		{
			outputList.Add("Type help to view the list of available commands");

			HEAL = new DebugCommand<int>("heal", "Heals the player, negative numbers substracts the health", "heal <amount>", (value) =>
			{
				GameManager.INSTANCE.playerManager.HealPlayer(value);

				outputList.Add("Player health set");
			});

			ARMOR = new DebugCommand<int>("armor", "Armors the player, negative numbers substract the armor", "armor <amount>", (value) =>
			{
				GameManager.INSTANCE.playerManager.ArmorPlayer(value);

				outputList.Add("Player armor set");
			});

			SET_DAMAGE = new DebugCommand<uint>("setdamage", "Sets the player damage", "setdamage <amount>", (value) =>
			{
				GameManager.INSTANCE.playerManager.playerAttack.damage = (int)value;

				outputList.Add("Player has damage set to " + value);
			});

			SHOW_FPS = new DebugCommand<bool>("showfps", "Toogles FPS counter", "showfps <true/false>", (toogle) =>
			{
				var framerate = GameObject.Find("Framerate").GetComponent<UnityEngine.UI.Text>();

				if (framerate != null) framerate.enabled = toogle;

				var toogleText = toogle ? "On" : "Off";

				outputList.Add("FPS counter is " + toogleText);
			});

			GOD_MODE = new DebugCommand<bool>("godmode", "Makes the player invincible", "godmode <true/false>", (toogle) =>
			{
				GameManager.INSTANCE.playerManager.GodMode = toogle;

				var toogleText = toogle ? "On" : "Off";

				outputList.Add("Godmode " + toogleText);
			});

			NOCLIP = new DebugCommand<bool>("noclip", "Makes the player able to go trough objects", "noclip <true/false>", (toogle) =>
			{
				GameManager.INSTANCE.playerManager.boxCollider.enabled = !toogle;

				var toogleText = toogle ? "On" : "Off";

				outputList.Add("Noclip " + toogleText);
			});

			INVISIBILITY = new DebugCommand<bool>("invisibility", "Makes the player invisible to the enemies", "invisibility <true/false>", (toogle) =>
			{
				GameManager.INSTANCE.playerManager.Invisible = toogle;

				var toogleText = toogle ? "On" : "Off";

				outputList.Add("Invisiblity " + toogleText);
			});

			SPAWN_ENEMY = new DebugCommand<string>("spawnenemy", "Spawns a specified enemy in the scene. To see the enemy list type \"enemylist\" ", "spawnenemy <name>", (name) =>
			{
				EnemySpawner(name);

				GameManager.INSTANCE.AddEnemies();
			});

			LEVEL_UP = new DebugCommand("levelup", "Levels up the player", "levelup", () =>
			{
				GameManager.INSTANCE.playerManager.playerLeveling.LevelUp();

				outputList.Add("Player has leveled up");
			});

			KILL_ENEMIES = new DebugCommand("killenemies", "Kills all enemies in the level", "killenemies", () =>
			{
				var enemyList = GameManager.INSTANCE.enemyList;

				if (enemyList.Count != 0)
				{
					for (int i = 0; i < enemyList.Count; i++)
					{
						enemyList[i].GetComponent<Enemy.EnemyManager>().DamageEnemy(int.MaxValue);
					}

					outputList.Add(enemyList.Count + " enemies killed");
					enemyList.Clear();
				}
				else
				{
					outputList.Add("No enemies found");
				}
			});

			ENEMY_LIST = new DebugCommand("enemylist", "Displays a list of all names of enemies", "enemylist", () =>
			{
				for (int i = 0; i < enemyList.Count; i++)
				{
					outputList.Add(enemyList[i]);
				}
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
				HEAL,
				ARMOR,
				SET_DAMAGE,
				SHOW_FPS,
				GOD_MODE,
				NOCLIP,
				INVISIBILITY,
				SPAWN_ENEMY,
				KILL_ENEMIES,
				ENEMY_LIST,
				LEVEL_UP,
				DIE,
				CLEAR,
				HELP
			};
		}

		void Update()
		{
			if (PauseMenu.IS_GAME_PAUSED) return;

			if (Input.GetButtonDown("Console"))
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
					else if (commandList[i] as DebugCommand<uint> != null)
					{
						try
						{
							(commandList[i] as DebugCommand<uint>).Invoke(uint.Parse(proprieties[1]));
						}
						catch (System.Exception)
						{
							outputList.Add("Prameter must be a positive integer");
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

		private void EnemySpawner(string name)
		{
			switch (name)
			{
				case "melee":
					Instantiate(enemyPrefab, GameManager.INSTANCE.playerManager.transform.position, Quaternion.identity);
					outputList.Add(name + " enemy spawned");
					break;
				case "ranged":
					Instantiate(rangedEnemyPrefab, GameManager.INSTANCE.playerManager.transform.position, Quaternion.identity);
					outputList.Add(name + " enemy spawned");
					break;
				default:
					outputList.Add("Enemy not found");
					break;
			}
		}
	}
}
