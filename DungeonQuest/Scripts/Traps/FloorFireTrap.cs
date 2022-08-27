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

		private AudioSource audioSource;

		void Awake()
		{
			audioSource = GetComponent<AudioSource>();

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

			audioSource.Play();
			fire.SetActive(true);

			yield return new WaitForSeconds(fireDeactivationInterval);

			fireActivationInterval = defaultFireActivationInterval;

			fire.SetActive(false);

			hasFireStarted = false;
		}
	}
}
