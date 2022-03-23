using System.Collections;

namespace DungeonQuest.Traps
{
	public interface ITrap
	{
		IEnumerator TriggerTrap();
	}
}
