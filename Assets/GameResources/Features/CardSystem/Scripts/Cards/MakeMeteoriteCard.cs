namespace GameJam.Features.CardSystem
{
	using System.Collections.Generic;
	using System.Linq;
	using UnityEngine;
	[CreateAssetMenu(fileName = "MakeMeteoriteCard", menuName = "Cards/MakeMeteoriteCard")]
	public class MakeMeteoriteCard : AbstractCard
	{
		public override void ActivateCard()
		{
			base.ActivateCard();
			
			float coolness = CoolnessScaleController.Instance.GetCoolness();
			
			List<GameObject> friendList = new List<GameObject>();
			GameObject aim = default;

			if (coolness < Random.Range(0f, 1f))
			{
				Debug.Log("Мыши");
				List<GameObject> walls = GameObject.FindGameObjectsWithTag("Wall").ToList();
                			
				List<GameObject> turrets = GameObject.FindGameObjectsWithTag("Turret").ToList();
                			
				List<GameObject> bomb = GameObject.FindGameObjectsWithTag("Bomb").ToList();
				
				List<GameObject> mainBase = GameObject.FindGameObjectsWithTag("MainBase").ToList();
				
				friendList.AddRange(walls);
				friendList.AddRange(turrets);
				friendList.AddRange(bomb);
				friendList.AddRange(mainBase);
				aim = friendList[Random.Range(0, friendList.Count)];
			}
			else
			{
				List<GameObject> rats = GameObject.FindGameObjectsWithTag("Enemy").ToList();
				aim = rats[Random.Range(0, rats.Count)];
			}
			
			TDManager.instance.SpawnMeteor(aim.transform.position);
			
		}
	}
}