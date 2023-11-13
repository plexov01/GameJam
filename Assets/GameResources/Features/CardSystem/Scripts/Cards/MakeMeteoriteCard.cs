namespace GameJam.Features.CardSystem
{
	using System.Collections.Generic;
	using System.Linq;
	using UnityEngine;
	[CreateAssetMenu(fileName = "MakeMeteoriteCard", menuName = "Cards/MakeMeteoriteCard")]
	public class MakeMeteoriteCard : AbstractCard
	{
		public override void ActivateCard()
		{
			base.ActivateCard();
			
			TDManager.instance.SpawnMeteor();
			
			// SoundManager soundManager = SoundManager.Instance;
			// soundManager.PlaySound(soundManager.audioClipRefsSo.meteor, Camera.main.transform.position);
			// soundManager.PlaySound(soundManager.audioClipRefsSo.attackRats,Camera.main.transform.position);
			
		}
	}
}