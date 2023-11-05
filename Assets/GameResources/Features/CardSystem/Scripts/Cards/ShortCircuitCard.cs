namespace GameJam.Features.CardSystem
{
	using UnityEngine;
	[CreateAssetMenu(fileName = "ShortCircuitCard", menuName = "Cards/ShortCircuitCard")]
	public class ShortCircuitCard :AbstractCard
	{
		public override void ActivateCard()
		{
			base.ActivateCard();
			Debug.Log("ShortCircuitCard");
		}
	}
}