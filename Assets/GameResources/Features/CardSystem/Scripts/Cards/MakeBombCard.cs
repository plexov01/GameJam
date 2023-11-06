namespace GameJam.Features.CardSystem
{
	using UnityEngine;

	[CreateAssetMenu(fileName = "MakeBombCard", menuName = "Cards/MakeBombCard")]
	public class MakeBombCard : AbstractCard
	{
		public override void ActivateCard()
		{
			base.ActivateCard();
			TDManager.instance.BuildMine();
			
			SoundManager soundManager = SoundManager.Instance;
			
			soundManager.PlaySound(soundManager.audioClipRefsSo.Bomb, Camera.main.transform.position);
		}
	}
}