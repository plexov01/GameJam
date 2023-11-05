namespace GameJob.Features.CardSystem
{
	using UnityEngine;
	[CreateAssetMenu(fileName = "EnemyWaveCard", menuName = "Cards/EnemyWaveCard")]
	public class EnemyWaveCard:AbstractCard
	{
		public override void ActivateCard()
		{
			base.ActivateCard();
			TDManager.instance.StartCoroutine(TDManager.instance.SpawnEnemies(10));
		}
	}
}