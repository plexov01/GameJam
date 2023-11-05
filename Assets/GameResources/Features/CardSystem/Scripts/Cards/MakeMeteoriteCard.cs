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
			TDManager.instance.SpawnMeteor();
			
			//шкала крутости
			// FindGa
			// GameObject.Find
			// List<GameObject> rats = GameObject.FindGameObjectsWithTag("Enemy").ToList();
			//
			// List<GameObject> walls = GameObject.FindGameObjectsWithTag("Wall").ToList();
			//
			// List<GameObject> turrets = GameObject.FindGameObjectsWithTag("Turret").ToList();
			//
			// List<GameObject> turrets = GameObject.FindGameObjectsWithTag("Turret").ToList();
		}
	}
}