namespace GameJam.Features.CardSystem
{
	using UI;
	using UnityEngine;

	[CreateAssetMenu(fileName = "MakeDarkCard", menuName = "Cards/MakeDarkCard")]
	public class MakeDarkCard: AbstractCard
	{
		public override void ActivateCard()
		{
			base.ActivateCard();
			FindObjectOfType<DarkController>()?.ShowDark();
			SoundManager soundManager = SoundManager.Instance;
			soundManager.PlaySound(soundManager.audioClipRefsSo.tma, Camera.main.transform.position);
		}
	}
}