namespace GameJam.Features.CardSystem
{
    using UnityEngine;
    /// <summary>
    /// Карточка постройки башни
    /// </summary>
    [CreateAssetMenu(fileName = "BuildTowerCard", menuName = "Cards/BuildTowerCard")]
    public class BuildTowerCard : AbstractCard
    {
        public override void ActivateCard()
        {
            base.ActivateCard();
            TDManager.instance.BuildTower();
        }
    }
}

