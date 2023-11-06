namespace GameJam.Features.CardSystem
{
	using UnityEngine;

	[CreateAssetMenu(fileName = "MarioPowerCard", menuName = "Cards/MarioPowerCard")]
	public class MarioPowerCard: AbstractCard
	{
		public float DeltaHealth = default;
		public float SpeedDivider = default;
		public float SizeMultiplier = default;
		public float DeltaDamage = default;
		public float DeltaAttackSpeed = default;
		
		public override void ActivateCard()
		{
			base.ActivateCard();
			
			if (Random.Range(0,100) < 50)
			{
				TDManager.instance.ChangeEnemiesStats(0, DeltaHealth, SpeedDivider, SizeMultiplier, DeltaDamage, DeltaAttackSpeed);
			}
			else
			{
				TDManager.instance.ChangeEnemiesStats(0, DeltaHealth, SpeedDivider, SizeMultiplier, DeltaDamage, DeltaAttackSpeed);
			}
			
			Debug.Log("MarioPowerCard");
		}
	}
}