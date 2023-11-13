namespace GameJam.Features.CardSystem
{
	using UnityEngine;
	[CreateAssetMenu(fileName = "MakeRepairCard", menuName = "Cards/MakeRepairCard")]
	public class MakeRepairCard: AbstractCard
	{
		public override void ActivateCard()
		{
			base.ActivateCard();
			TDManager.instance.Repair();
		}
	}
}