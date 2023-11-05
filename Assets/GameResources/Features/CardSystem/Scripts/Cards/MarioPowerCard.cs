namespace GameJob.Features.CardSystem
{
	using UnityEngine;
	[CreateAssetMenu(fileName = "MarioPowerCard", menuName = "Cards/MarioPowerCard")]
	public class MarioPowerCard: AbstractCard
	{
		public override void ActivateCard()
		{
			base.ActivateCard();
			if (Random.Range(0,100)<50)
			{
				
			}
			else
			{
				
			}
			Debug.Log("MarioPowerCard");
		}
	}
}