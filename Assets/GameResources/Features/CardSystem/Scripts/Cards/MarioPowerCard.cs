namespace GameJam.Features.CardSystem
{
	using UnityEngine;
	using UnityEngine.Serialization;

	[CreateAssetMenu(fileName = "MarioPowerCard", menuName = "Cards/MarioPowerCard")]
	public class MarioPowerCard: AbstractCard
	{
		public float DeltaHealth = default;
		public float SpeedDivider = default;
		[Min(0.01f)] public float Size = 1;
		public float DeltaDamage = default;
		[Min(0.01f)] public float DeltaAttackSpeed = default;
		
		public override void ActivateCard()
		{
			base.ActivateCard();
			
			if (Random.Range(0,100) < 50)
			{
				TDManager.instance.ChangeEnemiesStats(0, DeltaHealth, SpeedDivider, Size, DeltaDamage, DeltaAttackSpeed);
			}
			else
			{
				TDManager.instance.ChangeEnemiesStats(0, DeltaHealth, SpeedDivider, Size, DeltaDamage, DeltaAttackSpeed);
			}
			
			Debug.Log("MarioPowerCard");
		}
	}
}