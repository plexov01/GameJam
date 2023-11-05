namespace GameJam.Features.CardSystem
{
	using UnityEngine;
	[CreateAssetMenu(fileName = "MakeLavaCard", menuName = "Cards/MakeLavaCard")]
	public class MakeLavaCard: AbstractCard
	{
		public float duration = default;
		public override void ActivateCard()
		{
			base.ActivateCard();
			TDManager.instance.LavaFloor(duration);
		}
	}
}