﻿using UnityEngine;
using System.Collections.Generic;

namespace DungeonQuest.Debuging
{
	public class DebugConsole : MonoBehaviour
	{
		private bool isConsoleOn;

		private const float OUTPUT_WINDOW_HEIGHT = 300f;
		private string input;

		private static DebugCommand<int> HEAL;
		private static DebugCommand<int> ARMOR;
		private static DebugCommand<int> GIVE_COINS;
		private static DebugCommand<int> LOAD_SCENE;
		private static DebugCommand<uint> SET_DAMAGE;
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

		private Vector2 scroll;

		private List<object> commandList;
		private List<string> outputList = new List<string>();

		private EnemyPrefabLoader enemyPrefabs = new EnemyPrefabLoader();

		void Awake()
		{
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

			LOAD_SCENE = new DebugCommand<int>("loadscene", "Loads a specified scene", "loadscene <index>", (value) =>
			{
				GameManager.INSTANCE.LoadScene(value);
			});

			SET_DAMAGE = new DebugCommand<uint>("setdamage", "Sets the player damage", "setdamage <amount>", (value) =>
			{
				GameManager.INSTANCE.playerManager.playerAttack.IncreaseDamage((int)value);

				outputList.Add("Player has damage set to " + value);
			});

			GOD_MODE = new DebugCommand<bool>("godmode", "Makes the player invincible", "godmode <true/false>", (value) =>
			{
				GameManager.INSTANCE.playerManager.GodMode = value;

				var toogleText = value ? "On" : "Off";

				outputList.Add("Godmode " + toogleText);
			});

			NOCLIP = new DebugCommand<bool>("noclip", "Makes the player able to go trough objects", "noclip <true/false>", (value) =>
			{
				GameManager.INSTANCE.playerManager.collider2D.enabled = !value;

				var toogleText = value ? "On" : "Off";

				outputList.Add("Noclip " + toogleText);
			});

			INVISIBILITY = new DebugCommand<bool>("invisibility", "Makes the player invisible to the enemies", "invisibility <true/false>", (value) =>
			{
				GameManager.INSTANCE.playerManager.Invisible = value;

				var toogleText = value ? "On" : "Off";

				outputList.Add("Invisiblity " + toogleText);
			});

			SPAWN_ENEMY = new DebugCommand<string>("spawnenemy", "Spawns a specified enemy in the scene. To see the enemy list type \"enemylist\" ", "spawnenemy <name>", (value) =>
			{
				SpawnEnemy(value);

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
				outputList.Add("Enemy list:");

				for (int i = 0; i < enemyPrefabs.enemyList.Count; i++)
				{
					outputList.Add(enemyPrefabs.enemyList[i]);
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
				GIVE_COINS,
				SET_DAMAGE,
				GOD_MODE,
				NOCLIP,
				INVISIBILITY,
				SPAWN_ENEMY,
				LOAD_SCENE,
				KILL_ENEMIES,
				ENEMY_LIST,
				LEVEL_UP,
				DIE,
				CLEAR,
				HELP
			};
			#endregion

			outputList.Add("Type \"help\" to view the list of available commands");

			enemyPrefabs.LoadPrefabs();
		}

		void Update()
		{
			if (Input.GetButtonDown("Console"))
			{
				if (!isConsoleOn && GameManager.INSTANCE.CurrentGameState != GameManager.GameState.Paused)
				{
					input = "";
					isConsoleOn = true;
					AudioListener.pause = true;

					GameManager.EnableCursor(true);
					GameManager.INSTANCE.SetGameState(GameManager.GameState.Paused);
				}
				else if (isConsoleOn)
				{
					isConsoleOn = false;
					AudioListener.pause = false;

					GameManager.EnableCursor(false);
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

		private void SpawnEnemy(string name)
		{
			name = name.ToLower();

			switch (name)
			{
				case "meleeskeleton":
					Instantiate(enemyPrefabs.MeleeSkeleton, GameManager.INSTANCE.playerManager.transform.position, Quaternion.identity);
					outputList.Add(name + " spawned");
					break;
				case "rangedskeleton":
					Instantiate(enemyPrefabs.RangedSkeleton, GameManager.INSTANCE.playerManager.transform.position, Quaternion.identity);
					outputList.Add(name + " spawned");
					break;
				default:
					outputList.Add("Enemy not found");
					break;
			}
		}
	}
}
