namespace GameJam.Features.CardSystem
{
	using System.Collections.Generic;
	using System.Linq;
	using UnityEngine;
	/// <summary>
	/// Карта изменения башни
	/// </summary>
	[CreateAssetMenu(fileName = "ImproveTowerCard", menuName = "Cards/ImproveTowerCard")]
	public class ImproveTowerCard: AbstractCard
	{
		public override void ActivateCard()
		{
			base.ActivateCard();
			List<GameObject> towers = GameObject.FindGameObjectsWithTag("Turret").ToList();
			GameObject tower = towers[Random.Range(0, towers.Count)];
			Transform transform = tower.transform;
			Quaternion quaternion = tower.transform.rotation;
			if (Random.Range(0,100) < 50)
			{
				Destroy(tower);
				
				// TDManager.instance.ChangeEnemiesStats(0, DeltaHealth, DeltaSpeed, Size, DeltaDamage, DeltaAttackSpeed);
			}
			else
			{
				// TDManager.instance.ChangeEnemiesStats(0, DeltaHealth, DeltaSpeed, Size, DeltaDamage, DeltaAttackSpeed);
			}
		}
	}
}