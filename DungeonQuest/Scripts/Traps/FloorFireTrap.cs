using UnityEngine;
using System.Collections;

namespace DungeonQuest.Traps
{
	public class FloorFireTrap : MonoBehaviour 
	{
		[SerializeField] private float defaultFireActivationInterval;
		[SerializeField] private float fireDeactivationInterval;
		[SerializeField] private GameObject fire;

		private bool hasFireStarted;
		private float fireActivationInterval;

		void Awake()
		{
			fireActivationInterval = defaultFireActivationInterval;
		}

		void Update()
		{
			if (!hasFireStarted && fireActivationInterval <= 0f)
			{
				StartCoroutine(ToogleFire());
			}
			else if (fireActivationInterval > 0f)
			{
				fireActivationInterval -= Time.deltaTime;
			}
		}
		
		private IEnumerator ToogleFire()
		{
			hasFireStarted = true;

			audio.Play();
			fire.SetActive(true);

			yield return new WaitForSeconds(fireDeactivationInterval);

			fireActivationInterval = defaultFireActivationInterval;

			fire.SetActive(false);

			hasFireStarted = false;
		}
	}
}
