using System;

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
			TDManager.instance.ShortCircuit();
		}
	}
}