#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace DungeonQuest
{
	public class TilemapEditor : EditorWindow
	{
		private int selectedItem;

		private string[] options = new string[]
		{
			"Map Tiles", "Enviromental", "Traps"
		};

		private GameObject[] prefabs;
		private GameObject selectedPrefab;
		private GameObject selectedGameObject;

		private List<GameObject> spawnedGameObjects = new List<GameObject>();
		private GUIContent handleContent = new GUIContent();

		private Vector2 scrollPosition;

		[MenuItem("Window/TilemapEditor")]
		static void Init()
		{
			var window = (TilemapEditor)GetWindow(typeof(TilemapEditor), false, "Tilemap Editor");

			window.position = new Rect(window.position.xMin + 100f, window.position.yMin + 100f, 500f, 250f);
		}

		void OnDisable()
		{
			SceneView.onSceneGUIDelegate -= OnSceneGUI;
		}

		void OnGUI()
		{
			selectedItem = EditorGUILayout.Popup(selectedItem, options);

			var objects = Resources.LoadAll("Prefabs/Map", typeof(GameObject));

			if (selectedItem == 0)
			{
				objects = Resources.LoadAll("Prefabs/Map/Tiles", typeof(GameObject));

			}
			else if (selectedItem == 1)
			{
				objects = Resources.LoadAll("Prefabs/Map/Enviromental", typeof(GameObject));
			}
			else if (selectedItem == 2)
			{
				objects = Resources.LoadAll("Prefabs/Map/Traps", typeof(GameObject));
			}
			
			prefabs = new GameObject[objects.Length];
			for (int i = 0; i < objects.Length; i++) prefabs[i] = (GameObject)objects[i];

			// Drawing the window

			scrollPosition = GUILayout.BeginScrollView(scrollPosition);

			GUILayout.BeginHorizontal();

			if (prefabs != null)
			{
				for (int i = 0; i < prefabs.Length; i++)
				{
					var prefabTexture = AssetPreview.GetAssetPreview(prefabs[i]);

					if (GUILayout.Button(prefabTexture, GUILayout.MaxWidth(50), GUILayout.MaxHeight(50)))
					{
						selectedPrefab = prefabs[i];
						handleContent.image = AssetPreview.GetAssetPreview(prefabs[i]);

						SceneView.onSceneGUIDelegate -= OnSceneGUI;
						SceneView.onSceneGUIDelegate += OnSceneGUI;

						FocusWindowIfItsOpen<SceneView>();
					}
				}
			}

			GUILayout.EndHorizontal();
			GUILayout.EndScrollView();
		}

		void OnSceneGUI(SceneView sceneView)
		{
			Handles.BeginGUI();

			var style = new GUIStyle();
			style.normal.textColor = Color.white;

			GUILayout.Label(selectedPrefab.name, style);
			GUILayout.Box(handleContent, GUILayout.MinWidth(40), GUILayout.MinHeight(40));

			if (GUILayout.Button("Done", GUILayout.MaxWidth(40), GUILayout.MaxHeight(20))) SceneView.onSceneGUIDelegate -= OnSceneGUI;

			Handles.EndGUI();

			var spawnPosition = new Vector2(205f, 205f);
			
			if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.A) Spawn(spawnPosition);

			if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.D)
			{
				if (spawnedGameObjects.Count > 0)
				{
					if (spawnedGameObjects[spawnedGameObjects.Count - 1] != null)
					{
						DestroyImmediate(spawnedGameObjects[spawnedGameObjects.Count - 1]);
					}

					spawnedGameObjects.RemoveAt(spawnedGameObjects.Count - 1);
				}
			}

			if (selectedGameObject != null) Handles.Label(selectedGameObject.transform.position, "X");

			if (selectedPrefab != null)
			{
				if (selectedGameObject != null && selectedGameObject.GetComponent<SpriteRenderer>())
				{
					float selectedGameObjectWidth = selectedGameObject.GetComponent<SpriteRenderer>().bounds.size.x;
					float selectedGameObjectHeight = selectedGameObject.GetComponent<SpriteRenderer>().bounds.size.y;

					float selectedPrefabWidth = selectedPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
					float selectedPrefabHeight = selectedPrefab.GetComponent<SpriteRenderer>().bounds.size.y;

					if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.UpArrow)
					{
						spawnPosition = new Vector3(selectedGameObject.transform.position.x, selectedGameObject.transform.position.y + (selectedGameObjectHeight / 2) + (selectedPrefabHeight / 2), 0);
						Spawn(spawnPosition);
					}

					if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.DownArrow)
					{
						spawnPosition = new Vector3(selectedGameObject.transform.position.x, selectedGameObject.transform.position.y - (selectedGameObjectHeight / 2) - (selectedPrefabHeight / 2), 0);
						Spawn(spawnPosition);
					}

					if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.LeftArrow)
					{
						spawnPosition = new Vector3(selectedGameObject.transform.position.x - (selectedGameObjectWidth / 2) - (selectedPrefabWidth / 2), selectedGameObject.transform.position.y, 0);
						Spawn(spawnPosition);
					}

					if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.RightArrow)
					{
						spawnPosition = new Vector3(selectedGameObject.transform.position.x + (selectedGameObjectWidth / 2) + (selectedPrefabWidth / 2), selectedGameObject.transform.position.y, 0);
						Spawn(spawnPosition);
					}
				}
			}

			if (Selection.activeGameObject != null) selectedGameObject = Selection.activeGameObject;

			SceneView.RepaintAll();
		}

		private void Spawn(Vector2 spawnPosition)
		{
			var gameObject = PrefabUtility.InstantiatePrefab(selectedPrefab) as GameObject;
			var parentObject = GameObject.Find("Map").transform;

			Selection.activeObject = gameObject;

			gameObject.name = selectedPrefab.name;
			gameObject.transform.position = spawnPosition;

			if (parentObject != null) gameObject.transform.SetParent(parentObject); 

			spawnedGameObjects.Add(gameObject);

			if (spawnedGameObjects.Count > 3) spawnedGameObjects.RemoveAt(0);
		}
	}
}
#endif
