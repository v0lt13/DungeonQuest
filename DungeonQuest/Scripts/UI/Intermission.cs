using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace DungeonQuest.UI
{
	public class Intermission : MonoBehaviour
	{
		[SerializeField, TextArea] private string story;
		[SerializeField] private string sceneName;
		[Space(10f)]
		[SerializeField] private Text storyText;

		private bool canContinue;

		void Start()
		{
			Time.timeScale = 1f;
			audio.ignoreListenerPause = true;

			StartCoroutine(DisplayText());
		}

		void Update()
		{
			if (Input.GetButtonDown("Skip") && canContinue)
			{
				GameManager.LoadScene(sceneName);
			}
		}

		private IEnumerator DisplayText()
		{
			canContinue = false;

			foreach (var letter in story.ToCharArray())
			{
				if (Input.GetButtonDown("Skip"))
				{
					storyText.text = story;
					canContinue = true;
					break;
				}

				storyText.text += letter;
				yield return new WaitForSeconds(0.05f);
			}

			canContinue = true;
		}
	}
}
