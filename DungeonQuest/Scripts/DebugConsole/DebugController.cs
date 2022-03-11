using UnityEngine;
using DungeonQuest.Menus;
using System.Collections.Generic;

namespace DungeonQuest.DebugConsole
{
	public class DebugController : MonoBehaviour
	{
		public static bool IS_CONSOLE_ON;
		private string input;

		[SerializeField] private GameObject enemyPrefab;
		private Vector2 scroll;

		private static DebugCommand<bool> SHOW_FPS;
		private static DebugCommand<bool> GOD_MODE;
		private static DebugCommand<bool> NOCLIP;
		private static DebugCommand KILL_ENEMIES;
		private static DebugCommand SPAWN_ENEMY;
		private static DebugCommand DIE;
		private static DebugCommand HELP;

		private List<string> outputList = new List<string>();
		private List<object> commandList;

		void Awake()
		{
			outputList.Add("Type help to view the list of available commands");

			SHOW_FPS = new DebugCommand<bool>("showfps", "Toogles FPS counter", "showfps <true or false>", (toogle) =>
			{
				var framerate = GameObject.Find("Framerate").GetComponent<UnityEngine.UI.Text>();

				if (framerate != null) framerate.enabled = toogle;

				outputList.Add("showfps has been set to " + toogle);
			});

			GOD_MODE = new DebugCommand<bool>("godmode", "Makes the player invincible", "godmode <true/false>", (toogle) =>
			{
				GameManager.INSTANCE.playerManager.GodMode = toogle;

				outputList.Add("godmode has been set to " + toogle);
			});

			NOCLIP = new DebugCommand<bool>("noclip", "Makes the player able to go trough objects", "noclip <true/false>", (toogle) =>
			{
				GameManager.INSTANCE.playerManager.boxCollider.enabled = !toogle;

				outputList.Add("noclip has been set to " + toogle);
			});

			KILL_ENEMIES = new DebugCommand("killenemies", "Kills all enemies in the level", "killenemies", () =>
			{
				GameManager.INSTANCE.enemyManager.DamageEnemy(float.MaxValue);

				outputList.Add("All enemies killed");				
			});

			SPAWN_ENEMY = new DebugCommand("spawnenemy", "Spawns an enemy in the scene", "spawnenemy", () =>
			{
				Instantiate(enemyPrefab, GameManager.INSTANCE.playerManager.transform.position, Quaternion.identity);

				outputList.Add("Enemy spawned");
			});

			DIE = new DebugCommand("die", "Kills the player", "die", () =>
			{
				GameManager.INSTANCE.playerManager.DamagePlayer(float.MaxValue);

				outputList.Add("R.I.P");
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
				SHOW_FPS,
				GOD_MODE,
				NOCLIP,
				KILL_ENEMIES,
				SPAWN_ENEMY,
				DIE,
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

			GUI.Box(new Rect(0, y, Screen.width, 80), "");

			OutputConsoleMessage();

			y += 80;

			GUI.Box(new Rect(0, y, Screen.width, 30), "");
			GUI.backgroundColor = new Color(0, 0, 0, 0);

			input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 20f), input);

			if (Event.current.keyCode == KeyCode.Return)
			{
				HandleInput();
				input = "";
			}
		}

		private void HandleInput()
		{
			string[] proprieties = input.Split(' ');

			for (int i = 0; i < commandList.Count; i++)
			{
				DebugCommandBase commandBase = commandList[i] as DebugCommandBase;

				if (input.Contains(commandBase.CommandID))
				{
					try
					{
						if (commandList[i] as DebugCommand != null)
						{
							(commandList[i] as DebugCommand).Invoke();
						}
						else if (commandList[i] as DebugCommand<int> != null)
						{
							(commandList[i] as DebugCommand<int>).Invoke(int.Parse(proprieties[1]));
						}
						else if (commandList[i] as DebugCommand<string> != null)
						{
							(commandList[i] as DebugCommand<string>).Invoke(proprieties[1]);
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
					catch (System.Exception)
					{
						outputList.Add("Invalid parameters");
					}
				}
			}
		}
		
		private void OutputConsoleMessage()
		{
			for (int i = 0; i < outputList.Count; i++)
			{
				var label = outputList[i];
				var viewport = new Rect(0, 0, Screen.width - 30, 20 * outputList.Count);
				var labelRect = new Rect(5, 20 * i, viewport.width - 100, 20);

				scroll = GUI.BeginScrollView(new Rect(0, 0, Screen.width, 80), scroll, viewport);
				GUI.Label(labelRect, label);
				GUI.EndScrollView();
			}
		}
	}
}
