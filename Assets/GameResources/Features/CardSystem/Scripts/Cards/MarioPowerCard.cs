namespace GameJam.Features.CardSystem
{
	using UnityEngine;

	[CreateAssetMenu(fileName = "MarioPowerCard", menuName = "Cards/MarioPowerCard")]
	public class MarioPowerCard: AbstractCard
	{
		public float DeltaHealth = default;
		public float DeltaSpeed = default;
		[Min(0.01f)] public float Size = 1;
		public float DeltaDamage = default;
		[Min(0.01f)] public float DeltaAttackSpeed = default;
		
		public override void ActivateCard()
		{
			base.ActivateCard();
			
			if (Random.Range(0,100) < 50)
			{
				TDManager.instance.ChangeEnemiesStats(0, DeltaHealth, DeltaSpeed, Size, DeltaDamage, DeltaAttackSpeed);
			}
			else
			{
				TDManager.instance.ChangeEnemiesStats(0, DeltaHealth, DeltaSpeed, Size, DeltaDamage, DeltaAttackSpeed);
			}
			
			Debug.Log("MarioPowerCard");
		}
	}
}