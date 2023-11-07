namespace GameJam.Features.CardSystem
{
	using System.Collections.Generic;
	using System.Linq;
	using UnityEngine;
	
	[CreateAssetMenu(fileName = "ShortCircuitCard", menuName = "Cards/ShortCircuitCard")]
	public class ShortCircuitCard :AbstractCard
	{
		public override void ActivateCard()
		{
			base.ActivateCard();
			TDManager.instance.ShortCircuit();

			SoundManager soundManager = SoundManager.Instance;
			soundManager.PlaySound(soundManager.audioClipRefsSo.cuircut, Camera.main.transform.position);

		}
	}
}