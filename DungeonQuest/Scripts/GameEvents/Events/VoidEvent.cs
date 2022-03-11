using UnityEngine;

namespace DungeonQuest.GameEvents
{
	public class VoidEvent : BaseGameEvent<Void>
	{
		public void Invoke()
		{ 
			Invoke(new Void()); 
		}

		#if UNITY_EDITOR
		[UnityEditor.MenuItem("GameEvents/VoidEvent")]
		private static void CreateVoidEvent()
		{
			var @event = ScriptableObject.CreateInstance<VoidEvent>();
			UnityEditor.AssetDatabase.CreateAsset(@event, "Assets/Events/Void Event.asset");
		}
		#endif
	}
}
