using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DungeonQuest.GameEvents;
using DungeonQuest.Data;

namespace DungeonQuest.UI
{
	public class Dialogue : MonoBehaviour
	{
		[SerializeField] private string[] dialogue;
		[Space]
		[SerializeField] private Text diablogueText;
		[SerializeField] private GameObject prompt;

		private int currentDialogue;
		private bool canContinue;
		private bool breakEnumaretor;

		void Start()
		{
			StartCoroutine(DisplayText());
			GameManager.INSTANCE.SetGameState(GameManager.GameState.Paused);
		}

		void Update()
		{
			prompt.SetActive(canContinue);

			if (Input.GetButtonDown("Skip"))
			{
				if (canContinue)
				{
					if (currentDialogue != dialogue.Length - 1)
					{
						currentDialogue++;
						breakEnumaretor = false;

						StartCoroutine(DisplayText());
					}
					else
					{
						var gameManager = GameManager.INSTANCE;
					 
						gameManager.hasDialogue = true;
						gameManager.SetGameState(GameManager.GameState.Running);
						gameManager.gameData.SaveData(GameDataHandler.DataType.Game);

						gameObject.SetActive(false);
					}
				}
				else
				{
					breakEnumaretor = true;
				}
			}
		}

		private IEnumerator DisplayText()
		{
			canContinue = false;
			diablogueText.text = string.Empty;

			yield return StartCoroutine(WaitForRealSeconds(0.01f));

			foreach (var letter in dialogue[currentDialogue].ToCharArray())
			{
				if (breakEnumaretor)
				{
					diablogueText.text = dialogue[currentDialogue];
					canContinue = true;

					break;
				}

				diablogueText.text += letter;

				yield return StartCoroutine(WaitForRealSeconds(0.03f));
			}

			canContinue = true;
		}

		private IEnumerator WaitForRealSeconds(float seconds)
		{
			float startTime = Time.realtimeSinceStartup;

			while (Time.realtimeSinceStartup < startTime + seconds) yield return null;
		}
	}
}
