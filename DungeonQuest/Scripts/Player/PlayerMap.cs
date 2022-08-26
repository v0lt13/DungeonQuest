using UnityEngine;
using UnityEngine.UI;
using DungeonQuest.UI;
using DungeonQuest.Grid;

namespace DungeonQuest
{
	public class PlayerMap : MonoBehaviour
	{
		[SerializeField] private float scrollSensitivity;
		[Space(10f)]
		[SerializeField] private GameObject mapUI;
		[SerializeField] private GameObject playerUI;
		[SerializeField] private GameObject mapCameraObject;
		[SerializeField] private GameObject secretRoomTextObject;
		[Space(10f)]
		[SerializeField] private Text killCountText;
		[SerializeField] private Text secretCountText;
		[Space(10f)]
		[SerializeField] private AudioClip mapSFX;

		[HideInInspector] public bool isMapOn;

		private const float MAX_MAP_CAMERA_SIZE = 300f;
		private const float MIN_MAP_CAMERA_SIZE = 75f;
		
		private float mapCameraBorderX;
		private float mapCameraBorderY;

		private bool isDragging;

		private Vector3 origin;
		private Vector3 difference;

		private Camera mapCamera;
		private GameObject playerCameraObject;
		private GameObject levelStartHeader;
		private GridGenerator grid;
		private AudioSource audioSource;

		void Start()
		{
			playerCameraObject = GameObject.Find("Main Camera");
			levelStartHeader = GameObject.Find("LevelStartHeader");

			grid = GameObject.Find("GameManager").GetComponent<GridGenerator>();

			mapCamera = mapCameraObject.GetComponent<Camera>();
			audioSource = GetComponent<AudioSource>();

			audioSource.ignoreListenerPause = true;
			mapCameraObject.transform.position = playerCameraObject.transform.position;

			mapCameraBorderX = grid.gridX * 10;
			mapCameraBorderY = grid.gridY * 10;
		}

		void Update()
		{
			killCountText.text = "Kills: " + GameManager.INSTANCE.killCount.ToString() + "/" + GameManager.INSTANCE.totalKillCount.ToString();
			secretCountText.text = "Secrets: " + GameManager.INSTANCE.secretCount.ToString() + "/" + GameManager.INSTANCE.totalSecretCount.ToString();

			if (Input.GetButtonDown("Map"))
			{
				if (GameManager.INSTANCE.CurrentGameState != GameManager.GameState.Paused && !isMapOn)
				{
					EnableMap();
				}
				else if (isMapOn)
				{
					DisableMap();
				}
			}

			if (isMapOn)
			{
				mapCamera.orthographicSize += Input.mouseScrollDelta.x * scrollSensitivity;
				mapCamera.orthographicSize -= Input.mouseScrollDelta.y * scrollSensitivity;

				if (Input.GetKeyDown(KeyCode.C)) mapCameraObject.transform.position = playerCameraObject.transform.position;

				if (mapCamera.orthographicSize >= MAX_MAP_CAMERA_SIZE) mapCamera.orthographicSize = MAX_MAP_CAMERA_SIZE;

				if (mapCamera.orthographicSize <= MIN_MAP_CAMERA_SIZE) mapCamera.orthographicSize = MIN_MAP_CAMERA_SIZE;

				if (secretRoomTextObject != null) secretRoomTextObject.SetActive(false);
			}
		}

		void LateUpdate()
		{
			if (Input.GetButton("Attack") && isMapOn)
			{
				difference = mapCamera.ScreenToWorldPoint(Input.mousePosition) - mapCamera.transform.position;

				if (!isDragging)
				{
					isDragging = true;
					origin = mapCamera.ScreenToWorldPoint(Input.mousePosition);
				}
			}
			else
			{
				isDragging = false;
			}

			if (isDragging) mapCamera.transform.position = origin - difference;

			// Stop camera at the borders
			if (mapCamera.transform.position.x > mapCameraBorderX) mapCamera.transform.position = new Vector3(mapCameraBorderX, mapCamera.transform.position.y, mapCamera.transform.position.z);

			if (mapCamera.transform.position.y > mapCameraBorderY) mapCamera.transform.position = new Vector3(mapCamera.transform.position.x, mapCameraBorderY, mapCamera.transform.position.z);

			if (mapCamera.transform.position.x < 0) mapCamera.transform.position = new Vector3(0, mapCamera.transform.position.y, mapCamera.transform.position.z);

			if (mapCamera.transform.position.y < 0) mapCamera.transform.position = new Vector3(mapCamera.transform.position.x, 0, mapCamera.transform.position.z);
		}

		private void EnableMap()
		{
			isMapOn = true;
			AudioListener.pause = true;

			mapUI.SetActive(true);
			mapCameraObject.SetActive(true);

			playerUI.SetActive(false);
			playerCameraObject.SetActive(false);

			audioSource.PlayOneShot(mapSFX);

			GameManager.INSTANCE.SetGameState(GameManager.GameState.Paused);
		}

		private void DisableMap()
		{
			isMapOn = false;
			AudioListener.pause = false;

			mapUI.SetActive(false);
			mapCameraObject.SetActive(false);

			playerUI.SetActive(true);
			playerCameraObject.SetActive(true);

			if (levelStartHeader != null) Destroy(levelStartHeader);

			GameManager.INSTANCE.SetGameState(GameManager.GameState.Running);
		}
	}
}
