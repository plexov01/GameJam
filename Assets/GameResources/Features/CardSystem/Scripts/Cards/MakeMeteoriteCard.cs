namespace GameJob.Features.CardSystem
{
	using UnityEngine;
	[CreateAssetMenu(fileName = "MakeMeteoriteCard", menuName = "Cards/MakeMeteoriteCard")]
	public class MakeMeteoriteCard : AbstractCard
	{
		public override void ActivateCard()
		{
			base.ActivateCard();
			Debug.Log("MakeMeteoriteCard");
		}
	}
}