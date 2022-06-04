using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DungeonQuest.GameEvents;

namespace DungeonQuest.UI
{
	public class Dialogue : MonoBehaviour
	{
		[SerializeField] private string[] dialogue;
		[Space(10f)]
		[SerializeField] private Text diablogueText;
		[SerializeField] private GameObject prompt;

		private int currentDialogue;
		private bool canContinue;

		void Start()
		{
			StartCoroutine(DisplayText());
			GameManager.INSTANCE.SetGameState(GameManager.GameState.Paused);
		}

		void Update()
		{
			prompt.SetActive(canContinue);

			if (Input.GetButtonDown("Skip") && canContinue)
			{
				if (currentDialogue != dialogue.Length - 1)
				{
					currentDialogue++;

					StartCoroutine(DisplayText());
				}
				else
				{
					var gameManager = GameManager.INSTANCE;
					 
					gameManager.hasDialogue = true;
					gameManager.SetGameState(GameManager.GameState.Running);
					gameManager.gameData.SaveGameData();

					gameObject.SetActive(false);
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
				if (Input.GetButtonDown("Skip"))
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
