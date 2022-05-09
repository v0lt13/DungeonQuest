using UnityEngine;
using DungeonQuest.Enemy;
using System.Collections.Generic;

namespace DungeonQuest.Debuging
{
	public class DebugConsole : MonoBehaviour
	{
		private const float OUTPUT_WINDOW_HEIGHT = 350f;
		public static bool ENABLE_CONSOLE;

		private bool isConsoleOn;
		private string input;

		private static DebugCommand<int> HEAL;
		private static DebugCommand<int> ARMOR;
		private static DebugCommand<int> GIVE_COINS;
		private static DebugCommand<uint> LOAD_SCENE;
		private static DebugCommand<uint> ADD_POTIONS;
		private static DebugCommand<uint> SET_DAMAGE;
		private static DebugCommand<uint> SET_SPEED;
		private static DebugCommand<uint> UNLOCK_LEVEL;
		private static DebugCommand<bool> GOD_MODE;
		private static DebugCommand<bool> NOCLIP;
		private static DebugCommand<bool> INVISIBILITY;
		private static DebugCommand<string, uint> SPAWN_ENEMY;
		private static DebugCommand KILL_ENEMIES;
		private static DebugCommand ENEMY_LIST;
		private static DebugCommand SCENE_LIST;
		private static DebugCommand LEVEL_UP;
		private static DebugCommand DIE;
		private static DebugCommand SAVE;
		private static DebugCommand CLEAR;
		private static DebugCommand HELP;


		private Vector2 scroll;

		private List<object> commandList;
		private List<string> outputList = new List<string>();

		private EnemyPrefabManager enemyPrefabs = new EnemyPrefabManager();
		private GUIStyle outputTextStyle = new GUIStyle();

		void Awake()
		{
			enemyPrefabs.LoadPrefabs();
			outputList.Add("Type \"help\" to view the list of available commands");

			#region COMMANDS			
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

			GIVE_COINS = new DebugCommand<int>("givecoins", "Gives coins to the player, negative numbers substract the amount", "givecoins <amount>", (value) =>
			{
				GameManager.INSTANCE.playerManager.GiveCoins(value);

				outputList.Add(value.ToString() + " coins given");
			});

			LOAD_SCENE = new DebugCommand<uint>("loadscene", "Loads a specified scene", "loadscene <index>", (value) =>
			{
				if (value < Application.levelCount)
				{
					GameManager.INSTANCE.SetGameState(GameManager.GameState.Running);
					GameManager.INSTANCE.LoadScene((int)value);
				}
				else
				{
					outputList.Add("Scene does not exist");
				}
			});

			ADD_POTIONS = new DebugCommand<uint>("addpotions", "Gives healing potions to the player", "addpotions <amount>", (value) =>
			{
				GameManager.INSTANCE.playerManager.playerHealing.AddPotions((int)value);

				outputList.Add(value + " potions added");
			});

			SET_DAMAGE = new DebugCommand<uint>("setdamage", "Sets the player damage", "setdamage <amount>", (value) =>
			{
				GameManager.INSTANCE.playerManager.playerAttack.damage = (int)value;

				outputList.Add("Player damage has been set to " + value);
			});

			SET_SPEED = new DebugCommand<uint>("setspeed", "Sets the player speed. Default = 35", "setspeed <amount>", (value) =>
			{
				GameManager.INSTANCE.playerManager.playerMovement.playerSpeed = value;

				outputList.Add("Player speed has been set to " + value);
			});

			UNLOCK_LEVEL = new DebugCommand<uint>("unlocklevel", "Unlocks a specified level. Requires reloading the scene if in the Lobby", "unlocklevel <level>", (value) =>
			{
				GameManager.INSTANCE.UnlockLevel((int)value);
				GameManager.INSTANCE.gameData.SavePlayerData();

				outputList.Add("Level " + value + " unlocked");
			});

			GOD_MODE = new DebugCommand<bool>("godmode", "Makes the player invincible", "godmode <true/false>", (value) =>
			{
				GameManager.INSTANCE.playerManager.GodMode = value;

				var toogleText = value ? "On" : "Off";

				outputList.Add("Godmode " + toogleText);
			});

			NOCLIP = new DebugCommand<bool>("noclip", "Makes the player able to go trough objects and invisible", "noclip <true/false>", (value) =>
			{
				GameManager.INSTANCE.playerManager.noClip = value;
				GameManager.INSTANCE.playerManager.invisible = value;

				var toggleText = value ? "On" : "Off";

				outputList.Add("Noclip " + toggleText);
			});

			INVISIBILITY = new DebugCommand<bool>("invisibility", "Makes the player invisible to the enemies", "invisibility <true/false>", (value) =>
			{
				GameManager.INSTANCE.playerManager.invisible = value;

				var toggleText = value ? "On" : "Off";

				outputList.Add("Invisiblity " + toggleText);
			});

			SPAWN_ENEMY = new DebugCommand<string, uint>("spawnenemy", "Spawns a specified enemy in the scene. To see the enemy list type \"enemylist\" ", "spawnenemy <name> <level>", (primaryValue, secondaryValue) =>
			{
				SpawnEnemy(primaryValue, secondaryValue);
			});

			LEVEL_UP = new DebugCommand("levelup", "Levels up the player", "levelup", () =>
			{
				var playerLeveling = GameManager.INSTANCE.playerManager.playerLeveling;

				if (playerLeveling.IsPlayerMaxLevel)
				{
					outputList.Add("Player is max level");
				}
				else
				{
					playerLeveling.LevelUp();
					outputList.Add("Player has leveled up");
				}
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
				outputList.Add("Enemy list:");

				for (int i = 0; i < enemyPrefabs.enemyList.Count; i++)
				{
					outputList.Add(enemyPrefabs.enemyList[i].ToString());
				}
			});

			SCENE_LIST = new DebugCommand("scenelist", "Displays a list of all scenes and their index", "scenelist", () =>
			{
				var sceneList = new List<string>
				{
					"0 - MainMenu",
					"1 - LoadingScene",
					"2 - Entrance",
					"3 - Lobby",
					"4 - C1L1",
					"5 - C1L2",
					"6 - C1L3",
					"7 - C1L4",
					"8 - C1L5",
					"9 - S1",
					"10 - C2L1",
					"11 - C2L2",
					"12 - C2L3",
					"13 - C2L4",
					"14 - C2L5",
					"15 - S2",
					"16 - Intermission01",
					"17 - Intermission02"
				};

				outputList.Add("Scene list:");

				for (int i = 0; i < sceneList.Count; i++)
				{
					outputList.Add(sceneList[i]);
				}
			});

			DIE = new DebugCommand("die", "Kills the player", "die", () =>
			{
				GameManager.INSTANCE.playerManager.DamagePlayer(int.MaxValue);

				outputList.Add("R.I.P");
			});

			SAVE = new DebugCommand("save", "Saves the player data", "save", () =>
			{
				GameManager.INSTANCE.gameData.SavePlayerData();

				outputList.Add("Game saved");
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
				ADD_POTIONS,
				GIVE_COINS,
				SET_DAMAGE,
				SET_SPEED,
				UNLOCK_LEVEL,
				GOD_MODE,
				NOCLIP,
				INVISIBILITY,
				SPAWN_ENEMY,
				LOAD_SCENE,
				KILL_ENEMIES,
				ENEMY_LIST,
				SCENE_LIST,
				LEVEL_UP,
				DIE,
				SAVE,
				CLEAR,
				HELP
			};
			#endregion
		}

		void Update()
		{
			if (Input.GetButtonDown("Console") && ENABLE_CONSOLE)
			{
				if (!isConsoleOn && GameManager.INSTANCE.CurrentGameState != GameManager.GameState.Paused)
				{
					input = "";
					isConsoleOn = true;
					AudioListener.pause = true;

					GameManager.INSTANCE.SetGameState(GameManager.GameState.Paused);
				}
				else if (isConsoleOn)
				{
					isConsoleOn = false;
					AudioListener.pause = false;

					GameManager.INSTANCE.SetGameState(GameManager.GameState.Running);
				}
			}
		}

		void OnGUI()
		{
			if (!isConsoleOn) return;

			var y = 0f;

			GUI.Box(new Rect(0f, y, Screen.width, OUTPUT_WINDOW_HEIGHT), ""); // Draw output window

			OutputConsoleMessage();

			y += OUTPUT_WINDOW_HEIGHT;

			GUI.Box(new Rect(0f, y, Screen.width, 30f), ""); // Draw input window
			GUI.backgroundColor = new Color(0f, 0f, 0f, 0f);

			GUI.SetNextControlName("InputField");
			input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 20f), input);

			if (Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.KeypadEnter)
			{
				GUI.FocusControl("InputField");
				HandleInput();

				input = "";
				scroll.y += 9999; // Scroll to the bottom of the output window
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
					try
					{
						if (commandList[i] as DebugCommand != null)
						{
							(commandList[i] as DebugCommand).Invoke();
						}
						else if (commandList[i] as DebugCommand<string> != null)
						{
							(commandList[i] as DebugCommand<string>).Invoke(proprieties[1]);
						}
						else if (commandList[i] as DebugCommand<string, uint> != null)
						{								
							(commandList[i] as DebugCommand<string, uint>).Invoke(proprieties[1], uint.Parse(proprieties[2]));
						}
						else if (commandList[i] as DebugCommand<int> != null)
						{
							(commandList[i] as DebugCommand<int>).Invoke(int.Parse(proprieties[1]));
						}
						else if (commandList[i] as DebugCommand<uint> != null)
						{
							(commandList[i] as DebugCommand<uint>).Invoke(uint.Parse(proprieties[1]));
						}
						else if (commandList[i] as DebugCommand<float> != null)
						{
							(commandList[i] as DebugCommand<float>).Invoke(float.Parse(proprieties[1]));
						}
						else if (commandList[i] as DebugCommand<bool> != null)
						{
							(commandList[i] as DebugCommand<bool>).Invoke(bool.Parse(proprieties[1]));
						}
					}
					catch (System.Exception exception)
					{
						outputList.Add("Invalid parameter or something went wrong, exception type: " + exception + exception.Message);
					}
				}
			}
		}
		
		private void OutputConsoleMessage()
		{
			const int MAX_OUTPUT_MESSAGES = 50;

			if (outputList.Count > MAX_OUTPUT_MESSAGES)
			{
				outputList.RemoveAt(0);
			}

			for (int i = 0; i < outputList.Count; i++)
			{
				string label = outputList[i];
				var viewport = new Rect(0f, 0f, Screen.width - 30f, 20f * outputList.Count);
				var labelRect = new Rect(5f, 20f * i, viewport.width - 100f, 20f);
				outputTextStyle.normal.textColor = Color.white;

				scroll = GUI.BeginScrollView(new Rect(0f, 0f, Screen.width, OUTPUT_WINDOW_HEIGHT), scroll, viewport);
				
				GUI.Label(labelRect, label);
				GUI.EndScrollView();
			}
		}

		private void SpawnEnemy(string name, uint level)
		{
			name = name.ToLower();

			switch (name)
			{
				case "meleeskeleton":
					enemyPrefabs.InstatiateEnemy(enemyPrefabs.MeleeSkeleton as GameObject, level);
					outputList.Add(name + " spawned");
					break;

				case "rangedskeleton":
					enemyPrefabs.InstatiateEnemy(enemyPrefabs.RangedSkeleton as GameObject, level);
					outputList.Add(name + " spawned");
					break;

				case "armmeleeskeleton":
					enemyPrefabs.InstatiateEnemy(enemyPrefabs.ArmoredMeleeSkeleton as GameObject, level);
					outputList.Add(name + " spawned");
					break;

				case "armrangedskeleton":
					enemyPrefabs.InstatiateEnemy(enemyPrefabs.ArmoredRangedSkeleton as GameObject, level);
					outputList.Add(name + " spawned");
					break;

				case "spider":
					enemyPrefabs.InstatiateEnemy(enemyPrefabs.Spider as GameObject, level);
					outputList.Add(name + " spawned");
					break;

				default:
					outputList.Add("Enemy not found");
					break;
			}
		}
	}
}
