namespace GameJam.Features.CardSystem
{
	using System.Collections.Generic;
	using System.Linq;
	using UnityEngine;
	
	[CreateAssetMenu(fileName = "ShortCircuitCard", menuName = "Cards/ShortCircuitCard")]
	public class ShortCircuitCard :AbstractCard
	{
		public override void ActivateCard()
		{
			base.ActivateCard();

			float coolness = CoolnessScaleController.Instance.GetCoolness();
			
			List<GameObject> walls = GameObject.FindGameObjectsWithTag("Wall").ToList();
			for (int i = 0; i < walls.Count; i++)
			{
				if (Random.Range(0f,1f) > coolness)
				{
					Destroy(walls[i]);
				}	
			}
			
			List<GameObject> turrets = GameObject.FindGameObjectsWithTag("Turret").ToList();
			for (int i = 0; i < turrets.Count; i++)
			{
				if (Random.Range(0f,1f) > coolness)
				{
					Destroy(turrets[i]);
				}	
			}
                            			
			List<GameObject> bomb = GameObject.FindGameObjectsWithTag("Bomb").ToList();
			for (int i = 0; i < bomb.Count; i++)
			{
				if (Random.Range(0f,1f) > coolness)
				{
					Destroy(bomb[i]);
				}	
			}
			
			//main base не уничтожается
			// List<GameObject> mainBase = GameObject.FindGameObjectsWithTag("MainBase").ToList();
			// for (int i = 0; i < walls.Count; i++)
			// {
			// 	if (Random.Range(0f,1f) > coolness)
			// 	{
			// 		Destroy(mainBase[i]);
			// 	}	
			// }
			
			List<GameObject> rats = GameObject.FindGameObjectsWithTag("Enemy").ToList();
			
			for (int i = 0; i < rats.Count; i++)
			{
				if (Random.Range(0f,1f) < coolness)
				{
					rats[i].GetComponentInChildren<Health>().TakeDamage(1000000);
					Destroy(rats[i]);
				}	
			}
			SoundManager soundManager = SoundManager.Instance;
			soundManager.PlaySound(soundManager.audioClipRefsSo.cuircut, Camera.main.transform.position);

		}
	}
}