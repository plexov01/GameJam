namespace GameJob.Features.CardSystem
{
	using UnityEngine;
	[CreateAssetMenu(fileName = "MarioPowerCard", menuName = "Cards/MarioPowerCard")]
	public class MarioPowerCard: AbstractCard
	{
		public override void ActivateCard()
		{
			Debug.Log("MarioPowerCard");
		}
	}
}