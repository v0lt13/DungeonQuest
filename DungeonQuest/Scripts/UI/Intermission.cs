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
		private bool breakEnumerator;

		void Start()
		{
			Time.timeScale = 1f;
			GetComponent<AudioSource>().ignoreListenerPause = true;

			StartCoroutine(DisplayText());
		}

		void Update()
		{
			if (Input.GetButtonDown("Skip"))
			{
				if (canContinue)
				{
					GameManager.LoadScene(sceneName);
				}
				else
				{
					breakEnumerator = true;
				}
			}
		}

		private IEnumerator DisplayText()
		{
			canContinue = false;

			foreach (var letter in story.ToCharArray())
			{
				if (breakEnumerator)
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
