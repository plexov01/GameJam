namespace GameJob.Features.CardSystem
{
	using UnityEngine;

	[CreateAssetMenu(fileName = "MakeDarkCard", menuName = "Cards/MakeDarkCard")]
	public class MakeDarkCard: AbstractCard
	{
		public override void ActivateCard()
		{
			Debug.Log("MakeDarkCard");
		}
	}
}