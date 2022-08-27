using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

namespace DungeonQuest
{
	public class LoadingScreen : MonoBehaviour
	{
		[SerializeField] private Slider loadingSlider;

		public static int SCENE_INDEX = -1;
		public static string SCENE_NAME = "";

		void Start()
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;

			if (SCENE_NAME != string.Empty)
			{
				StartCoroutine(LoadingAsync(SCENE_NAME));
				SCENE_NAME = string.Empty;
			}
			else if (SCENE_INDEX != -1)
			{
				StartCoroutine(LoadingAsync(SCENE_INDEX));
				SCENE_INDEX = -1;
			}
			else
			{
				GetComponentInChildren<Text>().text = "Couldn't load scene!";

				Invoke("LoadMenu", 2f);
			}
		}

		private void LoadMenu()
		{
			SceneManager.LoadScene("MainMenu");
		}

		private IEnumerator LoadingAsync(int sceneName)
		{
			AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

			while (!operation.isDone)
			{
				loadingSlider.value = operation.progress;
				yield return null;
			}
		}

		private IEnumerator LoadingAsync(string sceneIndex)
		{
			AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

			while (!operation.isDone)
			{
				loadingSlider.value = operation.progress;
				yield return null;
			}
		}
	}
}
