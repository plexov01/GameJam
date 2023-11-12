namespace GameJam.Features.CardSystem
{
	using System.Collections.Generic;
	using System.Linq;
	using UnityEngine;

	/// <summary>
	/// Карточка заморозки
	/// </summary>
	[CreateAssetMenu(fileName = "FreezeCard", menuName = "Cards/FreezeCard")]
	public class FreezeCard: AbstractCard
	{
		public float Duration = default;
		public override void ActivateCard()
		{
			base.ActivateCard();
			
			
			TDManager.instance.Freeze(Duration);
			
			SoundManager soundManager = SoundManager.Instance;
			soundManager.PlaySound(soundManager.audioClipRefsSo.freeze, Camera.main.transform.position);
			
		}
	}
}