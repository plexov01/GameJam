namespace GameJob.Features.CardSystem
{
	using UnityEngine;

	[CreateAssetMenu(fileName = "MakeBombCard", menuName = "Cards/MakeBombCard")]
	public class MakeBombCard : AbstractCard
	{
		public override void ActivateCard()
		{
			base.ActivateCard();
			Debug.Log("MakeBombCard");
		}
	}
}