namespace GameJam.Features.CardSystem
{
    using UnityEngine;
    /// <summary>
    /// Карта изменения башни
    /// </summary>
    [CreateAssetMenu(fileName = "UpgradeTowerCard", menuName = "Cards/UpgradeTowerCard")]
    public class UpgradeTowerCard : AbstractCard
    {
        public override void ActivateCard()
        {
            base.ActivateCard();
            TDManager.instance.ChangeTurretTier(true);
            
            SoundManager soundManager = SoundManager.Instance;
            soundManager.PlaySound(soundManager.audioClipRefsSo.Upgrade, Camera.main.transform.position);
        }
    }
}
