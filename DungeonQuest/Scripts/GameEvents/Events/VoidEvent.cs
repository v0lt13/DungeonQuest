using UnityEngine;

namespace DungeonQuest.GameEvents
{
	[CreateAssetMenu(fileName = "VoidEvent", menuName = "ScriptableObjects/VoidEvent", order = 0)]
	public class VoidEvent : BaseGameEvent<Void>
	{
		public void Invoke()
		{ 
			Invoke(new Void()); 
		}
	}
}
